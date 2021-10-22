using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ECommerce1.Models
{
    public class About
    {
        public string History { get; set; }
        public string Mission { get; set; }
        public string Vision { get; set; }
    }
}
