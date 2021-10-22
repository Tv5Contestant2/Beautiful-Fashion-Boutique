using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
