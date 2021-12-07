using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class CartDetails
    {
        [Key]
        public int Id { get; set; }
        public long ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int GenderId { get; set; }
        public int Quantity { get; set; }
        public string CustomersId { get; set; }

        public Product Product { get; set; }
    }
}
