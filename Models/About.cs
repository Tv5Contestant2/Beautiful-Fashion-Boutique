using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ECommerce1.Models
{
    public class About
    {
        [Key]
        public int Id { get; set; }
        [AllowHtml]
        public string History { get; set; }
        [AllowHtml]
        public string Mission { get; set; }
        [AllowHtml]
        public string Vision { get; set; }
    }
}
