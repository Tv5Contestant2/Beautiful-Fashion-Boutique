using ECommerce1.Models;
using System;
using System.Collections.Generic;

namespace ECommerce1.ViewModel
{
    public class OrderViewModel
    {
        public Guid TransactionId { get; set; }
        public string CustomersId { get; set; }
        public string Customer { get; set; }
        public string Address { get; set; }
        public long ProductId { get; set; }
        public int DeliveryStatusId { get; set; }
        public int OrderStatusId { get; set; }
        public int Quantity { get; set; }
        public string CancellationReason { get; set; }
        public int QuantityReturned { get; set; }
        public string ReturnReason { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
