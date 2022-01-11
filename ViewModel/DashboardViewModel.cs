using ECommerce1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.ViewModel
{
    public class DashboardViewModel
    {
        public decimal Sales { get; set; }
        public long ProductsSold { get; set; }
        public long Pending { get; set; }
        public IEnumerable<Orders> RecentOrders { get; set; }
        public IEnumerable<Orders> RecentDeliveries { get; set; }
        public IEnumerable<Message> RecentMessages { get; set; }
        public IEnumerable<OrderDetails> TopSelling { get; set; }
        public IEnumerable<Product> OutOfStock { get; set; }
        public IEnumerable<Product> Critical { get; set; }

    }
}
