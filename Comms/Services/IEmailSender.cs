using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comms.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string htmlContent);
    }

    public class EmailException : Exception
    {
        public EmailException(string message) : base(message)
        {
        }
    }
}
