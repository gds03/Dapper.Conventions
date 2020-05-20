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
        private ICommandExecutor<OrderQueriesWithConventions> conventions;

        public OrderQueriesWithConventions(ICommandExecutor<OrderQueriesWithConventions> conventions)
        {
            this.conventions = conventions;
        }


        public IEnumerable<OrderDetails> GetAll() =>
            conventions._(sql =>
            {
                using (var conn = new SqlConnection())
                {
                    return conn.Query<OrderDetails>(sql);
                }
            }); 


        [OverrideConventions("GetHigherThan200")]
        public IEnumerable<OrderDetails> GetComplexFiltered(string partOfDetails) =>
            conventions._((sql, conn) => conn.Query<OrderDetails>(sql));

        public OrderDetails GetSingle(int id) => 
            conventions._((sql, conn) => conn.QuerySingle<OrderDetails>(sql, new { Id = id }));
    }
}
