using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
    }
}
