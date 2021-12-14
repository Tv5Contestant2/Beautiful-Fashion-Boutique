using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce1.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public Guid MessageId { get; set; }
        public string SenderId { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public DateTime DateSent { get; set; }

        public User Sender { get; set; }
    }
}
