using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class ProfileController : Controller
    {
        private readonly IProfileService _service;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IMessageService _messageService;
        private readonly IProductsService _productService;
        private readonly IProductCategoriesService _productCategoriesService;
        private readonly ICustomersService _customersService;
        private readonly ICommonServices _commonServices;
        private readonly UserManager<User> _userManager;

        public ProfileController(IProfileService service
            , ICartService cartService
            , IOrderService orderService
            , IMessageService messageService
            , IProductsService productService
            , IProductCategoriesService productCategoriesService
            , ICustomersService customersService
            , ICommonServices commonServices
            , UserManager<User> userManager)
        {
            _service = service;
            _cartService = cartService;
            _orderService = orderService;
            _messageService = messageService;
            _productService = productService;
            _productCategoriesService = productCategoriesService;
            _customersService = customersService;
            _commonServices = commonServices;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var _customer = await _customersService.GetCustomerById(userId);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.MessagesCount = await _messageService.GetCustomerMessagesCount(userId);

            if (string.IsNullOrEmpty(_customer.Image)) _customer.Image = _commonServices.NoImage;

            ViewBag.Customer = _customer;

            var _customerViewModel = new CustomerViewModel
            {
                AddressBaranggay = _customer.AddressBaranggay,
                AddressBlock = _customer.AddressBlock,
                AddressCity = _customer.AddressCity,
                AddressLot = _customer.AddressLot,
                Birthday = _customer.Birthday,
                ContactNumber = _customer.ContactNumber,
                //DateCreated = (DateTime)_customer.DateCreated,
                Email = _customer.Email,
                FirstName = _customer.FirstName,
                GenderId = _customer.GenderId,
                Genders = await _customersService.GetGenders(),
                Id = _customer.Id,
                Image = string.IsNullOrEmpty(_customer.Image) ? _commonServices.NoImage : _customer.Image,
                LastName = _customer.LastName,
            };

            return View(_customerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind] CustomerViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Image)) model.Image = _commonServices.GetImageByte64StringFromSplit(model.Image);
            if (string.IsNullOrEmpty(model.Password)) //Do not update password if empty
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            ModelState.Remove("Email");

            if (!ModelState.IsValid)
            {
                _customersService.InitializeCustomer(model);
                return View(model);
            }

            await _customersService.UpdateCustomer(model);
            return RedirectToAction("Home", "StoreFront");
        }

        public async Task<IActionResult> Orders()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.MessagesCount = await _messageService.GetCustomerMessagesCount(userId);

            var result = await _service.GetCustomerOrders(userId);
            var user = await _userManager.FindByIdAsync(userId);

            if (string.IsNullOrEmpty(user.Image)) user.Image = _commonServices.NoImage;

            ViewBag.Customer = user;

            return View(result);
        }

        public async Task<IActionResult> Returns()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.MessagesCount = await _messageService.GetCustomerMessagesCount(userId);

            var result = await _service.GetCustomerReturns(userId);
            var user = await _userManager.FindByIdAsync(userId);

            if (string.IsNullOrEmpty(user.Image)) user.Image = _commonServices.NoImage;

            ViewBag.Customer = user;

            return View(result);
        }

        public async Task<IActionResult> Wishlist()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.MessagesCount = await _messageService.GetCustomerMessagesCount(userId);

            var result = await _service.GetCustomerWishlist(userId);
            var user = await _userManager.FindByIdAsync(userId);
            if (string.IsNullOrEmpty(user.Image)) user.Image = _commonServices.NoImage;

            ViewBag.Customer = user;

            return View(result);
        }

        public async Task<IActionResult> Messages()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            var result = await _messageService.GetCustomerMessages(userId);
            var user = await _userManager.FindByIdAsync(userId);

            if (string.IsNullOrEmpty(user.Image)) user.Image = _commonServices.NoImage;

            ViewBag.Customer = user;

            var viewModel = new MessageViewModel
            {
                Messages = result,
                SenderId = userId
            };

            return View(viewModel);
        }

        [Route("Profile/ViewOrder/{transactionId:Guid}")]
        public async Task<IActionResult> ViewOrder(Guid transactionId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            var user = await _userManager.FindByIdAsync(userId);
            var order = _orderService.GetOrderById(transactionId.ToString());
            var orderDetails = await _orderService.GetOrderDetailsById(transactionId.ToString());
            var customerShippingAddress = new CustomersShippingAddress
            {
                Block = user.AddressBlock,
                Lot = user.AddressLot,
                Barangay = user.AddressBaranggay,
                City = user.AddressCity
            };

            var viewModel = new OrderViewModel()
            {
                CustomersId = userId,
                Customer = user.FirstName,
                Address = user.AddressCity,
                TransactionId = transactionId,
                DeliveryStatusId = order.DeliveryStatusId,
                OrderStatusId = order.OrderStatusId,
                OrderDetails = orderDetails,
                OrderStatus = order.OrderStatus,
                Colors = await _productCategoriesService.GetColors(),
                Sizes = await _productCategoriesService.GetSizes(),
                CustomersShippingAddress = customerShippingAddress
            };

            if (string.IsNullOrEmpty(user.Image)) user.Image = _commonServices.NoImage;

            ViewBag.Customer = user;
            return View(viewModel);
        }

        public async Task<IActionResult> ViewReturns(Guid transactionId, long productId) //
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            var user = await _userManager.FindByIdAsync(userId);
            var returns = _orderService.GetReturns(transactionId);
            var returnDetails = await _orderService.GetReturnsByReference(transactionId, productId); //viewModel.ProductId

            var viewModel = new OrderViewModel();
            viewModel.CustomersId = userId;
            viewModel.Customer = user.FirstName;
            viewModel.Address = user.AddressCity;
            viewModel.TransactionId = transactionId;
            if (returns != null)
                viewModel.ReturnStatus = returns.ReturnStatus;
            viewModel.ReturnDetails = returnDetails;

            if (string.IsNullOrEmpty(user.Image)) user.Image = _commonServices.NoImage;

            ViewBag.Customer = user;
            return View(viewModel);
        }

        public async Task<IActionResult> Addresses()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.MessagesCount = await _messageService.GetCustomerMessagesCount(userId);

            var result = await _service.GetCustomerBillingAddresses(userId);
            var user = await _userManager.FindByIdAsync(userId);

            if (string.IsNullOrEmpty(user.Image)) user.Image = _commonServices.NoImage;

            ViewBag.Customer = user;

            return View(result);
        }

        [Route("Profile/ReturnOrder/{productId:long}")]
        public async Task<IActionResult> ReturnOrder(long productId, OrderViewModel viewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);
            ViewBag.MessagesCount = await _messageService.GetCustomerMessagesCount(userId);

            var user = await _userManager.FindByIdAsync(userId);
            viewModel.Customer = user.FirstName;
            viewModel.Address = user.AddressCity;
            viewModel.OrderDetails = await _orderService.GetOrderDetailsById(viewModel.TransactionId.ToString());
            viewModel.ProductId = productId;

            if (string.IsNullOrEmpty(user.Image)) user.Image = _commonServices.NoImage;

            ViewBag.Customer = user;

            var products = await _productService.GetProductsWithSamePrice(productId);
            ViewBag.Products = products;
            ViewBag.Returns = await _orderService.GetReturnsByReference(viewModel.TransactionId, productId);
            return View(viewModel);
        }

        public IActionResult ReturnSuccess()
        {
            return View();
        }

        public async Task<IActionResult> CancelReturnRequest(Guid transactionId, long productId)
        {
            await _orderService.CancelReturnRequest(transactionId, productId);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> CancelRequestByProduct(OrderViewModel viewModel)
        {
            await Task.Delay(0);
            _orderService.CancelRequestByProduct(viewModel);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult CreateMessage(MessageViewModel message)
        {
            _messageService.CreateMessage(message);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Route("Profile/ViewMessage/{messageId:Guid}")]
        public async Task<IActionResult> ViewMessage(Guid messageId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.CartCount = await _cartService.GetCartTotalQty(userId);
            ViewBag.OrderCount = await _orderService.GetCustomerOrderCount(userId);
            ViewBag.ReturnsCount = await _orderService.GetCustomerReturnsCount(userId);
            ViewBag.WishlistCount = await _cartService.GetWishlistCount(userId);

            var result = await _messageService.GetMessageConversation(messageId);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.Customer = user;

            var viewModel = new MessageViewModel
            {
                Messages = result,
                MessageId = messageId.ToString(),
                SenderId = userId
            };

            return View(viewModel);
        }
    }
}