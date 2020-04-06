using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace Dapper.Conventions.Interfaces
{
    public interface IQueryExecutor<TUsingConventions>
    {
        T Run<T>(Func<string, T> callback, [CallerMemberName] string methodName = null);

        T Run<T>(Func<string, IDbConnection, T> callback, [CallerMemberName] string methodName = null);
    }
}
