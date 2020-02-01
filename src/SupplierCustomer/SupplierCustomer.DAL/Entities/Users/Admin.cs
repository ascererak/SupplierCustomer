using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SupplierCustomer.DAL.Entities.Users
{
    internal class Admin
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
