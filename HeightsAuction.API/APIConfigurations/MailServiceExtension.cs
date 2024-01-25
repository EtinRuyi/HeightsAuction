using HeightsAuction.Domain.Entities.Helper;

namespace HeightsAuction.API.APIConfigurations
{
    public static class MailServiceExtension
    {
        public static void AddMailService(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
        }
    }
}
