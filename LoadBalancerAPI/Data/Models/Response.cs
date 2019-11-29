using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace LoadBalancerAPI.Data.Models
{
    public class Response
    { 
        public long Id { get; set; }
        [Column("ServerId")]
        public long? ServerId { get; set; }
        [Column("RequestId")]
        [Required]
        public long RequestId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Count { get; set; }
        [ForeignKey("ServerId")]
        public virtual Server Server { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }
    }
}
