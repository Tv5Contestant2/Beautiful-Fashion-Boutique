using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class Modules
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

    }
}
