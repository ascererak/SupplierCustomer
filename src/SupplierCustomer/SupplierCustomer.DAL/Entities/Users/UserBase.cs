using System.ComponentModel.DataAnnotations.Schema;

namespace SupplierCustomer.DAL.Entities.Users
{
    internal class UserBase
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}