﻿using ECommerce1.Models;
using System.Collections.Generic;

namespace ECommerce1.ViewModel
{
    public class CartViewModel
    {
        public Cart Cart { get; set; }
        public string CustomersId { get; set; }
        public IEnumerable<ProductVariant> ProductVariants { get; set; }
        public IEnumerable<CartDetails> CartDetails { get; set; }

        public int CartCount { get; set; }
        public int WishlistCount { get; set; }
        public List<Color> Colors { get; set; }
        public List<Size> Sizes { get; set; }

    }
}
