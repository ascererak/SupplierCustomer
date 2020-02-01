using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SupplierCustomer.DAL.Entities;
using SupplierCustomer.DAL.Entities.Identity;
using SupplierCustomer.DAL.Entities.Users;
using SupplierCustomer.DAL.Interfaces;

namespace SupplierCustomer.DAL
{
    class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
               : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Operator> Operators { get; set; }

        public DbSet<Viewer> Viewers { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Ignore(u => u.AccessFailedCount)
                                  .Ignore(u => u.NormalizedUserName)
                                  .Ignore(u => u.PhoneNumber)
                                  .Ignore(u => u.PhoneNumberConfirmed)
                                  .Ignore(u => u.TwoFactorEnabled);

            builder.Entity<Admin>().HasKey(u => u.Id);
            builder.Entity<Operator>().HasKey(u => u.Id);
            builder.Entity<Viewer>().HasKey(u => u.Id);
            builder.Entity<Customer>().HasKey(u => u.Id);
            builder.Entity<Delivery>().HasKey(u => u.Id);
            builder.Entity<Order>().HasKey(u => u.Id);
        }
    }
}
