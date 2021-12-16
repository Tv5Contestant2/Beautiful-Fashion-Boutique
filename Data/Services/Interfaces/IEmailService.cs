using ECommerce1.Models;
using ECommerce1.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendMessage(MessageViewModel viewModel);
        public Task SendConfirmationEmail(string recipient, string confirmationLink);
        public Task SendReceipt(string recipient, List<OrderDetails> orderDetails, Orders orders);
        public Task SendResetLinkEmail(string recipient, string resetLink);
        public Task AccountVerified(string userId);
    }
}
