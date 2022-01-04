using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class CartDetails
    {
        [Key]
        public int Id { get; set; }
        public long CartId { get; set; }
        public long ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }

        public virtual Color Colors { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductVariant ProductVariants { get; set; }
        public virtual Size Sizes { get; set; }
    }
}
