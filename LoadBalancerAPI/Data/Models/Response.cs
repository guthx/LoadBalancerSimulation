using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace LoadBalancerAPI.Data.Models
{
    public class Response
    { 
        public long Id { get; set; }
        [Required]
        public long ServerId { get; set; }

        public virtual Server Server { get; set; }
    }
}
