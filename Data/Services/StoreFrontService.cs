using ECommerce1.Data.Services.Interfaces;
using ECommerce1.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce1.Data.Services
{
    public class StoreFrontService : IStoreFrontService
    {
        private readonly IAdministratorService _administratorService;
        private readonly ICartService _cartService;
        private readonly IProductCategoriesService _productCategoriesService;
        private readonly IProductsService _productService;

        public StoreFrontService(IAdministratorService administratorService
            , ICartService cartService
            , IProductCategoriesService productCategoriesService
            , IProductsService productService)
        {
            _administratorService = administratorService;
            _cartService = cartService;
            _productService = productService;
            _productCategoriesService = productCategoriesService;
        }

        public async Task<StoreFrontViewModel> InitializeStoreFront(string userId, int page, FilterViewModel filterViewModel)
        {
            var cartDetails = await _cartService.GetCartItems(userId);
            var colors = await _productCategoriesService.GetColors();
            var productCategories = await _productCategoriesService.GetAllProductCategories();
            var products = await _productService.GetAllProducts(filterViewModel.GenderId);
            var sizes = await _productCategoriesService.GetSizesPerCategory(filterViewModel.ProductCategoryId);
            var wishlists = await _cartService.GetWishlistItems(userId);

            var result = new StoreFrontViewModel()
            {
                CartCount = cartDetails.Sum(x => x.CartDetails.Quantity),
                CurrentPage = page,
                CustomersId = userId,
                ItemPerPage = 15,
                FacebookLink = _administratorService.GetFacebookLink(),
                Filter = new FilterViewModel() { 
                    Colors = colors,
                    ProductCategories = productCategories,
                    Sizes = sizes,
                },
                Products = products,
                Wishlists = wishlists,
                WishlistCount = wishlists.Count()
            };

            return result;
        }


        public async Task<ProductViewModel> InitializeViewProduct(long productId, string userId)
        {
            var productDetails = await _productService.GetProductById(productId);
            var productReviews = await _productService.GetProductReviews(productId);

            var result = new ProductViewModel()
            {
                IsExistInWishlist = false,
                ProductDetails = productDetails,
                ProductReviews = productReviews,
                Colors = productDetails.ProductVariants.Select(x => x.Color).Distinct(),
                Size = productDetails.ProductVariants.Select(x => x.Size).Distinct()
            };

            if (userId != null)
            {
                var cartDetails = await _cartService.GetCartItems(userId);
                var wishlists = await _cartService.GetWishlistItems(userId);

                result.CartCount = cartDetails.Sum(x => x.CartDetails.Quantity);
                result.IsExistInWishlist = _cartService.CheckIfExistInWishlist(userId, productId);
                result.WishlistCount = wishlists.Count();
            }

            return result;
        }
    }
}
