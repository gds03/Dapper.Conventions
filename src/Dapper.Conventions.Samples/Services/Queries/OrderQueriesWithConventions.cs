using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper.Conventions.Samples.Interfaces;
using Dapper.Conventions.Attributes;
using Dapper.Conventions.Interfaces;
using Dapper.Conventions.Samples.DTO;

namespace Dapper.Conventions.Samples.Services.Queries
{
    [UseConventions("Query/Orders")]
    public class OrderQueriesWithConventions : IOrderQueries
    {
        const string ConnectionString = "Data Source=localhost,1433;Initial Catalog=Test;User Id=sa;Password=Summer2020";

        private IConventionsLookup<OrderQueriesWithConventions> conventionsLookup;

        public OrderQueriesWithConventions(IConventionsLookup<OrderQueriesWithConventions> conventionsLookup)
        {
            this.conventionsLookup = conventionsLookup;
        }


        public IEnumerable<OrderDetails> GetAll()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<OrderDetails>(conventionsLookup.GetQuery());
            }
        }

        [OverrideConventions("GetHigherThan200")]
        public IEnumerable<OrderDetails> GetComplexFiltered(string partOfDetails)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<OrderDetails>(conventionsLookup.GetQuery());
            }
        }

        public OrderDetails GetSingle(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.QuerySingle<OrderDetails>(conventionsLookup.GetQuery(), new { Id = id });
            }
        }
    }
}
