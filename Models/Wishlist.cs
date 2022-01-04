using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class Wishlist
    {
        [Key]
        public int Id { get; set; }
        public long ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public string CustomersId { get; set; }

        public Product Product { get; set; }
    }
}
