using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IUserService
    {
        public Task UpdateLastLoggedIn(string userId);
        public Task ArchiveUsers();
        public Task DeleteCustomers();
    }
}
