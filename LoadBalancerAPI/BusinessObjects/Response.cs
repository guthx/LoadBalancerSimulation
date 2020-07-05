using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadBalancerAPI.BusinessObjects
{
    public class Response
    {
        public Server Server { get; set; }
        public Request Request { get; set; }
        public int Count { get; set; }
        public double Distance { get; set; }
    }
}
