using Comms.Controllers;
using Comms.Models;
using Comms.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommsTest.Controllers
{
    public class EmailController_Test
    {
        protected Mock<IEmailSender> emailSender;
        protected Mock<IGatekeeperApiClient> gatekeeperApiClient;
        protected Mock<ILogger<EmailController>> logger;
        protected EmailController controller;

        public EmailController_Test()
        {
            emailSender = new Mock<IEmailSender>();
            gatekeeperApiClient = new Mock<IGatekeeperApiClient>();
            logger = new Mock<ILogger<EmailController>>();
            controller = new EmailController(emailSender.Object, gatekeeperApiClient.Object, logger.Object);
        }
    }

    public class EmailController_ToEmailAddressTest : EmailController_Test
    {
        private MessageToEmail msg;

        public EmailController_ToEmailAddressTest() : base()
        {
            msg = new MessageToEmail
            {
                EmailAddress = "test@example.com",
                Subject = "Test Subject",
                Content = "Test Content"
            };
        }

        [Fact]
        public async void ToEmailAddress_CallsSendEmailAsync()
        {
            emailSender.Setup(e => e.SendEmailAsync(msg.EmailAddress, msg.Subject, msg.Content)).Returns(Task.CompletedTask).Verifiable();
            await controller.ToEmailAddress(msg);
            emailSender.Verify();
        }

        [Fact]
        public async void ToEmailAddress_ReturnsOk()
        {
            emailSender.Setup(e => e.SendEmailAsync(msg.EmailAddress, msg.Subject, msg.Content)).Returns(Task.CompletedTask);
            var result = await controller.ToEmailAddress(msg);
            Assert.IsType<OkResult>(result);
            var content = result as OkObjectResult;
            Assert.Null(content);
        }

        [Fact]
        public async void ToEmailAddress_Returns500OnException()
        {
            var ex = new EmailException("test exception");
            emailSender.Setup(e => e.SendEmailAsync(msg.EmailAddress, msg.Subject, msg.Content)).ThrowsAsync(ex);
            var result = await controller.ToEmailAddress(msg);
            Assert.IsType<StatusCodeResult>(result);
            var content = result as StatusCodeResult;
            Assert.Equal(500, content.StatusCode);
        }
    }

    public class EmailController_ToUserTest : EmailController_Test
    {
        private MessageToUserId msg;

        public EmailController_ToUserTest() : base()
        {
            msg = new MessageToUserId
            {
                UserId = "abc123",
                Subject = "Test Subject",
                Content = "Test Content"
            };
        }

        [Fact]
        public async void ToUser_MakesCorrectApiCall()
        {
            var apiResponse = new HttpResponseMessage {
                StatusCode = HttpStatusCode.NotFound
            };
            gatekeeperApiClient.Setup(g => g.GetAsync("api/Users/abc123")).ReturnsAsync(apiResponse).Verifiable();
            await controller.ToUser(msg);
            gatekeeperApiClient.Verify();
        }

        [Fact]
        public async void ToUser_CallsSendEmailAsync()
        {
            var apiResponse = new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"email\":\"test@example.com\"}")
            };
            gatekeeperApiClient.Setup(g => g.GetAsync("api/Users/abc123")).ReturnsAsync(apiResponse);
            emailSender.Setup(e => e.SendEmailAsync("test@example.com", msg.Subject, msg.Content)).Returns(Task.CompletedTask).Verifiable();
            await controller.ToUser(msg);
            emailSender.Verify();
        }

        [Fact]
        public async void ToUser_ReturnsOk()
        {
            var apiResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"email\":\"test@example.com\"}")
            };
            gatekeeperApiClient.Setup(g => g.GetAsync("api/Users/abc123")).ReturnsAsync(apiResponse);
            emailSender.Setup(e => e.SendEmailAsync("test@example.com", msg.Subject, msg.Content)).Returns(Task.CompletedTask);
            var result = await controller.ToUser(msg);
            Assert.IsType<OkResult>(result);
            var content = result as OkObjectResult;
            Assert.Null(content);
        }

        [Fact]
        public async void ToUser_ReturnsNotFoundOnInvalidUserId()
        {
            var apiResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };
            gatekeeperApiClient.Setup(g => g.GetAsync("api/Users/abc123")).ReturnsAsync(apiResponse);
            var result = await controller.ToUser(msg);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void ToUser_Returns500OnEmailException()
        {
            var apiResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"email\":\"test@example.com\"}")
            };
            gatekeeperApiClient.Setup(g => g.GetAsync("api/Users/abc123")).ReturnsAsync(apiResponse);
            emailSender.Setup(e => e.SendEmailAsync("test@example.com", msg.Subject, msg.Content)).ThrowsAsync(new EmailException("test"));
            var result = await controller.ToUser(msg);
            Assert.IsType<StatusCodeResult>(result);
            var content = result as StatusCodeResult;
            Assert.Equal(500, content.StatusCode);
        }

        [Fact]
        public async void ToUser_Returns500OnApiException()
        {

        }
    }
}
