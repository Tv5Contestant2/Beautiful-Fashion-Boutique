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

        public Returns GetReturns(Guid transactionId)
        {
            var orders = _context.OrdersDetails
                .Include(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Include(x => x.Orders)
                .Include(x => x.ReturnStatus)
                .FirstOrDefault(x => x.TransactionId == transactionId
                         && x.IsReturned == true);

            var result = _context.Returns
                .Include(x => x.ReturnStatus)
                .FirstOrDefault(x => x.OrderReference == transactionId);

            if (orders != null)
            {
                if (result != null)
                    result.ReturnStatus = orders.ReturnStatus;
            }

            return result;
        }
        public async Task<IEnumerable<OrderDetails>> GetOrderDetailsById(string transactionId)
        {
            var result = await _context.OrdersDetails
                .Include(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Colors)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Sizes)
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
                .Include(x => x.ReturnStatus)
                .Include(x => x.ChangeProducts)
                .Include(x => x.ChangeProductImages)
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

        public async Task<OrderDetails> CancelReturnRequest(Guid transactionId, long productId)
        {
            var result = new OrderDetails();

            if (productId != 0)
            {
                result = _context.OrdersDetails
                  .FirstOrDefault(x => x.TransactionId == transactionId
                                    && x.ProductId == productId);
            }
            else
            {
                result = _context.OrdersDetails
                  .FirstOrDefault(x => x.TransactionId == transactionId);
            }
            result.ReturnStatusId = null;
            result.ReturnReason = null;
            result.QuantityReturned = 0;
            result.IsReturned = false;

            _context.OrdersDetails.Update(result);

            var returns = _context.Returns
                .Where(x => x.OrderReference == transactionId && x.ProductId == productId)
                .ToList();

            _context.Returns.RemoveRange(returns);

            await _context.SaveChangesAsync();

            return result;
        }

        public void CancelRequestByProduct(OrderViewModel viewModel)
        {
            var result = _context.Returns
                .FirstOrDefault(x => x.OrderReference == viewModel.Returns.OrderReference && x.ProductId == viewModel.Returns.ProductId
                && x.ChangeProductsId == viewModel.Returns.ChangeProductsId);

            _context.Returns.Remove(result);

            _context.SaveChanges();
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
                                  && x.ProductId == viewModel.Returns.ProductId);

            result.ReturnStatusId = (int)OrderStatusEnum.Approved;

            _context.OrdersDetails.Update(result);

            //var result = _context.Returns
            //    .FirstOrDefault(x => x.OrderReference.ToString() == transactionId
            //                      && x.ProductId == viewModel.Returns.ProductId
            //                      && x.ChangeProductsId == viewModel.Returns.ChangeProductsId);

            //result.ReturnStatusId = (int)OrderStatusEnum.Approved;

            _context.OrdersDetails.Update(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<OrderDetails> DeclineReturn(string transactionId, OrderViewModel viewModel)
        {
            var result = _context.OrdersDetails
                .FirstOrDefault(x => x.TransactionId.ToString() == transactionId
                                  && x.ProductId == viewModel.Returns.ProductId);

            result.ReturnStatusId = (int)OrderStatusEnum.Declined;

            _context.OrdersDetails.Update(result);

            //var result = _context.Returns
            //    .FirstOrDefault(x => x.OrderReference.ToString() == transactionId
            //                      && x.ProductId == viewModel.Returns.ProductId
            //                      && x.ChangeProductsId == viewModel.Returns.ChangeProductsId);

            //result.ReturnStatusId = (int)OrderStatusEnum.Approved;

            _context.OrdersDetails.Update(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public void AddToReturns(OrderViewModel viewModel)
        {
            var returns = new Returns()
            {
                OrderReference = viewModel.Returns.OrderReference,
                ProductId = viewModel.Returns.ProductId,
                ChangeProductsId = viewModel.Returns.ChangeProductsId,
                ReturnDate = DateTime.Now
            };

            _context.Returns.Add(returns);
            _context.SaveChanges();
        }

        public void RemoveFromReturns(OrderViewModel viewModel)
        {
            var result = _context.Returns.FirstOrDefault(x => x.Id == viewModel.Returns.Id);

            _context.Returns.Remove(result);
            _context.SaveChanges();
        }

        public void ClearReturns(OrderViewModel viewModel)
        {
            var result = _context.Returns.Where(x => x.OrderReference == viewModel.TransactionId && x.ProductId == viewModel.ProductId).ToList();

            _context.Returns.RemoveRange(result);
            _context.SaveChanges();
        }
    }
}
