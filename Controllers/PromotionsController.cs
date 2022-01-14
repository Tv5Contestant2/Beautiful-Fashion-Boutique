using ECommerce1.Data;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ECommerce1.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class PromotionsController : Controller
    {
        private readonly IPromotionsService _service;
        private readonly ICommonServices _commonServices;
        private readonly UserManager<User> _userManager;
        private readonly AppDBContext _appDBContext;

        public PromotionsController(IPromotionsService service
            , AppDBContext appDBContext
            , ICommonServices commonServices
            , UserManager<User> userManager)
        {
            _service = service;
            _appDBContext = appDBContext;
            _commonServices = commonServices;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var data = await _service.GetAllPromos();

            var viewModel = new PromoViewModel
            {
                ItemPerPage = 10,
                Promos = data,
                CurrentPage = page
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePromo()
        {
            var _model = await _service.InitializePromo();
            return View(_model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromo([Bind] Promos model)
        {
            if (!string.IsNullOrEmpty(model.Image)) model.Image = _commonServices.GetImageByte64StringFromSplit(model.Image);

            if (model.StartDate == null) ModelState.AddModelError("StartDate", "The field Start Date is required.");
            if (model.StartDate == null) ModelState.AddModelError("EndDate", "The field End Date is required.");
            if (model.EndDate < model.StartDate) ModelState.AddModelError("EndDate", "The field End Date should not be less than the Start Date.");
            if (model.StartDate > model.EndDate) ModelState.AddModelError("StartDate", "The field Start Date should not be greater than the End Date.");

            if (!ModelState.IsValid)
            {
                model = await _service.InitializePromo(model);
                return View(model);
            }

            model.DateCreated = DateTime.Now;
            if (model.StatusId <= 0) model.StatusId = (int)StatusEnum.Active;

            if (model.IsByCategory) model.PromoCategory = await _service.GetProductCategoryTitle((int)model.ProductCategoryId);
            if (model.IsByGender) model.PromoCategory = await _service.GetGenderTitle((int)model.GenderId);

            var _result = await _service.CreatePromo(model);
            if (_result.Item1)
            {
                var _promoResult = await _service.UpdateProductsSale(model);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(string.Empty, _result.Item2);
                return View("CreatePromo", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePromo(long id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (string.IsNullOrEmpty(user.Image)) ViewBag.Image = _commonServices.NoImage;

            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            ViewBag.Role = role;

            var _result = await _service.GetPromoById(id);
            var _model = await _service.InitializePromo(_result);
            if (_model == null) return RedirectToAction("Error", "Home");
            return View(_model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePromo(long id, [Bind] Promos model)
        {
            if (!string.IsNullOrEmpty(model.Image)) model.Image = _commonServices.GetImageByte64StringFromSplit(model.Image);

            if (model.StartDate == null) ModelState.AddModelError("StartDate", "The field Start Date is required.");
            if (model.StartDate == null) ModelState.AddModelError("EndDate", "The field End Date is required.");
            if (model.EndDate < model.StartDate) ModelState.AddModelError("EndDate", "The field End Date should not be less than the Start Date.");
            if (model.StartDate > model.EndDate) ModelState.AddModelError("StartDate", "The field Start Date should not be greater than the End Date.");

            if (!ModelState.IsValid)
            {
                model = await _service.InitializePromo(model);
                return View(model);
            }

            if (model.StatusId <= 0) model.StatusId = (int)StatusEnum.Active;

            if (model.IsByCategory) model.PromoCategory = await _service.GetProductCategoryTitle((int)model.ProductCategoryId);
            if (model.IsByGender) model.PromoCategory = await _service.GetGenderTitle((int)model.GenderId);

            await _service.UpdatePromo(id, model);
            var _promoResult = await _service.UpdateProductsSale(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeletePromo(long id)
        {
            var PromoDetails = await _service.GetPromoById(id);
            if (PromoDetails == null) return RedirectToAction("Error", "Home");
            return View(PromoDetails);
        }

        [HttpPost, ActionName("DeletePromo")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var PromoDetails = await _service.GetPromoById(id);
            if (PromoDetails == null) return RedirectToAction("Error", "Home");

            await _service.DeletePromo(id);
            return RedirectToAction(nameof(Index));
        }
    }
}