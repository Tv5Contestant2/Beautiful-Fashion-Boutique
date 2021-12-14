using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class MessageService : IMessageService
    {
        private readonly AppDBContext _context;

        public MessageService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetCustomerMessages(string userId)
        {
            var result = await _context.Messages
                .ToListAsync();

            return result;
        }

        public async Task<int> GetCustomerMessagesCount(string userId)
        {
            var result = await _context.Messages
                .Where(x => x.SenderId == userId)
                .ToListAsync();

            return result != null ? result.Count() : 0;
        }

        public void CreateMessage(MessageViewModel viewModel)
        {
            var message = new Message
            {
                MessageId = new Guid(),
                SenderId = viewModel.SenderId,
                Subject = viewModel.Subject,
                MessageBody = viewModel.MessageBody,
                DateSent = DateTime.Now
            };

            _context.Messages.Add(message);
            _context.SaveChanges();
        }
    }
}