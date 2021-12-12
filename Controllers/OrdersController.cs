using ECommerce1.Data.Enums;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class OrdersController : Controller
    {
        private readonly IOrderService _service;
        private readonly ICartService _cartService;
        private readonly UserManager<User> _userManager;

        public OrdersController(ICartService cartService
            , IOrderService service
            , UserManager<User> userManager)
        {
            _service = service;
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page)
        {
            var result = await _service.GetAllOrders();

            ViewBag.Received = result.Where(x => x.DeliveryStatusId == (int)DeliveryStatusEnum.Received).Count();
            ViewBag.Shipped = result.Where(x => x.DeliveryStatusId == (int)DeliveryStatusEnum.Shipped).ToList().Count();
            ViewBag.Pending = result.Where(x => x.DeliveryStatusId == (int)DeliveryStatusEnum.Pending).ToList().Count(); ;

            var viewModel = new OrderViewModel
            {
                ItemPerPage = 10,
                Orders = result,
                CurrentPage = page
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Returns(int page)
        {
            var result = await _service.GetAllReturns();

            ViewBag.Approved = result.Where(x => x.OrderDetails.Any(x => x.ReturnStatusId == (int)OrderStatusEnum.Approved)).Count();
            ViewBag.Pending = result.Where(x => x.OrderDetails.Any(x => x.ReturnStatusId == (int)OrderStatusEnum.PendingRequest)).Count();

            var viewModel = new OrderViewModel
            {
                ItemPerPage = 10,
                Orders = result,
                CurrentPage = page
            };

            return View(viewModel);
        }

        [Route("Orders/ViewOrder/{transactionId:Guid}")]
        public async Task<IActionResult> ViewOrder(string transactionId)
        {
            var result = await _service.GetOrderDetailsById(transactionId);
            var order = _service.GetOrderById(transactionId);
            ViewBag.TransactionId = transactionId;
            ViewBag.DeliveryStatusId = order.DeliveryStatusId;
            ViewBag.OrderStatusId = order.OrderStatusId;

            return View(result);
        }

        [Route("Orders/ViewReturn/{transactionId:Guid}")]
        public async Task<IActionResult> ViewReturn(Guid transactionId)
        {
            var result = await _service.GetReturnRequestById(transactionId.ToString());
            var viewModel = new OrderViewModel();

            viewModel.TransactionId = transactionId;
            viewModel.OrderDetails = result;

            return View(viewModel);
        }

        [Route("Orders/UpdateOrder/{transactionId:Guid}")]
        public async Task<IActionResult> UpdateOrder(string transactionId)
        {
            await _service.UpdateOrderStatuses(transactionId);

            return RedirectToAction(nameof(Index));
        }

        [Route("Orders/CancelOrder/{transactionId:Guid}")]
        public async Task<IActionResult> CancelOrder(string transactionId, OrderViewModel viewModel)
        {
            await _service.CancelOrder(transactionId, viewModel);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Route("Orders/ApproveReturn/{transactionId:Guid}")]
        public async Task<IActionResult> ApproveReturn(string transactionId, OrderViewModel viewModel)
        {
            await _service.ApproveReturn(transactionId, viewModel);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> ReturnOrder(OrderViewModel viewModel)
        {
            await _service.ReturnOrder(viewModel);

            return RedirectToAction("ReturnSuccess", "Profile");
        }
    }
}
