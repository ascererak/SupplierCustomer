using System.ComponentModel.DataAnnotations.Schema;

namespace SupplierCustomer.DAL.Entities
{
    internal class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [ForeignKey("Order")]
        public string OrderId { get; set; }
    }
}
