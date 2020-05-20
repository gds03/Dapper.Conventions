using Dapper.Conventions.Interfaces;
using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace Dapper.Conventions
{
    public class CommandExecutor<TUsingConventions> : ICommandExecutor<TUsingConventions>
    {
        private readonly Func<IDbConnection> dbConnectionFactory;
        private readonly IConventionsLookup<TUsingConventions> conventionsLookup;

        public CommandExecutor(IConventionsLookup<TUsingConventions> conventionsLookup)
        {
            this.conventionsLookup = conventionsLookup ?? throw new ArgumentNullException(nameof(conventionsLookup));
        }

        public CommandExecutor(IConventionsLookup<TUsingConventions> conventionsLookup, Func<IDbConnection> dbConnectionFactory) : this(conventionsLookup)
        {
            this.dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
            
        }

        public T _<T>(Func<string, T> callback, [CallerMemberName] string methodNameOrCommand = null)
        {
            var sql = conventionsLookup.GetCommandFor(methodNameOrCommand);
            return callback(sql);
        }

        public T _<T>(Func<string, IDbConnection, T> callback, [CallerMemberName] string methodNameOrCommand = null)
        {
            if(dbConnectionFactory == null)
            {
                throw new InvalidOperationException("For this function to be used, please call the constructor passing a connection factory");
            }

            using (var dbConnection = dbConnectionFactory())
            {
                return this._(sql => callback(sql, dbConnection), methodNameOrCommand);
            }
        }
    }
}
