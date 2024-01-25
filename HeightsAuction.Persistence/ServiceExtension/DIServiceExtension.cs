using HeightsAuction.Application.Interfaces.Services;
using HeightsAuction.Application.ServicesImplementations;
using HeightsAuction.Domain.Entities.Helper;
using HeightsAuction.Persistence.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeightsAuction.Persistence.ServiceExtension
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HAuctionDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            var emailSettings = new EmailSettings();
            configuration.GetSection("EmailSettings").Bind(emailSettings);
            services.AddSingleton(emailSettings);
            services.AddTransient<IEmailServices, EmailServices>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();

            // Register Identity
            //services.AddIdentity<AppUser, IdentityRole>()
            //.AddEntityFrameworkStores<SaviDbContext>()
            //.AddDefaultTokenProviders();

            // Register RoleManager
            // services.AddScoped<RoleManager<IdentityRole>>();
            //services.AddScoped<IUserService, UserService>();
            



            // Register GenericRepository
            //services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
