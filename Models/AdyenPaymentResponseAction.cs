using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Models
{
    public class AdyenPaymentResponseAction
    {
        public string type { get; set; }
        public string method { get; set; }
        public string paymentMethodType { get; set; }
        public string url { get; set; }
    }
}