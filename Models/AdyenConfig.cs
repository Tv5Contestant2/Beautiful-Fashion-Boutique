using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Models
{
    public class AdyenConfig
    {
        public string ApiKey { get; set; }
        public string Merchant { get; set; }
        public string ClientKey { get; set; }
        public string Currency { get; set; }
        public string ReturnURL { get; set; }
    }
}