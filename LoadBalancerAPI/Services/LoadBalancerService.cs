using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoadBalancerAPI.APIModels.Requests;
using LoadBalancerAPI.BusinessObjects;
using LoadBalancerAPI.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LoadBalancerAPI.Services
{
    public class LoadBalancerService : ILoadBalancerService
    {
        private readonly DatabaseContext _context;

        public LoadBalancerService(DatabaseContext context)
        {
            _context = context;
        }

        public List<Response> GetResponsesForPeriod(GetResponsesForPeriodRequest request)
        {
            var responses = _context.Responses
                .Where(r => DateTime.Now.Subtract(r.Date).TotalMinutes <= request.Time)
                .Include(r => r.Request)
                .Include(r => r.Server)
                .ToList();

            var returnedResponses = new List<Response>();

            foreach (var response in responses)
            {
                if (response.ServerId.HasValue)
                {
                    returnedResponses.Add(new Response()
                    {
                        Server = new Server()
                        {
                            Id = response.ServerId.GetValueOrDefault(),
                            Lat = response.Server.Latitude,
                            Lng = response.Server.Longitude
                        },
                        Request = new Request()
                        {
                            Lat = response.Request.Latitude,
                            Lng = response.Request.Longitude
                        },
                        Count = response.Count,
                        Distance = 0
                    });
                }
                else
                {
                    returnedResponses.Add(new Response()
                    {
                        Server = null,
                        Request = new Request()
                        {
                            Lat = response.Request.Latitude,
                            Lng = response.Request.Longitude
                        },
                        Count = response.Count,
                        Distance = 0
                    });
                }
                
            }

            return returnedResponses;
        }

        public List<Response> GetResponsesForRequest(GetResponsesForRequestRequest request)
        {
            var newRequest = new Data.Models.Request
            {
                Amount = request.Count,
                Latitude = request.Lat,
                Longitude = request.Lng
            };
            _context.Requests.Add(newRequest);
            _context.SaveChanges();
            var responses = ServerLoadManager.CreateResponsesForRequest(newRequest);
            foreach (var response in responses)
            {
                if (response.Server != null)
                {
                    var dbResponse = new Data.Models.Response()
                    {
                        RequestId = newRequest.Id,
                        ServerId = response.Server.Id,
                        Date = DateTime.Now,
                        Count = response.Count
                    };
                    _context.Responses.Add(dbResponse);
                }
                else
                {
                    var dbResponse = new Data.Models.Response()
                    {
                        RequestId = newRequest.Id,
                        ServerId = null,
                        Date = DateTime.Now,
                        Count = response.Count
                    };
                    _context.Responses.Add(dbResponse);
                }
                
            }
            _context.SaveChanges();

            return responses;
            
        }

        public void SetLoadTime(SetLoadTimeRequest request)
        {
            var time = request.Seconds;
            if (time > 300)
                time = 300;
            if (time < 5)
                time = 5;
            ServerLoadManager.LoadTime = time;
        }
    }
}
