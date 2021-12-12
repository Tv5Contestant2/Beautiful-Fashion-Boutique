using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class SocMed
    {
        [Key]
        public int Id { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }

        [Display(Name = "Store Hours")]
        public string StoreHours { get; set; }

        [Display(Name = "Store Location")]
        public string StoreLocation { get; set; }

        [Display(Name = "Map URL")]
        public string MapLocation { get; set; }
    }
}
