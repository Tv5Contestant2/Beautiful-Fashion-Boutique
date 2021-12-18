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
    public class AdministratorController : Controller
    {
        private readonly IAdministratorService _service;
        private readonly IMessageService _messageService;
        private readonly UserManager<User> _userManager;

        public AdministratorController(IAdministratorService service
            , IMessageService messageService
            , UserManager<User> userManager)
        {
            _service = service;
            _messageService = messageService;
            _userManager = userManager;
        }

        public IActionResult AboutUs()
        {
            var result = _service.GetAboutUs();
            return View(result);
        }

        public IActionResult ContactUs()
        {
            var result = _service.GetContactUs();
            return View(result);
        }

        public IActionResult CreateAboutUs(About about)
        {
            _service.CreateAboutUs(about);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateContactUs(SocMed socMed)
        {
            _service.CreateContactUs(socMed);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Sales = _service.GetProductSales();
            ViewBag.ProductsSold = _service.GetProductSold();
            ViewBag.Pending = _service.GetPendingOrders();

            ViewBag.RecentOrders = _service.GetRecentOrders();
            ViewBag.RecentDeliveries = _service.GetRecentDeliveries();
            ViewBag.RecentMessages = _service.GetRecentMessages();
            ViewBag.OutOfStock = await _service.GetOutOfStock();

            return View();
        }

        public async Task<IActionResult> Messages()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var result = await _messageService.GetAllCustomerMessages();

            var viewModel = new MessageViewModel
            {
                Messages = result,
                SenderId = userId,
            };

            return View(viewModel);
        }

        public IActionResult CreateMessage(MessageViewModel message)
        {
            _messageService.CreateMessage(message);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [Route("Administrator/ViewMessage/{messageId:Guid}")]
        public async Task<IActionResult> ViewMessage(Guid messageId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var result = await _messageService.GetMessageConversation(messageId);

            var viewModel = new MessageViewModel
            {
                Messages = result,
                MessageId = messageId.ToString(),
                SenderId = userId
            };

            return View(viewModel);
        }

        public IActionResult UnderConstruction()
        {
            return View();
        }
    }
}