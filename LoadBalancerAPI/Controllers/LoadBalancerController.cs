using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoadBalancerAPI.APIModels.Requests;
using LoadBalancerAPI.APIModels.Responses;
using LoadBalancerAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadBalancerController : ControllerBase
    {
        private readonly ILoadBalancerService _service;

        public LoadBalancerController(ILoadBalancerService service)
        {
            _service = service;
        }

        // GET: api/LoadBalancer
        [HttpPost("/requests")]
        public ActionResult<GetResponsesResponse> GetResponseForRequest([FromBody] GetResponsesForRequestRequest request)
        {
            try
            {
                if (!TryValidateModel(request))
                {
                    return StatusCode(400);
                }
                var responses = _service.GetResponsesForRequest(request);
                var response = new GetResponsesResponse
                {
                    Responses = responses
                };
                
                return Ok(response);

            } 
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        // GET: api/LoadBalancer/5
        [HttpGet("/responses")]
        public ActionResult<GetResponsesResponse> GetResponsesForPeriod([FromBody] GetResponsesForPeriodRequest request)
        {
            try
            {
                if (!TryValidateModel(request))
                {
                    return StatusCode(400);
                }
                var responses = _service.GetResponsesForPeriod(request);
                var response = new GetResponsesResponse
                {
                    Responses = responses
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("/load-time")]
        public ActionResult<StatusCodeResult> SetLoadTime([FromBody] SetLoadTimeRequest request)
        {
            try
            {
                if (!TryValidateModel(request))
                {
                    return StatusCode(400);
                }
                _service.SetLoadTime(request);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        } 
    }
}
