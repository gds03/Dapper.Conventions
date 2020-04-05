using Dapper.Conventions.Samples.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Conventions.Samples
{
    public interface IApplication
    {
        void Run();
    }
    public class Application : IApplication
    {
        IOrderQueries orderQueries;

        public Application(IOrderQueries orderQueries)
        {
            this.orderQueries = orderQueries;
        }


        public void Run()
        {
            Console.WriteLine("GET ALL");
            foreach(var orderDetail in orderQueries.GetAll())
            {
                Console.WriteLine($"ID {orderDetail.Id}, description {orderDetail.Description}, price {orderDetail.Price} hasdiscount {orderDetail.HasDiscount}");
            }


            Console.WriteLine("GET Higher than 200");
            foreach (var orderDetail2 in orderQueries.GetComplexFiltered(null))
            {
                Console.WriteLine($"ID {orderDetail2.Id}, description {orderDetail2.Description}, price {orderDetail2.Price} hasdiscount {orderDetail2.HasDiscount}");
            }

            Console.WriteLine("GET Single Id 2");
            var orderDetailSingle = orderQueries.GetSingle(2);
            Console.WriteLine($"ID {orderDetailSingle.Id}, description {orderDetailSingle.Description}, price {orderDetailSingle.Price} hasdiscount {orderDetailSingle.HasDiscount}");
        }
    }
}
