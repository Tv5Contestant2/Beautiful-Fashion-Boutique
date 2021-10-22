using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ECommerce1.Models
{
    public class Announcements
    {
        [Key]
        public int AnnouncementID { get; set; }
        public DateTime AnnouncementCreation { get; set; }
        public string AnnouncementDesc { get; set; }

    }
}
