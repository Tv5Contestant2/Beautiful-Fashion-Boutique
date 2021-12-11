using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Models
{
    public class AdyenPaymentResponse
    {
        public string resultCode { get; set; }
        public AdyenPaymentResponseAction action { get; set; }
    }
}