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

        public Task<IEnumerable<Product>> GetAllProducts();

        public Task<IEnumerable<Product>> GetFeaturedProductsOnSale();

        public Task<Product> GetProductById(long id);

        public Task<IEnumerable<Product>> GetAllProductsByGender(int genderId);

        public void InitializeProductListForResponse(Product product);
    }
}
