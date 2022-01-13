using ECommerce1.Data;
using ECommerce1.Data.Enums;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce1.Controllers
{
    [AllowAnonymous]
    public class ProductsController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductsService _service;
        private readonly AppDBContext _context;

        public ProductsController(IProductsService service, ICartService cartService, AppDBContext context)
        {
            _service = service;
            _cartService = cartService;
            _context = context;
        }

        public async Task<IActionResult> Index(int page)
        {
            var data = await _service.GetAllProducts();

            foreach (var item in data)
            {
                if (item.ProductVariants != null && item.ProductVariants.Any())
                {
                    item.StockQty = item.ProductVariants.Sum(x => x.Quantity);
                }
            }

            ViewBag.InStock = data.Where(x => x.StockStatusId == (int)StockStatusEnum.InStock).ToList().Count();
            ViewBag.OutOfStock = data.Where(x => x.StockStatusId == (int)StockStatusEnum.OutOfStock).ToList().Count();
            ViewBag.Critical = data.Where(x => x.StockStatusId == (int)StockStatusEnum.Critical).ToList().Count();

            var viewModel = new ProductViewModel
            {
                ItemPerPage = 10,
                Products = data,
                CurrentPage = page
            };

            return View(viewModel);
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

            if (string.IsNullOrEmpty(product.ProductVariantJSON)) ModelState.AddModelError(string.Empty, "Product variant(s) is required.");

            if (!ModelState.IsValid)
            {
                _service.InitializeProductListForResponse(product);
                return View(product);
            }

            if (!string.IsNullOrEmpty(product.ProductVariantJSON))
            {
                product.ProductVariants = JsonSerializer.Deserialize<List<ProductVariant>>(product.ProductVariantJSON);
            }

            if (!string.IsNullOrEmpty(product.ProductImageJSON))
            {
                List<ProductImage> productImages = new List<ProductImage>();
                productImages = JsonSerializer.Deserialize<List<ProductImage>>(product.ProductImageJSON);
                if (productImages.Any())
                {
                    foreach (var item in productImages)
                    {
                        var _split = item.ImageBase64String.Split(",");
                        if (_split.Any())
                        {
                            item.Image = _split[1]; //Get Base64String only.
                        }
                    }
                }

                product.ProductImages = productImages;
            }

            product.StockStatusId = 1;
            product.StatusId = 1;
            product.DateCreated = DateTime.Now;

            _service.CreateProduct(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var productDetails = await _service.GetProductById(id);
            if (productDetails == null) return RedirectToAction("Error", "Home");

            await _service.DeleteProduct(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteProduct(long id)
        {
            var productDetails = await _service.GetProductById(id);
            if (productDetails == null) return RedirectToAction("Error", "Home");

            return View(productDetails);
        }

        public async Task<IActionResult> UpdateProduct(long id)
        {
            var _product = await _service.GetProductById(id);
            if (_product == null) return RedirectToAction("Error", "Home");

            _service.InitializeProductOnUpdate(_product);

            return View(_product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(long id, Product product)
        {
            if (string.IsNullOrEmpty(product.ProductVariantJSON)) ModelState.AddModelError(string.Empty, "Product variant(s) is required.");

            if (!ModelState.IsValid)
            {
                _service.InitializeProductListForResponse(product);
                return View(product);
            }

            if (!string.IsNullOrEmpty(product.ProductVariantJSON))
            {
                product.ProductVariants = JsonSerializer.Deserialize<List<ProductVariant>>(product.ProductVariantJSON);
            }

            if (!string.IsNullOrEmpty(product.ProductImageJSON))
            {
                List<ProductImage> productImages = new List<ProductImage>();
                productImages = JsonSerializer.Deserialize<List<ProductImage>>(product.ProductImageJSON);
                if (productImages.Any())
                {
                    foreach (var item in productImages)
                    {
                        var _split = item.ImageBase64String.Split(",");
                        if (_split.Any())
                        {
                            if (_split.Length > 1)
                            {
                                item.Image = _split[1]; //Get Base64String only.
                            }
                            else
                            {
                                item.Image = _split[0]; //Get Base64String only.
                            }
                        }
                    }
                }

                product.ProductImages = productImages;
            }

            await _service.UpdateProduct(id, product);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ViewProduct(long id)
        {
            var _product = await _service.GetProductById(id);
            if (_product == null) return RedirectToAction("Error", "Home");

            _service.InitializeProductOnUpdate(_product);

            return View(_product);
        }

        public IActionResult CreateReview(ProductViewModel viewModel)
        {
            var productReview = new ProductReview
            {
                CustomersId = viewModel.CustomersId,
                ProductId = viewModel.ProductId,
                Review = viewModel.Review,
                Rating = viewModel.Rating,
                ReviewDate = DateTime.Now
            };

            _service.CreateReview(productReview);

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}