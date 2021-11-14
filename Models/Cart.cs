using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public long ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int GenderId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
