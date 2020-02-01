using System;
using System.Collections.Generic;
using System.Text;

namespace SupplierCustomer.Contracts.Models
{
    public class SessionData
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }
    }
}
