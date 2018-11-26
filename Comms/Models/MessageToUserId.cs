using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Comms.Models
{
    public class MessageToUserId : BaseMessage
    {
        [Required]
        public virtual string UserId { get; set; }
    }
}
