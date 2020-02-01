using Microsoft.EntityFrameworkCore;
using SupplierCustomer.DAL.Entities;
using SupplierCustomer.DAL.Entities.Users;
using System.Threading;
using System.Threading.Tasks;

namespace SupplierCustomer.DAL.Interfaces
{
    internal interface IApplicationDbContext
    {
        DbSet<Admin> Admins { get; set; }

        DbSet<Operator> Operators { get; set; }

        DbSet<Viewer> Viewers { get; set; }

        DbSet<Customer> Customers { get; set; }

        DbSet<Delivery> Deliveries { get; set; }

        DbSet<Order> Orders { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        int SaveChanges();
    }
}
