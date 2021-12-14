using ECommerce1.Models;
using ECommerce1.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IMessageService
    {
        public Task<IEnumerable<Message>> GetCustomerMessages(string userId);
        public Task<int> GetCustomerMessagesCount(string userId);

        public void CreateMessage(MessageViewModel viewModel);
    }
}
