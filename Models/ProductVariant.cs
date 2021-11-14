using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce1.Models
{
    public class ProductVariant
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public int ColorId { get; set; }
        public int? SizeId { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public virtual Size Size { get; set; }
        [JsonIgnore]
        public virtual Color Color { get; set; }

    }
}
