using System;
using System.Linq;

namespace Dapper.Conventions.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OverrideConventionsAttribute : Attribute
    {
        public string FileName { get; }

        public OverrideConventionsAttribute(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException(nameof(fileName));
            }

            fileName = fileName.Trim();
            if (fileName.ToCharArray().Any(c => char.IsWhiteSpace(c)))
            {
                throw new InvalidOperationException($"subFolder parameter can't contain spaces.");
            }

            this.FileName = fileName;
        }
    }
}
