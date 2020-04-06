using System.Collections.Generic;
using Dapper.Conventions.Samples.Interfaces;
using Dapper.Conventions.Attributes;
using Dapper.Conventions.Interfaces;
using Dapper.Conventions.Samples.DTO;

namespace Dapper.Conventions.Samples.Services.Queries
{
    [UseConventions("Query/Orders")]
    public class OrderQueriesWithConventions : IOrderQueries
    {
        private IQueryExecutor<OrderQueriesWithConventions> conventionsQuery;

        public OrderQueriesWithConventions(IQueryExecutor<OrderQueriesWithConventions> conventionsQuery)
        {
            this.conventionsQuery = conventionsQuery;
        }


        public IEnumerable<OrderDetails> GetAll() => 
            conventionsQuery.Run((sql, conn) => conn.Query<OrderDetails>(sql)
        );


        [OverrideConventions("GetHigherThan200")]
        public IEnumerable<OrderDetails> GetComplexFiltered(string partOfDetails) =>
            conventionsQuery.Run((sql, conn) => conn.Query<OrderDetails>(sql)
        );

        public OrderDetails GetSingle(int id) => 
            conventionsQuery.Run((sql, conn) => conn.QuerySingle<OrderDetails>(sql, new { Id = id })
        );
    }
}
