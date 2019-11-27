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
        public long? ServerId { get; set; }
        [Required]
        public long RequestId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Server Server { get; set; }
        public virtual Request Request { get; set; }
    }
}
