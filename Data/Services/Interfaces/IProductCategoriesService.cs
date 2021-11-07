using ECommerce1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public interface IProductCategoriesService
    {
        public Task<IEnumerable<ProductCategory>> GetAllProductCategories();
    }
}
