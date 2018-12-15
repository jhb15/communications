using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comms.Models;
using Comms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;

        public EmailController(IEmailSender emailSender, IGatekeeperApiClient gatekeeperClient, ILogger<EmailController> logger)
        {
            this.emailSender = emailSender;
            this.gatekeeperClient = gatekeeperClient;
            this.logger = logger;
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
                logger.LogError(ex.Message);
                return StatusCode(500);
            }
            catch (GatekeeperApiException ex)
            {
                logger.LogError(ex.Message);
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
                logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}