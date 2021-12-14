using ECommerce1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.ViewModel
{
    public class MessageViewModel
    {
        public string MessageId { get; set; }
        public string SenderId { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public DateTime DateSent { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
}
