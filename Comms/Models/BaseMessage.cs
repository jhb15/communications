using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Comms.Models
{
    public class BaseMessage
    {
        [Required]
        public virtual string Subject { get; set; }

        [Required]
        public virtual string Content { get; set; }
    }
}
