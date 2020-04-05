using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Conventions.Samples.DTO
{
    public class OrderDetails
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public bool HasDiscount { get; set; }

    }
}
