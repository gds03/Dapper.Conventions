using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace Dapper.Conventions.Interfaces
{
    public interface ICommandExecutor<TUsingConventions>
    {
        T _<T>(Func<string, T> callback, [CallerMemberName] string methodNameOrCommand = null);

        T _<T>(Func<string, IDbConnection, T> callback, [CallerMemberName] string methodNameOrCommand = null);
    }
}
