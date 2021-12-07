using ECommerce1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.ViewModel
{
    public class CartViewModel
    {
        public Cart Cart { get; set; }
        public IEnumerable<CartDetails> CartDetails { get; set; }
    }
}
