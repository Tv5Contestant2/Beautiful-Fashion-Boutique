using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class OrderShippingInfo
    {
        [Key]
        public long Id { get; set; }
        public Guid TransactionId { get; set; }
        public int CustomerBillingAddressId { get; set; }
        public int CustomerShippingAddressId { get; set; }
        public int DeliveryMethodId { get; set; }
        public bool IsCashOnDelivery { get; set; }
        public string Notes { get; set; }
    }
}
