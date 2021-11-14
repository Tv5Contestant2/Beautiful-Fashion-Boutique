using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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

            if (!string.IsNullOrEmpty(product.ProductVariantJSON))
            {
                product.ProductVariants = JsonSerializer.Deserialize<List<ProductVariant>>(product.ProductVariantJSON);
            }

            if (!string.IsNullOrEmpty(product.ProductImageJSON))
            {
                List<ProductImage> productImages = new List<ProductImage>();
                productImages = JsonSerializer.Deserialize<List<ProductImage>>(product.ProductImageJSON);
                if (productImages.Any()) {
                    foreach (var item in productImages) {
                        var _split = item.ImageBase64String.Split(",");
                        if (_split.Any()) {
                            item.Image = _split[1]; //Get Base64String only.
                        }
                    }

                }

                product.ProductImages = productImages;
            }

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

        [HttpPost]
        public ActionResult fileupload(FormCollection form)
        {
            var _object = form;
            return View();
        }

        public async Task<IActionResult> UpdateProduct(long id)
        {
            var productDetails = await _service.GetProductById(id);
            if (productDetails == null) return View("NotFound");

            ViewBag.ProductRepos = _service.InitializeProduct();

            return View(productDetails);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(long id, Product product)
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
