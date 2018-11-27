using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comms.Models;
using Comms.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Comms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender emailSender;

        public EmailController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        [HttpPost("ToUser")]
        public IActionResult ToUser([FromBody] MessageToUserId message)
        {
            throw new NotImplementedException();
        }

        [HttpPost("ToEmail")]
        public async Task<IActionResult> ToEmailAddress([FromBody] MessageToEmail message)
        {
            try
            {
                await emailSender.SendEmailAsync(message.EmailAddress, message.Subject, message.Content);
                return Ok();
            }
            catch (EmailException ex)
            {
                // TODO log it properly
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }
    }
}