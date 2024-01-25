using HeightsAuction.Application.Interfaces.Repositories;
using HeightsAuction.Application.Interfaces.Services;
using HeightsAuction.Application.ServicesImplementations;
using HeightsAuction.Domain.Entities;
using HeightsAuction.Domain.Entities.Helper;
using HeightsAuction.Persistence.AppContext;
using HeightsAuction.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
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
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailServices, EmailServices>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<HAuctionDBContext>()
            .AddDefaultTokenProviders();
            services.AddScoped<RoleManager<IdentityRole>>();
            
        }
    }
}
