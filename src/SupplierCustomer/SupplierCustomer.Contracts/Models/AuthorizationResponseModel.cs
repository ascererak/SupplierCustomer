using System;
using System.Collections.Generic;
using System.Text;

namespace SupplierCustomer.Contracts.Models
{
    public class AuthorizationResponseModel
    {
        public bool IsSuccessful { get; set; }

        public string Token { get; set; }

        public string Message { get; set; }
    }
}
