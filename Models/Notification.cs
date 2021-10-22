using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce1.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        public int CustomerID { get; set; }
        public DateTime NotifCreationDate { get; set; }
        public string NotifTitle { get; set; }
        public string NotifDesc { get; set; }
        public int NotifRead { get; set; }
        public int NotifTrash { get; set; }
    }
}
