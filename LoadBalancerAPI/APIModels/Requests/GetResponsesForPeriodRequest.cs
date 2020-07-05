using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoadBalancerAPI.APIModels.Requests
{
    public class GetResponsesForPeriodRequest
    {
        [Required]
        public int Time { get; set; }
    }
}
