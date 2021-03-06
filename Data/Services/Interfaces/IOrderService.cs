using ECommerce1.Models;
using ECommerce1.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IOrderService
    {
        public bool AddToOrder(Orders orders, OrderShippingInfo orderShippingInfo);

        public Task<IEnumerable<Orders>> GetAllOrders();

        public Task<IEnumerable<Orders>> GetAllReturns();

        public Task<IEnumerable<OrderDetails>> GetReturnRequestById(string transactionId);

        public Task<int> GetCustomerOrderCount(string userId);

        public Task<int> GetCustomerReturnsCount(string userId);

        public Task<IEnumerable<Orders>> GetAllOrdersByUser(string userId);

        public Orders GetOrderById(Guid transactionId);

        public Task<IEnumerable<OrderDetails>> GetOrderDetailsById(Guid transactionId);

        public OrderShippingInfo GetOrderShippingInfo(Guid transactionId);

        public Task<IEnumerable<Returns>> GetReturnsByReference(Guid transactionId, long productId);

        public Returns GetReturns(Guid transactionId);

        public Task<Orders> CancelOrder(string transactionId, OrderViewModel viewModel);

        public Task<OrderDetails> CancelReturnRequest(Guid transactionId, long productId);

        public void CancelRequestByProduct(OrderViewModel viewModel);

        public Task<OrderDetails> ReturnOrder(OrderViewModel viewModel);

        public Task<OrderDetails> ApproveReturn(string transactionId, OrderViewModel viewModel);

        public Task<OrderDetails> DeclineReturn(string transactionId, OrderViewModel viewModel);

        public Task<Orders> UpdateOrder(OrderViewModel viewModel);

        public Task<Orders> UpdateOrderStatuses(Guid transactionId);

        public void AddToReturns(OrderViewModel viewModel);

        public void RemoveFromReturns(OrderViewModel viewModel);

        public void ClearReturns(OrderViewModel viewModel);

    }
}
