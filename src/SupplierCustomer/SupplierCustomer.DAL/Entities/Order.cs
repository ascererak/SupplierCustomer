using System;
using System.Collections.Generic;
using System.Text;

namespace SupplierCustomer.DAL.Entities
{
    internal class Order
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string Amount { get; set; }
    }
}
