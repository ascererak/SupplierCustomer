using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupplierCustomer.DAL;
using SupplierCustomer.DAL.Entities.Identity;
using SupplierCustomer.DAL.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace SupplierCustomer.WebApi.Extensions
{
    public static class ServiceCollectionExtentionMethods
    {
        public static IConfiguration Configuration { get; }

        public static void AddSupplierCustormer(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //serviceCollection.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            serviceCollection.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            //serviceCollection.AddTransient<IDatabaseSeeder, DatabaseSeeder>();
            serviceCollection.AddTransient<JwtSecurityTokenHandler>();

            serviceCollection
                .AddIdentity<User, IdentityRole<int>>(options => { options.User.RequireUniqueEmail = true; })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            serviceCollection.AddDbContext<ApplicationDbContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            serviceCollection.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            });
        }
    }
}
