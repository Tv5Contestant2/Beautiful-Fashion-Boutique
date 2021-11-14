using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECommerce1.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        [JsonIgnore]
        public string Image { get; set; }
        public string FileName { get; set; }
        public long ProductId { get; set; }

        [NotMapped]
        public string ImageBase64String { get; set; }

        [JsonIgnore]
        [NotMapped]
        [Display(Name = "Upload Image File")]
        public IFormFile ImageFile { get; set; }

       
    }
}
