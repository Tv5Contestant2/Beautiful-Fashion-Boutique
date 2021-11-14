using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        public string Image { get; set; }
        public string FileName { get; set; }

        [NotMapped]
        public string ImageBase64String { get; set; }

        [NotMapped]
        [Display(Name = "Upload Image File")]
        public IFormFile ImageFile { get; set; }

       
    }
}
