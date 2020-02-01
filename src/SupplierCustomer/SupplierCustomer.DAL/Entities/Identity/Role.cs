using Microsoft.AspNetCore.Identity;

namespace SupplierCustomer.DAL.Entities.Identity
{
    internal class Role : IdentityRole<int>
    {
        public Role(string name)
            : base(name)
        {
        }
    }
}
