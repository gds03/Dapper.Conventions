using Dapper.Conventions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Conventions.Domain
{
    public class SqlFileExtension : IDbFileExtension
    {
        public string Extension => ".sql";
    }
}
