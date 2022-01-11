using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Orders
    {
        [Key]
        public long Id { get; set; }

        public Guid TransactionId { get; set; }
        public string CustomersId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; }

        public int PaymentMethodId { get; set; }
        public int DeliveryMethodId { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderStatusId { get; set; }
        public int DeliveryStatusId { get; set; }
        public string CancellationReason { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime ReceivedDate { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }
        public List<OrderShippingInfo> OrderShippingInfo { get; set; }
        public User Customers { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
    }
}