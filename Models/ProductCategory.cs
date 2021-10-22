using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
