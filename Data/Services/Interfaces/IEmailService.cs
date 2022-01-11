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
        public Task SendConfirmationEmail(string recipient, string confirmationLink, string pass);
        public Task SendReceipt(string recipient, Orders orders);
        public Task SendOrderShipped(string recipient, OrderViewModel orders);
        public Task SendOrderReceived(string recipient, OrderViewModel orders);
        public Task SendBlockEmail(string recipient);
        public Task SendTemporaryDeleteEmail(string recipient);
        public Task SendDeleteEmail(string recipient);
        public Task SendResetLinkEmail(string recipient, string resetLink);
        public Task AccountVerified(string userId);
    }
}
