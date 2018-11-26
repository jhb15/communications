using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comms.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Comms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("ToUser")]
        public IActionResult ToUser([FromBody] MessageToUserId message)
        {
            throw new NotImplementedException();
        }

        [HttpPost("ToEmail")]
        public IActionResult ToEmailAddress([FromBody] MessageToEmail message)
        {
            throw new NotImplementedException();
        }
    }
}