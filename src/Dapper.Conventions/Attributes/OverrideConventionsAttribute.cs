using System;

namespace Dapper.Conventions.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OverrideConventionsAttribute : Attribute
    {
        public string FileName { get; }

        public OverrideConventionsAttribute(string fileName)
        {
            this.FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }
    }
}
