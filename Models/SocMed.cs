using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class SocMed
    {
        [Key]
        public int Id { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string StoreHours { get; set; }
        public string StoreLocation { get; set; }
        public string MapLocation { get; set; }
    }
}
