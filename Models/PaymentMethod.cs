using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class PaymentMethod
    {
        [Key]
        public int MethodID { get; set; }
        public string PayMethod { get; set; }
    }
}
