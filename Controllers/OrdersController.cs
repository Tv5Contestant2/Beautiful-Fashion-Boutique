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
        private readonly ICartService _cartService;
        private readonly ICommonServices _commonService;
        private readonly IOrderService _service;
        private readonly UserManager<User> _userManager;

        public OrdersController(ICartService cartService
            , ICommonServices commonService
            , IOrderService service
            , UserManager<User> userManager)
        {
            _service = service;
            _cartService = cartService;
            _commonService = commonService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var result = await _service.GetAllOrders();

            ViewBag.Received = result.Where(x => x.DeliveryStatusId == (int)DeliveryStatusEnum.Received).Count();
            ViewBag.Shipped = result.Where(x => x.DeliveryStatusId == (int)DeliveryStatusEnum.Shipped).ToList().Count();
            ViewBag.Pending = result.Where(x => x.DeliveryStatusId == (int)DeliveryStatusEnum.Pending && x.OrderStatusId != (int)OrderStatusEnum.Cancelled).ToList().Count();

            var viewModel = new OrderViewModel
            {
                ItemPerPage = 10,
                Orders = result,
                CurrentPage = page
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Returns(int page = 1)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

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
        public async Task<IActionResult> ViewOrder(Guid transactionId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var orderDetails = await _service.GetOrderDetailsById(transactionId);
            var orderShippingInfo = _service.GetOrderShippingInfo(transactionId);
            var order = _service.GetOrderById(transactionId);
            ViewBag.TransactionId = transactionId;
            ViewBag.DeliveryStatusId = order.DeliveryStatusId;
            ViewBag.OrderStatusId = order.OrderStatusId;

            var viewModel = new OrderViewModel()
            {
                TransactionId = transactionId,
                DeliveryStatusId = order.DeliveryStatusId,
                OrderStatusId = order.OrderStatusId,
                OrderDetails = orderDetails,
                CustomersId = order.CustomersId,
                OrderShippingInfo = orderShippingInfo,
                PaymentMethod = _commonService.GetPaymentMethod(order.PaymentMethodId),
                DeliveryMethod = _commonService.GetDeliveryMethod(order.DeliveryMethodId),
            };

            return View(viewModel);
        }

        [Route("Orders/ViewReturn/{transactionId:Guid}")]
        public async Task<IActionResult> ViewReturn(Guid transactionId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var result = await _service.GetReturnRequestById(transactionId.ToString());
            var viewModel = new OrderViewModel();

            viewModel.TransactionId = transactionId;
            viewModel.OrderDetails = result;

            return View(viewModel);
        }

        public async Task<IActionResult> ViewReturnDetails(Guid transactionId, long productId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var result = await _service.GetReturnsByReference(transactionId, productId);
            var viewModel = new OrderViewModel();

            viewModel.TransactionId = transactionId;
            viewModel.ReturnDetails = result;

            return View(viewModel);
        }

        [Route("Orders/UpdateOrder/{transactionId:Guid}")]
        public async Task<IActionResult> UpdateOrder(Guid transactionId, OrderViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            await _service.UpdateOrder(viewModel);
            //await _service.UpdateOrderStatuses(transactionId);

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
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            await _service.ApproveReturn(transactionId, viewModel);

            return RedirectToAction("ViewReturn", viewModel);
        }

        [Route("Orders/DeclineReturn/{transactionId:Guid}")]
        public async Task<IActionResult> DeclineReturn(string transactionId, OrderViewModel viewModel)
        {
            await _service.DeclineReturn(transactionId, viewModel);

            return RedirectToAction("ViewReturn", viewModel);
        }

        public async Task<IActionResult> ReturnOrder(OrderViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonService.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            await _service.ReturnOrder(viewModel);

            return RedirectToAction("ReturnSuccess", "Profile");
        }

        public async Task<IActionResult> AddToReturns(OrderViewModel viewModel)
        {
            await Task.Delay(0);
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            _service.AddToReturns(viewModel);

            return RedirectToAction("ReturnOrder", "Profile", viewModel);
        }

        public async Task<IActionResult> RemoveFromReturns(OrderViewModel viewModel)
        {
            await Task.Delay(0);
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            _service.RemoveFromReturns(viewModel);

            return RedirectToAction("ReturnOrder", "Profile", viewModel);
        }

        public async Task<IActionResult> ClearReturns(OrderViewModel viewModel)
        {
            await Task.Delay(0);
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            _service.ClearReturns(viewModel);

            return RedirectToAction("ReturnOrder", "Profile", viewModel);
        }
    }
}