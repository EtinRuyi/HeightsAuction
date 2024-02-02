using HeightsAuction.Domain.Entities.Helper;

namespace HeightsAuction.Application.Interfaces.Services
{
    public interface IEmailServices
    {
        Task SendMailAsync(MailRequest mailRequest);
    }
}
