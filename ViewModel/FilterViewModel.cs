using ECommerce1.Models;
using System.Collections.Generic;

namespace ECommerce1.ViewModel
{
    public class FilterViewModel
    {
        public int ColorId { get; set; }
        public int GenderId { get; set; }
        public int ProductCategoryId { get; set; }
        public int SizeId { get; set; }
        public string SearchString { get; set; }
        public IEnumerable<Color> Colors { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<Size> Sizes { get; set; }

    }
}
