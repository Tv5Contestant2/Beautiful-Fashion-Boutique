using ECommerce1.Data.Enums;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDBContext _context;

        public OrderService(AppDBContext context)
        {
            _context = context;
        }
        public bool AddToOrder(Orders order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            return true;
        }

        public async Task<IEnumerable<Orders>> GetAllOrders()
        {
            var result = await _context.Orders
                .Include(x => x.OrderStatus)
                .Include(x => x.DeliveryStatus)
                .Include(x => x.Customers)
                .ToListAsync();

            return result;
        }
        public async Task<IEnumerable<Orders>> GetAllReturns()
        {
            var result = await _context.Orders
                .Include(x => x.OrderDetails)
                .Include(x => x.Customers)
                .Where(x => x.OrderDetails.Any(x => x.IsReturned))
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<OrderDetails>> GetReturnRequestById(string transactionId)
        {
            var result = await _context.OrdersDetails
                .Include(x => x.ReturnStatus)
                .Include(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Where(x => x.TransactionId.ToString() == transactionId
                         && x.IsReturned == true)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Orders>> GetAllOrdersByUser(string userId)
        {
            var result = await _context.Orders
                .Include(x => x.Customers)
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            return result;
        }
        public Orders GetOrderById(string transactionId)
        {
            var result = _context.Orders
                .Include(x => x.OrderStatus)
                .FirstOrDefault(x => x.TransactionId.ToString() == transactionId);

            return result;
        }

        public async Task<IEnumerable<OrderDetails>> GetOrderDetailsById(string transactionId)
        {
            var result = await _context.OrdersDetails
                .Include(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Where(x => x.TransactionId.ToString() == transactionId)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetCustomerOrderCount(string userId)
        {
            var result = await _context.Orders
                .Where(x => x.CustomersId == userId)
                .ToListAsync();

            return result.Count();
        }

        public async Task<int> GetCustomerReturnsCount(string userId)
        {
            var result = await _context.OrdersDetails
                .Where(x => x.Orders.CustomersId == userId 
                         && x.IsReturned == true)
                .ToListAsync();

            return result.Count();
        }

        public async Task<IEnumerable<Returns>> GetReturnsByReference(Guid transactionId, long productId)
        {
            var result = await _context.Returns
                .Include(x => x.ChangeProducts)
                    .ThenInclude(x => x.ProductImages)
                .Where(x => x.OrderReference == transactionId && x.ProductId == productId)
                .ToListAsync();

            return result;
        }

        public async Task<Orders> UpdateOrderStatuses(string transactionId)
        {
            var result = _context.Orders.FirstOrDefault(x => x.TransactionId.ToString() == transactionId);

            if (result.DeliveryStatusId == (int)DeliveryStatusEnum.Pending)
            {
                result.DeliveryStatusId = (int)DeliveryStatusEnum.Shipped;
                result.OrderStatusId = (int)OrderStatusEnum.Shipped;
            }
            else if (result.DeliveryStatusId == (int)DeliveryStatusEnum.Shipped)
            {
                result.DeliveryStatusId = (int)DeliveryStatusEnum.Received;
                result.OrderStatusId = (int)OrderStatusEnum.Completed;
            }

            _context.Orders.Update(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<Orders> CancelOrder(string transactionId, OrderViewModel viewModel)
        {
            var result = _context.Orders.FirstOrDefault(x => x.TransactionId.ToString() == transactionId);

            result.OrderStatusId = viewModel.OrderStatusId;
            result.CancellationReason = viewModel.CancellationReason;

            _context.Orders.Update(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<OrderDetails> ReturnOrder(OrderViewModel viewModel)
        {
            var result = _context.OrdersDetails
                .FirstOrDefault(x => x.TransactionId == viewModel.TransactionId 
                                  && x.ProductId == viewModel.ProductId);

            result.IsReturned = true;
            result.ReturnReason = viewModel.ReturnReason;
            result.ReturnStatusId = (int)OrderStatusEnum.PendingRequest;
            result.QuantityReturned += viewModel.QuantityReturned;

            _context.OrdersDetails.Update(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<OrderDetails> ApproveReturn(string transactionId, OrderViewModel viewModel)
        {
            var result = _context.OrdersDetails
                .FirstOrDefault(x => x.TransactionId.ToString() == transactionId
                                  && x.ProductId == viewModel.ProductId);

            result.ReturnStatusId = (int)OrderStatusEnum.Approved;

            _context.OrdersDetails.Update(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public void AddToReturns(OrderViewModel viewModel)
        {
            var result = _context.Returns.Where(
                x => x.TransactionId == viewModel.TransactionId 
                && x.ProductId == viewModel.ProductId)
                .ToList();

            if (!result.Any(x => x.ChangeProductsId == viewModel.Returns.ChangeProductsId))
            {
                var returns = new Returns()
                {
                    TransactionId = Guid.NewGuid(),
                    OrderReference = viewModel.TransactionId,
                    ProductId = viewModel.ProductId,
                    Quantity = 1,
                    ChangeProductsId = viewModel.Returns.ChangeProductsId,
                    ReturnDate = DateTime.Now
                };

                _context.Returns.Add(returns);
            }
            else {

                var returns = new Returns();
                returns.Quantity += 1;

                _context.Returns.Update(returns);
            }

            _context.SaveChanges();
        }

        public void RemoveFromReturns(OrderViewModel viewModel)
        {
            var result = _context.Returns.FirstOrDefault(
                   x => x.OrderReference == viewModel.TransactionId
                   && x.ChangeProductsId == viewModel.Returns.ChangeProductsId);

            _context.Returns.Remove(result);
_context.SaveChangesAsync();
        }
    }
}
