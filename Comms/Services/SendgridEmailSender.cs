using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Comms.Services
{
    public class SendgridEmailSender : IEmailSender
    {
        private EmailAddress fromEmail;
        private SendGridClient client;

        public SendgridEmailSender(IConfiguration configuration)
        {
            var appConfig = configuration.GetSection("Comms");
            var fromAddress = appConfig.GetValue<string>("FromAddress", "notifications@aberfitness.biz");
            var fromName = appConfig.GetValue<string>("FromName", "AberFitness");
            fromEmail = new EmailAddress(fromAddress, fromName);
            client = new SendGridClient(appConfig.GetValue<string>("SendgridApiKey"));
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var toEmail = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(
                fromEmail,
                toEmail,
                subject,
                null,
                htmlContent
                );
            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode != HttpStatusCode.OK || response.StatusCode != HttpStatusCode.Accepted)
            {
                // TODO log it properly
                Console.WriteLine(response.Body);
                throw new EmailException("Sendgrid API returned non-OK result.");
            }
        }
    }
}
