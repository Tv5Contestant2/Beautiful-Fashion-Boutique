using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IOrderService
    {
        public bool AddToOrder(OrderDetails orders);

        public Task<IEnumerable<OrderDetails>> GetAllOrders();

        public Task<IEnumerable<OrderDetails>> GetAllOrdersByUser(); //temporary parameterless

    }
}
