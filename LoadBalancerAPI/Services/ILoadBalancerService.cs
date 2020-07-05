using LoadBalancerAPI.APIModels.Requests;
using LoadBalancerAPI.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadBalancerAPI.Services
{
    public interface ILoadBalancerService
    {
        List<Response> GetResponsesForRequest(GetResponsesForRequestRequest request);
        List<Response> GetResponsesForPeriod(GetResponsesForPeriodRequest request);
        void SetLoadTime(SetLoadTimeRequest request);
    }
}
