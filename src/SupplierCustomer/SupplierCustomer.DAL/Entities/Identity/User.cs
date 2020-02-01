using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplierCustomer.DAL.Entities.Identity
{
    internal class User : IdentityUser<int>
    {
        [ForeignKey("IdentityRole<int>")]
        public int RoleId { get; set; }
    }
}
