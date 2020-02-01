using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupplierCustomer.WebApi.Extensions;

namespace SupplierCustomer.WebApi
{
    public class Startup
    {
        private const string AllowAnyOriginPolicyName = "AllowAnyOrigin";
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors(options =>
            {
                options.AddPolicy(
                    AllowAnyOriginPolicyName,
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowCredentials().AllowAnyHeader();
                    });
            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(AllowAnyOriginPolicyName));
            });

            services.AddSupplierCustormer(configuration);
        }

        public void Configure(IApplicationBuilder applicationBuilder/*, 
            IDatabaseInitializer databaseInitializer,
            IDatabaseSeeder databaseSeeder*/)
        {
            applicationBuilder.UseDeveloperExceptionPage();
            applicationBuilder.UseStaticFiles();

            //databaseInitializer.Initialize();
            applicationBuilder.UseCors(AllowAnyOriginPolicyName);

            applicationBuilder.UseMvc(routes =>
            {
                routes.MapRoute("default", "/");
            });

            applicationBuilder.UseAuthentication();

            //databaseSeeder.Seed();
        }
    }
}
