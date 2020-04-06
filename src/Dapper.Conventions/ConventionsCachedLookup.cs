using Dapper.Conventions.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Dapper.Conventions.Attributes;
using System.IO;
using System.Linq;

namespace Dapper.Conventions
{

    public class ConventionsCachedLookup<T> : IConventionsLookup<T>
    {
        private readonly string directoryName;
        private readonly string fileExtensions;
        private readonly Dictionary<string, string> methodNameToFileContentsMap;         

        public ConventionsCachedLookup(string baseDirectory, string fileExtensions)
        {
            var conventionAttribute = typeof(T).GetCustomAttribute<UseConventionsAttribute>();
            if (conventionAttribute == null)
            {
                throw new InvalidOperationException($"To use this service please Mark {typeof(T).Name} class with {typeof(UseConventionsAttribute).Name} attribute");
            }

            if (string.IsNullOrEmpty(baseDirectory))
            {
                throw new ArgumentException(nameof(baseDirectory));
            }

            if (string.IsNullOrEmpty(fileExtensions))
            {
                throw new ArgumentException(nameof(fileExtensions));
            }            

            var folder = conventionAttribute.SubFolder ?? typeof(T).Name;
            this.fileExtensions = fileExtensions;
            this.directoryName = Path.Combine(baseDirectory.Replace("/", "\\"), folder.Replace("/", "\\"));

            if (!Directory.Exists(directoryName))
            {
                throw new DirectoryNotFoundException($"Directory {directoryName} does not exists");
            }

            methodNameToFileContentsMap = MapFiles();
        }


        public string GetQuery([CallerMemberName] string methodName = null)
        {
            if( !methodNameToFileContentsMap.TryGetValue(methodName, out var queryValue) )
            {
                throw new InvalidOperationException();
            }

            return queryValue;
        }



        private Dictionary<string, string> MapFiles()
        {
            var mapping = new Dictionary<string, string>();            
            var fileExceptions = new List<Exception>();

            foreach (var method in typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var value = method.Name;

                if (method.GetCustomAttribute<OverrideConventionsAttribute>() is OverrideConventionsAttribute overrideAttr)
                {
                    value = overrideAttr.FileName;
                }

                var fileException = GetFileContentsForMethod(GetFilePath(value), out var fileContents);
                if(fileException != null)
                {
                    fileExceptions.Add(fileException);
                }
                else
                {
                    mapping.Add(method.Name, fileContents);
                }                
            }

            if(fileExceptions.Any())
            {
                throw new AggregateException(fileExceptions);
            }
            
            return mapping;
        }

        private Exception GetFileContentsForMethod(string finalFilePath, out string fileContents)
        {
            fileContents = null;
            try
            {
                fileContents = File.ReadAllText(finalFilePath);
                return null;
            }
            catch(Exception ex)
            {
                return ex;
            }
        }


        private string GetFilePath(string name) => Path.ChangeExtension(Path.Combine(directoryName, name), fileExtensions);

    }
}
