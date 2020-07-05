using LoadBalancerAPI.Data.Context;
using LoadBalancerAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using GeoCoordinatePortable;

namespace LoadBalancerAPI.Services
{
    public class ServerLoadManager
    {
        private readonly DatabaseContext _context;
        static volatile public int[] ServerLoad;
        static volatile public Server[] Servers;
        static volatile public int LoadTime;

        public ServerLoadManager(DatabaseContext context)
        {
            _context = context;
            var servers = _context.Servers.ToList();
            ServerLoad = new int[servers.Count];
            Servers = new Server[servers.Count];
            for (var i = 0; i < servers.Count; i++)
            {
                ServerLoad[i] = servers[i].RequestLimit;
                Servers[i] = servers[i];
            }
            LoadTime = 30;
        }

        public static void AddLoad(int index, int count)
        {
            var timer = new Timer();
            timer.Interval = LoadTime * 1000;
            timer.Elapsed += (sender, e) => RemoveLoadOnTimer(sender, e, index, count);
            timer.AutoReset = false;
            ServerLoad[index] -= count;
            timer.Enabled = true;
        }

        private static void RemoveLoadOnTimer(Object source, ElapsedEventArgs e, int index, int count)
        {
            ServerLoad[index] += count;
        }

        public static List<BusinessObjects.Response> CreateResponsesForRequest(Request request)
        {
           
            var responseCount = request.Amount;
            var reversedServerLoad = new double[ServerLoad.Length];
            var normalizedServerLoad = new double[ServerLoad.Length];
            var distance = new double[ServerLoad.Length];
            var normalizedDistance = new double[ServerLoad.Length];
            var clientPosition = new GeoCoordinate(request.Latitude, request.Longitude);
            var serverDistribution = new Dictionary<int, int>();
            var connectionQuality = new double[ServerLoad.Length];
            var responses = new List<BusinessObjects.Response>();

            // find distances between each server and client and normalize 
            for(int i = 0; i < ServerLoad.Length; i++)
            {
                var serverPosition = new GeoCoordinate(Servers[i].Latitude, Servers[i].Longitude);
                distance[i] = clientPosition.GetDistanceTo(serverPosition);
            }
            var oldMaxDistances = new double[distance.Length];
            for (int i=0; i<oldMaxDistances.Length; i++)
            {
                oldMaxDistances[i] = distance.Max();
            }
            normalizedDistance = Normalize(distance, 0, 100, oldMaxDistances);
            
            // reverse ServerLoad array so 0 is no load and max value is max load
            for (int i = 0; i < ServerLoad.Length; i++)
            {
                reversedServerLoad[i] = Servers[i].RequestLimit - ServerLoad[i];
            }
            // normalize server load
            var oldMaxLoads = new double[ServerLoad.Length];
            for(int i=0; i<ServerLoad.Length; i++)
            {
                oldMaxLoads[i] = Servers[i].RequestLimit;
            }
            normalizedServerLoad = Normalize(reversedServerLoad, 0, 100, oldMaxLoads);

            // calculate connection quality to each server
            for (int i = 0; i < ServerLoad.Length; i++)
            {
                if (normalizedServerLoad[i] < 100)
                    connectionQuality[i] = normalizedDistance[i] * 2 + normalizedServerLoad[i];
                else
                    connectionQuality[i] = double.MaxValue;

            }

            for (int i = 0; i < responseCount; i++)
            {
                
                int bestServer = 0;
                int val;
                for (int j = 0; j < connectionQuality.Length; j++)
                {
                    if (connectionQuality[j] < connectionQuality[bestServer])
                        bestServer = j;
                }
                if (connectionQuality[bestServer] == double.MaxValue)
                    bestServer = -1;
                else
                {
                    normalizedServerLoad[bestServer] += 100 / (double)(Servers[bestServer].RequestLimit);
                    if (normalizedServerLoad[bestServer] < 100)
                        connectionQuality[bestServer] = normalizedDistance[bestServer] * 2 + normalizedServerLoad[bestServer];
                    else
                        connectionQuality[bestServer] = double.MaxValue;
                }
                if (serverDistribution.TryGetValue(bestServer, out val))
                {
                    serverDistribution[bestServer] = val + 1;
                } else
                {
                    serverDistribution.Add(bestServer, 1);
                }
                
            }

            foreach (KeyValuePair<int, int> entry in serverDistribution)
            {
                if (entry.Key != -1)
                {
                    var response = new BusinessObjects.Response()
                    {
                        Server = new BusinessObjects.Server()
                        {
                            Id = Servers[entry.Key].Id,
                            Lat = Servers[entry.Key].Latitude,
                            Lng = Servers[entry.Key].Longitude
                        },
                        Request = new BusinessObjects.Request()
                        {
                            Lat = request.Latitude,
                            Lng = request.Longitude
                        },
                        Count = entry.Value,
                        Distance = distance[entry.Key]
                    };
                    responses.Add(response);
                    AddLoad(entry.Key, entry.Value);
                } else
                {
                    var response = new BusinessObjects.Response()
                    {
                        Server = null,
                        Request = new BusinessObjects.Request()
                        {
                            Lat = request.Latitude,
                            Lng = request.Longitude
                        },
                        Count = entry.Value,
                        Distance = 0
                    };
                    responses.Add(response);
                }
                
            }

            return responses;
        }

        private static double[] Normalize(double[] data, double newMin, double newMax, double[] oldMax)
        {
            double oldMin = 0;
            var normalizedData = new double[data.Length];
            for(int i = 0; i < data.Length; i++)
            {
                normalizedData[i] = ((data[i] - oldMin) / (oldMax[i] - oldMin)) * (newMax - newMin) + newMin;
            }

            return normalizedData;
        }
        
        
    }
}
