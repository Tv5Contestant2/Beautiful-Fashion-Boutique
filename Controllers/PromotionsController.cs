using ECommerce1.Data;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class PromotionsController : Controller
    {
        private readonly IPromotionsService _service;
        private readonly AppDBContext _appDBContext;

        public PromotionsController(IPromotionsService service, AppDBContext appDBContext)
        {
            _service = service;
            _appDBContext = appDBContext;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllPromos();
            return View(data);
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
    }
}
