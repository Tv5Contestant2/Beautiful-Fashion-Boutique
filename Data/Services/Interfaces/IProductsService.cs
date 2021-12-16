using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IProductsService
    {
        Product InitializeProduct();

        Product InitializeProductOnUpdate(Product product);

        public void CreateProduct (Product product);

        public Task<Product> UpdateProduct(long id, Product product);

        public Task DeleteProduct(long id);

        public Task<List<Product>> GetAllProducts(int colorId);

        public Task<List<Product>> GetLatestProducts(int colorId);

        public Task<IEnumerable<Product>> GetFeaturedProductsOnSale();

        public Task<Product> GetProductById(long id);

        public Task<List<Product>> GetAllProductsByGender(int colorId, int genderId);

        public Task<List<Product>> GetProductsByCategory(int categoryId, int colorId, int genderId = 0);

        public Task<List<Product>> GetProductsBySize(int categoryId, int sizeId, int colorId, int genderId = 0);


        public Task<IEnumerable<ProductReview>> GetProductReviews(long id);

        public Task<List<Product>> GetProductsWithSamePrice(long id);

        public void InitializeProductListForResponse(Product product);

        public Task UpdateStocks(List<OrderDetails> orderDetails);

        public void CreateReview(ProductReview productReview);
    }
}
