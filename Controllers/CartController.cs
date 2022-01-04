using Adyen.Model.Checkout;
using ECommerce1.Data.Enums;
using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {
        private readonly ICartService _service;
        private readonly ICommonServices _commonService;
        private readonly IEmailService _emailService;
        private readonly IOrderService _orderService;
        private readonly IProductsService _productsService;
        private readonly IProductCategoriesService _productCategoriesService;
        private readonly IAdyenService _adyenService;
        private readonly UserManager<User> _userManager;

        public CartController(
            UserManager<User> userManager
            , ICartService service
            , ICommonServices commonService
            , IEmailService emailService
            , IOrderService orderService
            , IProductsService productsService
            , IProductCategoriesService productCategoriesService
            , IAdyenService adyenService)
        {
            _service = service;
            _commonService = commonService;
            _emailService = emailService;
            _orderService = orderService;
            _productsService = productsService;
            _productCategoriesService = productCategoriesService;
            _userManager = userManager;
            _adyenService = adyenService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var viewModel = await _service.InitializeCart(userId);
            ViewBag.CartCount = viewModel.CartCount;
            ViewBag.WishlistCount = viewModel.WishlistCount;

            return View(viewModel);
        }

        public async Task<IActionResult> AddToCart(ProductViewModel viewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var isExist = await _service.IsProductInCart(userId, viewModel.CartDetails);

            if (!isExist)
            {
                //Create Cart
                var cartId = await _service.CreateCart(userId);

                //Add to Cart Details
                if (cartId != 0)
                    await _service.AddToCartItems(cartId, viewModel.CartDetails);
            }
            else
                await _service.UpdateCartItems(viewModel.CartDetails);
            
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> IncreaseQuantity(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var cartDetails = await _service.GetCartItems(userId);
            if (cartDetails == null) return RedirectToAction("Error", "Home");

            await _service.IncreaseQuantity(id, 1);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> DecreaseQuantity(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var cartDetails = await _service.GetCartItems(userId);
            if (cartDetails == null) return RedirectToAction("Error", "Home");

            await _service.DecreaseQuantity(id);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> AddToWishlistFromCart(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var cartDetails = await _service.GetCartItems(userId);
            if (cartDetails == null) return RedirectToAction("Error", "Home");

            await _service.AddToWishlist(userId, id);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> RemoveFromCart(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var cartDetails = await _service.GetCartItems(userId);
            if (cartDetails == null) return RedirectToAction("Error", "Home");

            await _service.RemoveFromCart(id);

            return View();
        }

        //[HttpPost, ActionName("RemoveFromCart")]
        //public async Task<IActionResult> RemoveFromCartConfirmed(long id)
        //{
        //    var userId = _userManager.GetUserId(HttpContext.User);
        //    var cartDetails = await _service.GetCartItemsByProductId(id, 0, 0, userId);
        //    if (cartDetails == null) return RedirectToAction("Error", "Home");

        //    ViewBag.CartCount = 0; //await _service.GetCartTotalQty(userId);
        //    ViewBag.WishlistCount = 0; //await _service.GetWishlistCount(userId);

        //    await _service.RemoveFromCart(id, userId);

        //    return RedirectToAction(nameof(Index));
        //}

        //public async Task<IActionResult> Checkout(Cart cart)
        //{
        //    if (cart.CustomersId != null)
        //    {
        //        var userId = _userManager.GetUserId(HttpContext.User);
        //        var user = await _userManager.FindByIdAsync(userId);  //(await _userManager.Users.Where(x => x.Id == userId).ToListAsync()).FirstOrDefault();

        //        var viewModel = new CartViewModel();
        //        var cartDetails = await _service.GetCartItems(userId);

        //        cart.ShippingBarangay = user.AddressBaranggay;
        //        cart.ShippingBlock = user.AddressBlock;
        //        cart.ShippingCity = user.AddressCity;
        //        cart.ShippingContactNumber = user.ContactNumber;
        //        cart.ShippingEmail = user.Email;
        //        cart.ShippingFirstName = user.FirstName;
        //        cart.ShippingLastName = user.LastName;
        //        cart.ShippingLot = user.AddressLot;

        //        viewModel.Cart = cart;
        //        viewModel.CartDetails = cartDetails;
        //        ViewBag.CartCount = 0; //await _service.GetCartTotalQty(userId);
        //        ViewBag.WishlistCount = 0; //await _service.GetWishlistCount(userId);
        //        ViewBag.CustomersId = userId;
        //        viewModel.Colors = await _productCategoriesService.GetColors();
        //        viewModel.Sizes = await _productCategoriesService.GetSizes();

        //        // Update Cart
        //        await _service.UpdateCart(userId, cart);

        //        return View(viewModel);
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpPost]
        //public async Task<IActionResult> OrderConfirmed(Cart cart)
        //{
        //    var userId = _userManager.GetUserId(HttpContext.User);
        //    var user = await _userManager.FindByIdAsync(userId);

        //    var viewModel = new CartViewModel();
        //    var cartDetails = await _service.GetCartItems(userId);
        //    if (cart.CustomersId != null)
        //    {
        //        viewModel.Cart = cart;
        //        viewModel.CartDetails = cartDetails;
        //        ViewBag.CartCount = 0; //await _service.GetCartTotalQty(userId);
        //        ViewBag.WishlistCount = 0; //await _service.GetWishlistCount(userId);
        //        ViewBag.CustomersId = userId;
        //        ViewBag.Customer = user.FirstName;

        //        await _service.UpdateCart(userId, cart);
        //    }

        //    Guid transactionId = Guid.NewGuid();
        //    List<OrderDetails> orderDetails = new List<OrderDetails>();

        //    foreach (var item in cartDetails)
        //    {
        //        var _orderDetails = new OrderDetails
        //        {
        //            TransactionId = transactionId,
        //            ProductId = item.ProductId,
        //            Quantity = item.Quantity,
        //            ColorId = item.ColorId,
        //            SizeId = item.SizeId,
        //            SubTotal = (item.Quantity * item.Product.Price),
        //        };

        //        orderDetails.Add(_orderDetails);
        //    }

        //    var modeOfPayment = cart.IsGCash ? (int)PaymentMethodEnum.GCash :
        //                            cart.IsPayMaya ? (int)PaymentMethodEnum.PayMaya :
        //                                cart.IsCashOnDelivery ? (int)PaymentMethodEnum.COD :
        //                                    0;

        //    var order = new Orders()
        //    {
        //        TransactionId = transactionId,
        //        CustomersId = userId,
        //        ModeOfPayment = modeOfPayment,
        //        OrderDate = DateTime.Now,
        //        OrderStatusId = modeOfPayment == 0 ? (int)OrderStatusEnum.PendingPayment : (int)OrderStatusEnum.Created,
        //        DeliveryStatusId = (int)DeliveryStatusEnum.Pending,
        //        Total = cart.Total,
        //        ShippingFee = cart.ShippingFee,
        //        TaxAmount = cart.TaxAmount,
        //        OrderDetails = orderDetails
        //    };

        //    var result = _orderService.AddToOrder(order);

        //    if (result)
        //    {
        //        await _service.EmptyCart(userId);

        //        //Update inventory
        //        await _productsService.UpdateStocks(orderDetails);
        //    }

        //    ViewBag.CartCount = 0;

        //    // Send E-Receipt
        //    if (modeOfPayment != 0)
        //        await _emailService.SendReceipt(user.Email, orderDetails, order);

        //    if (cart.IsGCash)
        //    {
        //        var _resultCheckout = _adyenService.CheckoutUsingGCash(order.TransactionId.ToString()
        //            , (long)(order.Total + order.ShippingFee + order.TaxAmount) * 100// Need to multiply into 100 since the adyen automatically convert the last two zeroes into decimal
        //            , user);
        //        Response.Redirect(_resultCheckout.action.url);
        //    }

        //    return View(viewModel);
        //}

        //[HttpGet]
        //public async Task<IActionResult> AdyenPaymentResponse([FromQuery(Name = "redirectResult")] string redirectResult)
        //{
        //    await Task.Delay(0);

        //    var _result = _adyenService.HandlePaymentResponse(redirectResult);

        //    ViewBag.IsResult = _result.Item1;
        //    ViewBag.ResultMessage = _result.Item2;

        //    return View();
        //}
    }
}