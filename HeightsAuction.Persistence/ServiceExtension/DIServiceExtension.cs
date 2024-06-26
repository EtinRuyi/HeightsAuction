﻿using Hangfire;
using Hangfire.SqlServer;
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
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

            var hangfireConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddHangfire(options => options
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(hangfireConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            var emailSettings = new EmailSettings();
            configuration.GetSection("EmailSettings").Bind(emailSettings);
            services.AddSingleton(emailSettings);
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailServices, EmailServices>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IBiddingRoomService, BiddingRoomService>();
            services.AddScoped<IBiddingService, BiddingService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<HAuctionDBContext>()
            .AddDefaultTokenProviders();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddHangfireServer();

        }
    }
}
