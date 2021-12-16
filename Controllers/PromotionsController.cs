using ECommerce1.Data;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class PromotionsController : Controller
    {
        private readonly IPromotionsService _service;
        private readonly AppDBContext _appDBContext;

        public PromotionsController(IPromotionsService service, AppDBContext appDBContext)
        {
            _service = service;
            _appDBContext = appDBContext;
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

        public IActionResult CreatePromo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromo([Bind("Name,Description,SalePercentage,ImageFile,StartDate,EndDate")] Promos promos)
        {
            await Task.Delay(0);

            if (!ModelState.IsValid) 
                return View(promos);

            if (promos.ImageFile != null) {
                using (var ms = new MemoryStream())
                {
                    promos.ImageFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    promos.Image = Convert.ToBase64String(fileBytes);
                }
            }

            promos.DateCreated = DateTime.Now;

            _service.CreatePromo(promos);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdatePromo(long id)
        {
            var PromoDetails = await _service.GetPromoById(id);
            if (PromoDetails == null) return RedirectToAction("Error", "Home");
            return View(PromoDetails);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePromo(long id, [Bind("Id,Name,Description, SalePercentage")] Promos promos)
        {
            if (!ModelState.IsValid)
            {
                return View(promos);
            }
            await _service.UpdatePromo(id, promos);
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
