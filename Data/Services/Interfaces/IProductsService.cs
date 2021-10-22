using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IProductsService
    {
        public Task<IEnumerable<Product>> GetAllProducts();
        public Product GetProductById(long Id);
        public void CreateProduct (Product product);
        public Product UpdateProduct(long Id, Product product);
        public Task<bool> DeleteProduct(long Id);
        Product InitializeProduct();
    }
}
