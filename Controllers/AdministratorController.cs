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

        public async Task<IActionResult> AboutUs()
        {
            var result = _service.GetAboutUs();
            return View(result);
        }

        public async Task<IActionResult> ContactUs()
        {
            var result = _service.GetContactUs();
            return View(result);
        }

        public async Task<IActionResult> Settings()
        {
            var viewModel = new SettingsViewModel()
            {
                StoreLogo = _service.GetStoreLogo(),
                HeroVideo = _service.GetHeroVideo(),
                EmailLogo = _service.GetEmailLogo(),
            };

            return View(viewModel);
        }

        public async Task<IActionResult> CreateAboutUs(About about)
        {
            _service.CreateAboutUs(about);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CreateContactUs(SocMed socMed)
        {
            _service.CreateContactUs(socMed);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel()
            {
                Sales = _service.GetProductSales(),
                ProductsSold = _service.GetProductSold(),
                Pending = _service.GetPendingOrders(),

                RecentOrders = _service.GetRecentOrders(),
                RecentDeliveries = _service.GetRecentDeliveries(),
                RecentMessages = _service.GetRecentMessages(),
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

        public async Task<IActionResult> CreateMessage(MessageViewModel message)
        {
            _messageService.CreateMessage(message);

            return Redirect(Request.Headers["Referer"].ToString());
        }
        

        public async Task<IActionResult> UpdateSettings(SettingsViewModel viewModel)
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