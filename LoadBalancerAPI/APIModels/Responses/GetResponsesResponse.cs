using LoadBalancerAPI.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadBalancerAPI.APIModels.Responses
{
    public class GetResponsesResponse
    {
        public List<Response> Responses { get; set; }
    }
}
