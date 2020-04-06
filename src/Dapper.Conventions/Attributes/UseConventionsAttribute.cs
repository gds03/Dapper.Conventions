using System;
using System.Linq;

namespace Dapper.Conventions.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UseConventionsAttribute : Attribute
    {
        public string SubFolder { get; }

        public UseConventionsAttribute()
        {

        }

        public UseConventionsAttribute(string subFolder)
        {
            if(string.IsNullOrEmpty(subFolder))
            {
                throw new ArgumentException(nameof(subFolder));
            }

            subFolder = subFolder.Trim();
            if(subFolder.ToCharArray().Any(c => char.IsWhiteSpace(c)))
            {
                throw new InvalidOperationException($"subFolder parameter can't contain spaces.");
            }

            this.SubFolder = subFolder;
        }
    }
}
