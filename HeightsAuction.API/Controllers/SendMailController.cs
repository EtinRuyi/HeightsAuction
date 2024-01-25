using HeightsAuction.Application.Interfaces.Services;
using HeightsAuction.Domain.Entities.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeightsAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailController : ControllerBase
    {
        private readonly IEmailServices _emailServices;
        public SendMailController(IEmailServices emailServices)
        {
            _emailServices = emailServices;
        }
        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail()
        {
            try
            {
                MailRequest mailRequest = new MailRequest();
                mailRequest.ToEmail = "etinosa.idowu@gmail.com";
                mailRequest.Subject = "Welcome To Heights Auctions";
                mailRequest.Body = "Thanks for Bidding";
                await _emailServices.SendMailAsync(mailRequest);
                return Ok("Email sent successfully!");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
