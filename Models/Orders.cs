using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class Orders
    {
        [Key]
        public long Id { get; set; }
        public Guid TransactionId { get; set; }
        public string CustomersId { get; set; }
        public decimal Total { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public int ModeOfPayment { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderStatusId { get; set; }
        public int DeliveryStatusId { get; set; }
        public string CancellationReason { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }
        public List<OrderShippingInfo> OrderShippingInfo { get; set; }
        public User Customers { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
    }
}
