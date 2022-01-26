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
    public class AdministratorController : Controller
    {
        private readonly IAdministratorService _service;
        private readonly ICommonServices _commonService;
        private readonly IMessageService _messageService;
        private readonly UserManager<User> _userManager;

        public AdministratorController(IAdministratorService service
            , ICommonServices commonService
            , IMessageService messageService
            , UserManager<User> userManager)
        {
            _service = service;
            _commonService = commonService;
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

        public IActionResult Settings()
        {
            var viewModel = new SettingsViewModel()
            {
                StoreLogo = _service.GetStoreLogo(),
                StoreBanner = _service.GetHero(),
                EmailLogo = _service.GetEmailLogo(),
            };

            return View(viewModel);
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
            var viewModel = new DashboardViewModel()
            {
                Sales = await _service.GetProductSales(),
                ProductsSold = await _service.GetProductSold(),
                Pending = await _service.GetPendingOrders(),

                RecentOrders = await _service.GetRecentOrders(),
                RecentDeliveries = await _service.GetRecentDeliveries(),
                RecentMessages = await _service.GetRecentMessages(),
                TopSelling = await _service.GetTopSelling(),
                OutOfStock = await _service.GetOutOfStock(),
                Critical = await _service.GetCritical(),
            };

            return View(viewModel);
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
        

        public IActionResult UpdateSettings(SettingsViewModel viewModel)
        {
            _service.UpdateSettings(viewModel);

            return RedirectToAction(nameof(Index));
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
    }
}