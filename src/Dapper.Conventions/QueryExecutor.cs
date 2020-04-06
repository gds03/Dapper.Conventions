using Dapper.Conventions.Interfaces;
using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace Dapper.Conventions
{
    public class QueryExecutor<TUsingConventions> : IQueryExecutor<TUsingConventions>
    {
        private readonly Func<IDbConnection> dbConnectionFactory;
        private readonly IConventionsLookup<TUsingConventions> conventionsLookup;

        public QueryExecutor(IConventionsLookup<TUsingConventions> conventionsLookup)
        {
            this.conventionsLookup = conventionsLookup ?? throw new ArgumentNullException(nameof(conventionsLookup));
        }

        public QueryExecutor(IConventionsLookup<TUsingConventions> conventionsLookup, Func<IDbConnection> dbConnectionFactory) : this(conventionsLookup)
        {
            this.dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
            
        }

        public T Run<T>(Func<string, T> callback, [CallerMemberName] string methodName = null)
        {
            var sql = conventionsLookup.GetQuery(methodName);
            return callback(sql);
        }

        public T Run<T>(Func<string, IDbConnection, T> callback, [CallerMemberName] string methodName = null)
        {
            if(dbConnectionFactory == null)
            {
                throw new InvalidOperationException("For this function to be used, please call the constructor passing a connection factory");
            }

            using (var dbConnection = dbConnectionFactory())
            {
                return this.Run(sql => callback(sql, dbConnection), methodName);
            }
        }
    }
}
