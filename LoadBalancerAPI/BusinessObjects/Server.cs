using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadBalancerAPI.BusinessObjects
{
    public class Server
    {
        public long Id { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
