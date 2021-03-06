using ECommerce1.Data.Enums;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class ProductsService : IProductsService
    {
        private readonly AppDBContext _context;
        private readonly ICommonServices _commonServices;

        public ProductsService(AppDBContext context, ICommonServices commonServices)
        {
            _context = context;
            _commonServices = commonServices;
        }

        public Product InitializeProduct()
        {
            var _result = new Product()
            {
                Colors = _context.Colors.OrderBy(x => x.Id).ToList(),
                Genders = _context.Genders.ToList(),
                ProductCategories = _context.ProductCategories.ToList(),
                Sizes = _context.Sizes.OrderBy(x => x.Id).ToList(),
                Statuses = _context.Statuses.OrderBy(x => x.Id).ToList(),
                StockStatuses = _context.StockStatuses.OrderBy(x => x.Id).ToList(),
                ProductCategoryId = 1
            };

            return _result;
        }

        public void InitializeProductListForResponse(Product product)
        {
            var _product = _context.Products.Where(x => x.Id == product.Id).FirstOrDefault();

            if (product.ProductCategoryId <= 0) product.ProductCategoryId = _product.ProductCategoryId;
            if (product.GenderId <= 0) product.GenderId = _product.GenderId;

            if (product.Colors == null) product.Colors = _context.Colors.OrderBy(x => x.Id).ToList();
            if (product.Genders == null) product.Genders = _context.Genders.ToList();
            if (product.ProductCategories == null) product.ProductCategories = _context.ProductCategories.ToList();
            if (product.Sizes == null) product.Sizes = _context.Sizes.OrderBy(x => x.Id).ToList();
            if (product.Statuses == null) product.Statuses = _context.Statuses.OrderBy(x => x.Id).ToList();
            if (product.StockStatuses == null) product.StockStatuses = _context.StockStatuses.OrderBy(x => x.Id).ToList();
        }

        public void InitializeProductListForZeroes(Product product)
        {
            var _product = _context.Products.Where(x => x.Id == product.Id).FirstOrDefault();

            if (product.ProductCategoryId <= 0) product.ProductCategoryId = _product.ProductCategoryId;
            if (product.GenderId <= 0) product.GenderId = _product.GenderId;

            _product = null;

            //var _productCategoryId = _context.Products.Where(x => x.Id == product.Id).FirstOrDefault().ProductCategoryId;
            //var _genderId = _context.Products.Where(x => x.Id == product.Id).FirstOrDefault().GenderId;

            //if (product.ProductCategoryId <= 0) product.ProductCategoryId = _productCategoryId;
            //if (product.GenderId <= 0) product.GenderId = _genderId;
        }

        public Product InitializeProductOnUpdate(Product product)
        {
            product.Colors = _context.Colors.OrderBy(x => x.Id).ToList();
            product.Genders = _context.Genders.ToList();
            product.ProductCategories = _context.ProductCategories.ToList();
            product.Sizes = _context.Sizes.OrderBy(x => x.Id).ToList();
            product.Statuses = _context.Statuses.OrderBy(x => x.Id).ToList();
            product.StockStatuses = _context.StockStatuses.OrderBy(x => x.Id).ToList();

            if (product.ProductVariants.Any())
            {
                product.ProductVariantJSON = JsonSerializer.Serialize(product.ProductVariants);
            }

            if (product.ProductImages.Any())
            {
                foreach (var item in product.ProductImages)
                {
                    item.ImageBase64String = item.Image;
                }
                product.ProductImageJSON = JsonSerializer.Serialize(product.ProductImages);
            }

            return product;
        }

        public void CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public async Task<Product> UpdateProduct(long id, Product product)
        {
            if (string.IsNullOrEmpty(product.ProductImageJSON))
            {
                var _images = _context.ProductImages.Where(x => x.ProductId == product.Id);
                if (_images.Any())
                {
                    _context.ProductImages.RemoveRange(_images);
                    await _context.SaveChangesAsync();
                }
            }

            if (string.IsNullOrEmpty(product.ProductVariantJSON))
            {
                var _variants = _context.ProductVariants.Where(x => x.ProductId == product.Id);
                if (_variants.Any())
                {
                    _context.ProductVariants.RemoveRange(_variants);
                    await _context.SaveChangesAsync();
                }
            }

            //Start - Delete Non-Existing Images from Repo
            if (product.ProductImages != null && product.ProductImages.Any())
            {
                var _imageIds = product.ProductImages.Select(x => x.Id);
                var _repoImages = await _context.ProductImages.Where(x => x.ProductId == product.Id).ToListAsync();
                var _resultImagesToDelete = _repoImages.Where(x => !_imageIds.Contains(x.Id));
                if (_resultImagesToDelete.Any())
                {
                    _context.ProductImages.RemoveRange(_resultImagesToDelete);
                    await _context.SaveChangesAsync();
                }

                var _newImages = product.ProductImages.Where(x => x.Id == 0);
                if (_newImages.Any())
                {
                    foreach (var item in _newImages)
                    {
                        item.ProductId = product.Id;
                    }
                    product.ProductImages = _newImages;
                    await _context.ProductImages.AddRangeAsync(product.ProductImages);
                    await _context.SaveChangesAsync();
                }

                product.ProductImages = null; //untracked
            }

            //End

            //Start - Delete Non-Existing Variants from Repo
            if (product.ProductVariants != null && product.ProductVariants.Any())
            {
                var _variantIds = product.ProductVariants.Select(x => x.Id);
                var _repoVariants = await _context.ProductVariants.Where(x => x.ProductId == product.Id).ToListAsync();
                var _resultVariantsToDelete = _repoVariants.Where(x => !_variantIds.Contains(x.Id));
                if (_resultVariantsToDelete.Any())
                {
                    _context.ProductVariants.RemoveRange(_resultVariantsToDelete);
                    await _context.SaveChangesAsync();
                }

                var _newVariants = product.ProductVariants.Where(x => x.Id == 0);
                if (_newVariants.Any())
                {
                    foreach (var item in _newVariants)
                    {
                        item.ProductId = product.Id;
                    }
                    product.ProductVariants = _newVariants;
                    await _context.ProductVariants.AddRangeAsync(product.ProductVariants);
                    await _context.SaveChangesAsync();
                }

                product.ProductVariants = null; //untracked
            }
            //End

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task DeleteProduct(long id)
        {
            var result = _context.Products.FirstOrDefault(product => product.Id == id);
            _context.Products.Remove(result);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var result = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                .Include(x => x.InventoryStatus)
                .Include(x => x.Category)
                .ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductImages.Any())
                {
                    var _selectFirstImage = item.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Image = _commonServices.NoImage;
                }
            }

            var inventoryStatus = await _context.StockStatuses.ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductVariants.Sum(x => x.Quantity) == 0)
                {
                    item.StockStatusId = (int)StockStatusEnum.OutOfStock;
                    item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
                    continue;
                }

                if (item.ProductVariants.Sum(x => x.Quantity) <= item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.Critical;
                else if (item.ProductVariants.Sum(x => x.Quantity) > item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.InStock;

                item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
            }

            return result;
        }

        public async Task<List<Product>> GetLatestProducts(int colorId)
        {
            var result = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                .Include(x => x.InventoryStatus)
                .Include(x => x.Category)
                .ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductImages.Any())
                {
                    var _selectFirstImage = item.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Image = _commonServices.NoImage;
                }
            }

            var inventoryStatus = await _context.StockStatuses.ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductVariants.Sum(x => x.Quantity) == 0)
                {
                    item.StockStatusId = (int)StockStatusEnum.OutOfStock;
                    item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
                    continue;
                }

                if (item.ProductVariants.Sum(x => x.Quantity) <= item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.Critical;
                else if (item.ProductVariants.Sum(x => x.Quantity) > item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.InStock;

                item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
            }

            return result.OrderByDescending(x => x.DateCreated).ToList();
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsOnSale()
        {
            var result = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                    .ThenInclude(x => x.Color)
                .Include(x => x.ProductVariants)
                    .ThenInclude(x => x.Size)
                .Include(x => x.InventoryStatus)
                .Where(x => x.IsFeaturedProduct == true && x.IsSale == true)
                .ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductImages.Any())
                {
                    var _selectFirstImage = item.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Image = _commonServices.NoImage;
                }
            }

            var inventoryStatus = await _context.StockStatuses.ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductVariants.Sum(x => x.Quantity) == 0)
                {
                    item.StockStatusId = (int)StockStatusEnum.OutOfStock;
                    item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
                    continue;
                }

                if (item.ProductVariants.Sum(x => x.Quantity) <= item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.Critical;
                else if (item.ProductVariants.Sum(x => x.Quantity) > item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.InStock;

                item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
            }

            return result;
        }

        public async Task<Product> GetProductById(long id)
        {
            var result = await _context.Products
                .Include(x => x.ProductVariants).ThenInclude(x => x.Size)
                .Include(x => x.ProductVariants).ThenInclude(x => x.Color)
                .Include(x => x.ProductImages)
                .Include(x => x.InventoryStatus)
                .Include(x => x.ProductReviews).ThenInclude(x => x.Customers)
                .ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductImages.Any())
                {
                    var _selectFirstImage = item.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Image = _commonServices.NoImage;
                }
            }

            var inventoryStatus = await _context.StockStatuses.ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductVariants.Sum(x => x.Quantity) == 0)
                {
                    item.StockStatusId = (int)StockStatusEnum.OutOfStock;
                    item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
                    continue;
                }

                if (item.ProductVariants.Sum(x => x.Quantity) <= item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.Critical;
                else if (item.ProductVariants.Sum(x => x.Quantity) > item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.InStock;

                item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
            }

            return result.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<Product>> GetAllProductsByGender(int colorId, int genderId)
        {
            var result = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                    .ThenInclude(x => x.Color)
                .Include(x => x.ProductVariants)
                    .ThenInclude(x => x.Size)
                .Include(x => x.InventoryStatus)
                .ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductImages.Any())
                {
                    var _selectFirstImage = item.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Image = _commonServices.NoImage;
                }
            }

            var inventoryStatus = await _context.StockStatuses.ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductVariants.Sum(x => x.Quantity) == 0)
                {
                    item.StockStatusId = (int)StockStatusEnum.OutOfStock;
                    item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
                    continue;
                }

                if (item.ProductVariants.Sum(x => x.Quantity) <= item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.Critical;
                else if (item.ProductVariants.Sum(x => x.Quantity) > item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.InStock;

                item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
            }

            return result.Where(x => x.GenderId == genderId && x.ProductVariants.Any(x => (colorId != 0 ? x.ColorId == colorId : true))).ToList();
        }

        public async Task<List<Product>> GetProductsByCategory(int categoryId, int colorId, int genderId = 0)
        {
            var result = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                    .ThenInclude(x => x.Color)
                .Include(x => x.InventoryStatus)
                .ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductImages.Any())
                {
                    var _selectFirstImage = item.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Image = _commonServices.NoImage;
                }
            }

            var inventoryStatus = await _context.StockStatuses.ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductVariants.Sum(x => x.Quantity) == 0)
                {
                    item.StockStatusId = (int)StockStatusEnum.OutOfStock;
                    item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
                    continue;
                }

                if (item.ProductVariants.Sum(x => x.Quantity) <= item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.Critical;
                else if (item.ProductVariants.Sum(x => x.Quantity) > item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.InStock;

                item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
            }

            var filteredResult = new List<Product>();

            if (genderId == 0)
                filteredResult = result.Where(x => x.ProductCategoryId == categoryId && x.ProductVariants.Any(x => (colorId != 0 ? x.ColorId == colorId : true))).ToList();
            else
                filteredResult = result.Where(x => x.ProductCategoryId == categoryId && x.GenderId == genderId && x.ProductVariants.Any(x => (colorId != 0 ? x.ColorId == colorId : true))).ToList();

            return filteredResult;
        }

        public async Task<List<Product>> GetProductsBySize(int categoryId, int sizeId, int colorId, int genderId = 0)
        {
            var result = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                    .ThenInclude(x => x.Size)
                .Include(x => x.Colors)
                .Include(x => x.InventoryStatus)
                .ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductImages.Any())
                {
                    var _selectFirstImage = item.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Image = _commonServices.NoImage;
                }
            }

            var inventoryStatus = await _context.StockStatuses.ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductVariants.Sum(x => x.Quantity) == 0)
                {
                    item.StockStatusId = (int)StockStatusEnum.OutOfStock;
                    item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
                    continue;
                }

                if (item.ProductVariants.Sum(x => x.Quantity) <= item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.Critical;
                else if (item.ProductVariants.Sum(x => x.Quantity) > item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.InStock;

                item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
            }

            var filteredResult = new List<Product>();

            if (genderId == 0)
                filteredResult = result.Where(x => x.ProductCategoryId == categoryId && x.ProductVariants.Any(x => x.SizeId == sizeId || (colorId != 0 ? x.ColorId == colorId : true))).ToList();
            else
                filteredResult = result.Where(x => x.ProductCategoryId == categoryId && x.ProductVariants.Any(x => x.SizeId == sizeId || (colorId != 0 ? x.ColorId == colorId : true)) && x.GenderId == genderId).ToList();

            return filteredResult;
        }

        public async Task<IEnumerable<ProductReview>> GetProductReviews(long id)
        {
            var result = await _context.ProductReviews
                .Include(x => x.Customers)
                .Where(x => x.ProductId == id)
                .ToListAsync();

            foreach (var item in result)
            {
                item.Customers.Image = item.Customers.Image != null ? item.Customers.Image : _commonServices.NoImage;
            }

            return result;
        }

        public async Task<List<Product>> GetProductsWithSamePrice(long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            var result = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                .Include(x => x.InventoryStatus)
                .Where(x => x.Price == product.Price || x.PriceOnSale == product.Price)
                .ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductImages.Any())
                {
                    var _selectFirstImage = item.ProductImages.FirstOrDefault(); // Get first image that has been added to be  as default image to display
                    item.Image = _selectFirstImage != null ? _selectFirstImage.Image : string.Empty;
                }
                else
                {
                    //No Image
                    item.Image = _commonServices.NoImage;
                }
            }

            var inventoryStatus = await _context.StockStatuses.ToListAsync();

            foreach (var item in result)
            {
                if (item.ProductVariants.Sum(x => x.Quantity) == 0)
                {
                    item.StockStatusId = (int)StockStatusEnum.OutOfStock;
                    item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
                    continue;
                }

                if (item.ProductVariants.Sum(x => x.Quantity) <= item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.Critical;
                else if (item.ProductVariants.Sum(x => x.Quantity) > item.CriticalQty)
                    item.StockStatusId = (int)StockStatusEnum.InStock;

                item.InventoryStatus = inventoryStatus.FirstOrDefault(x => x.Id == item.StockStatusId);
            }

            return result.Where(x => x.StockStatusId != (int)StockStatusEnum.OutOfStock).ToList();
        }

        public async Task UpdateStocks(List<OrderDetails> orderDetails)
        {
            var products = await _context.ProductVariants
                .ToListAsync();

            foreach (var item in orderDetails)
            {
                foreach (var product in products.Where(x => x.ProductId == item.ProductId && x.ColorId == item.ColorId && x.SizeId == item.SizeId)) // must be product variant
                {
                    product.Quantity -= item.Quantity;
                    _context.ProductVariants.Update(product);
                }
            }

            await _context.SaveChangesAsync();
        }

        public void CreateReview(ProductReview productReview)
        {
            _context.ProductReviews.Add(productReview);
            _context.SaveChanges();
        }
    }
}