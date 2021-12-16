using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Models;
using ECommerce1.ViewModel;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Message>> GetAllCustomerMessages()
        {
            var result = await _context.Messages
                .Include(x => x.Sender)
                .ToListAsync();

            List<Message> _messages = result
                .GroupBy(l => l.MessageId)
                .Select(cl => new Message
                {
                    MessageId = cl.First().MessageId,
                    SenderId = cl.First().SenderId,
                    Subject = cl.First().Subject,
                    DateSent = cl.First().DateSent,
                    MessageBody = cl.First().MessageBody,
                    Sender = cl.First().Sender
                }).ToList();

            return _messages;
        }

        public async Task<IEnumerable<Message>> GetCustomerMessages(string userId)
        {
            var result = await _context.Messages
                .Include(x => x.Sender)
                .Where(x => x.SenderId == userId)
                .ToListAsync();

            List<Message> _messages = result
                .GroupBy(l => l.MessageId)
                .Select(cl => new Message
                {
                    MessageId = cl.First().MessageId,
                    SenderId = cl.First().SenderId,
                    Subject = cl.First().Subject,
                    DateSent = cl.First().DateSent,
                    MessageBody = cl.First().MessageBody,
                    Sender = cl.First().Sender
                }).ToList();

            return _messages;
        }

        public async Task<IEnumerable<Message>> GetMessageConversation(Guid messageId)
        {
            var result = await _context.Messages
                    .Include(x => x.Sender)
                    .Where(x => x.MessageId == messageId)
                    .ToListAsync();

            return result;
        }

        public async Task<int> GetCustomerMessagesCount(string userId)
        {
            var result = await _context.Messages
                .Include(x => x.Sender)
                .Where(x => x.SenderId == userId)
                .ToListAsync();

            List<Message> _messages = result
                .GroupBy(l => l.MessageId)
                .Select(cl => new Message
                {
                    MessageId = cl.First().MessageId,
                    SenderId = cl.First().SenderId,
                    Subject = cl.First().Subject,
                    DateSent = cl.First().DateSent,
                    MessageBody = cl.First().MessageBody,
                    Sender = cl.First().Sender
                }).ToList();

            return _messages != null ? _messages.Count() : 0;
        }

        public void CreateMessage(MessageViewModel viewModel)
        {
            var message = new Message
            {
                MessageId = viewModel.MessageId != null ? Guid.Parse(viewModel.MessageId) : Guid.NewGuid(),
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