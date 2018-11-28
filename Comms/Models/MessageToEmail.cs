using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Comms.Models
{
    public class MessageToEmail : BaseMessage
    {
        [Required]
        [EmailAddress]
        public virtual string EmailAddress { get; set; }
    }
}
