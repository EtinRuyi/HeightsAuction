using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeightsAuction.Persistence.ServiceExtension
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            //services.AddDbContext<SaviDbContext>(options =>
                //options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register Identity
            //services.AddIdentity<AppUser, IdentityRole>()
                //.AddEntityFrameworkStores<SaviDbContext>()
                //.AddDefaultTokenProviders();

            // Register RoleManager
           // services.AddScoped<RoleManager<IdentityRole>>();
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IAuthenticationService, AuthenticationService>();



            // Register GenericRepository
            //services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
