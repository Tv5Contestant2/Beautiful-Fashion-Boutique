using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Settings
    {
        [Key]
        public int Id { get; set; }
        public string StoreLogo { get; set; }
        public string StoreBanner { get; set; }
        public string EmailLogo { get; set; }
    }
}
