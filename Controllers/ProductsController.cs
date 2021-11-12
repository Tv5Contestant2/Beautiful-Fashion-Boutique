using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _service;
       
        public ProductsController(IProductsService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllProducts();

            return View(data);
        }

        public IActionResult CreateProduct()
        {
            var viewModel = _service.InitializeProduct();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await Task.Delay(0);

            if (!ModelState.IsValid) 
                return View(product);

            if (product.ImageFile != null) {
                using (var ms = new MemoryStream())
                {
                    product.ImageFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    product.Image = Convert.ToBase64String(fileBytes);
                }
            }

            product.StockStatusId = 1;
            product.StatusId = 1;
            product.DateCreated = DateTime.Now;

            _service.CreateProduct(product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateProduct(long id)
        {
            var productDetails = await _service.GetProductById(id);
            if (productDetails == null) return View("NotFound");
            return View(productDetails);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(long id, [Bind("Id,Name,Description")] Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            await _service.UpdateProduct(id, product);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeleteProduct(long id)
        {
            var productDetails = await _service.GetProductById(id);
            if (productDetails == null) return View("NotFound");
            return View(productDetails);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var productDetails = await _service.GetProductById(id);
            if (productDetails == null) return View("NotFound");

            await _service.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
