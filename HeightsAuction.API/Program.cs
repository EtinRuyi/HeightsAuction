using NLog.Web;
using NLog;
using HeightsAuction.API.Mapper;
using HeightsAuction.Common.Utilities;
using HeightsAuction.Persistence.ServiceExtension;
using HeightsAuction.API.APIConfigurations;
using System.Text.Json.Serialization;
using Hangfire;
using HeightsAuction.Application.Interfaces.Services;
using HeightsAuction.Application.ServicesImplementations;



var builder = WebApplication.CreateBuilder(args);
ConfigurationHelper.InstantiateConfiguration(builder.Configuration);

var configuration = builder.Configuration;
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    // Add services to the container.
    builder.Services.AddDependencies(configuration);
    builder.Services.AddMailService(configuration);
    builder.Services.AddControllers();
    //builder.Services.AddControllers().AddJsonOptions(options =>
    //{
    //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    //});
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAuthentication();
    builder.Services.ConfigureAuthentication(configuration);
    builder.Services.AddAutoMapper(typeof(MapperProfile));
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Heights Auction v1"));
    }
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        await Seeder.SeedRolesAndSuperAdmin(serviceProvider);
    }
    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseHangfireDashboard("/dashboard");

    RecurringJob.AddOrUpdate<IBiddingRoomService>("CheckExpiredRooms", service => service.CheckAndUpdateExpiredRoomsAsync(), Cron.MinuteInterval(10));
    RecurringJob.AddOrUpdate<IBiddingService>("UpdateWinningBids", service => service.UpdateWinningBidsAsync(), Cron.MinuteInterval(10));

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Something is not right here");
}
finally
{
    NLog.LogManager.Shutdown();
}