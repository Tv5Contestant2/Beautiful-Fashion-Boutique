using ECommerce1.ViewModel;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services.Interfaces
{
    public interface IStoreFrontService
    {
        public Task<StoreFrontViewModel> InitializeStoreFront(string userId, int page, FilterViewModel filterViewModel);

        public Task<ProductViewModel> InitializeViewProduct(long productId, string userId = null);

    }
}
