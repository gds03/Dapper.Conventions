using System.Collections.Generic;
using Dapper.Conventions.Samples.Interfaces;
using Dapper.Conventions.Attributes;
using Dapper.Conventions.Interfaces;
using Dapper.Conventions.Samples.DTO;
using System.Data.SqlClient;

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
            conventionsQuery._(sql =>
            {
                using (var conn = new SqlConnection())
                {
                    return conn.Query<OrderDetails>(sql);
                }
            }); 


        [OverrideConventions("GetHigherThan200")]
        public IEnumerable<OrderDetails> GetComplexFiltered(string partOfDetails) =>
            conventionsQuery._((sql, conn) => conn.Query<OrderDetails>(sql));

        public OrderDetails GetSingle(int id) => 
            conventionsQuery._((sql, conn) => conn.QuerySingle<OrderDetails>(sql, new { Id = id }));
    }
}
