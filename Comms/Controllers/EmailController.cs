using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comms.Models;
using Comms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Comms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender emailSender;
        private readonly IGatekeeperApiClient gatekeeperClient;

        public EmailController(IEmailSender emailSender, IGatekeeperApiClient gatekeeperClient)
        {
            this.emailSender = emailSender;
            this.gatekeeperClient = gatekeeperClient;
        }

        [HttpPost("ToUser")]
        public async Task<IActionResult> ToUser([FromBody] MessageToUserId message)
        {
            try
            {
                var response = await gatekeeperClient.GetAsync($"api/Users/{message.UserId}");
                if(response.IsSuccessStatusCode)
                {
                    User user = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
                    await emailSender.SendEmailAsync(user.Email, message.Subject, message.Content);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }

            }
            catch (EmailException ex)
            {
                // TODO log it properly
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
            catch (GatekeeperApiException ex)
            {
                // TODO log it properly
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
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