﻿using HeightsAuction.Application.Interfaces.Services;
using HeightsAuction.Domain.Entities.Helper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace HeightsAuction.Application.ServicesImplementations
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailServices> _logger;

        public EmailServices(ILogger<EmailServices> Logger, EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
            _logger = Logger;
        }
        public async Task SendMailAsync(MailRequest mailRequest)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));
                emailMessage.To.Add(new MailboxAddress(mailRequest.ToEmail, mailRequest.ToEmail));

                emailMessage.Subject = mailRequest.Subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $"{mailRequest.Body}<br/>"
                };

                emailMessage.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending email.");
                throw;
            }
        }
    }
}
