using ECommerce1.Models;
using System.Collections.Generic;

namespace ECommerce1.Data.Repositories
{
    public class ProductCategoryRepository
    {
        public IEnumerable<ProductCategory> GetProductCategories()
        {
            List<ProductCategory> categories = new List<ProductCategory>()
            {
                new ProductCategory
                {
                    Id = 1,
                    Title = "T-Shirt"
                },
                new ProductCategory
                {
                    Id = 2,
                    Title = "Jacket"
                }
            };

            return categories;
            }
        }
}
