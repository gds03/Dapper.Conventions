using Dapper.Conventions.Samples.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Conventions.Samples.Interfaces
{
    public interface IOrderQueries
    {
        IEnumerable<OrderDetails> GetAll();

        OrderDetails GetSingle(int id);

        IEnumerable<OrderDetails> GetComplexFiltered(string partOfDetails);
    }
}
