using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper.Conventions.Samples.Interfaces;
using Dapper.Conventions.Samples.DTO;

namespace Dapper.Conventions.Samples.Services.Queries
{
    public class OrderQueries : IOrderQueries
    {
        const string ConnectionString = "Data Source=localhost,1433;Initial Catalog=Test;User Id=sa;Password=Summer2020";


        public IEnumerable<OrderDetails> GetAll()
        {
            var sql = @"SELECT * FROM OrderDetails";

            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<OrderDetails>(sql);
            }
        }

        public IEnumerable<OrderDetails> GetComplexFiltered(string partOfDetails)
        {
            var sql = @"SELECT * FROM OrderDetails
                        where price > 200";

            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<OrderDetails>(sql);
            }
        }

        public OrderDetails GetSingle(int id)
        {
            var sql = @"SELECT * FROM OrderDetails where Id = @Id";

            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.QuerySingle<OrderDetails>(sql);
            }
        }
    }
}
