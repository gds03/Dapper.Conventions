using System;

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
            this.SubFolder = subFolder ?? throw new ArgumentNullException(nameof(subFolder));
        }
    }
}
