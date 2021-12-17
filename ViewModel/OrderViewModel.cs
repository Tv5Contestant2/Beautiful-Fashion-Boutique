using ECommerce1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public Returns Returns { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public OrderStatus ReturnStatus { get; set; }
        public IEnumerable<Orders> Orders { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Returns> ReturnDetails { get; set; }

        #region Pagination 
        public int ItemPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(Orders.Count() / (double)ItemPerPage));
        }
        public IEnumerable<Orders> PaginatedList()
        {
            int start = (CurrentPage - 1) * ItemPerPage;
            return Orders.OrderBy(b => b.Id).Skip(start).Take(ItemPerPage);
        }
        #endregion
    }
}
