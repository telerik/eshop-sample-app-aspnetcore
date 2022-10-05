using Models.InputModels;
using Models.ViewModels;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel?> GetUserByLoginCredentials(LoginUserInpuModel input);

        Task<bool> UserExists(string email);

        Task<bool> CreateNewUser(RegisterUserInpuModel input);

        IQueryable<ShoppingCartItemViewModel> GetUserShoppingCartItems(string userEmail);

        Task<int> GetUserShoppingCartItemsCount(string userEmail);

        Task<bool> AddShoppingCartItem(int productId, string userEmail);

        Task<bool> ChangeShoppingCartItemQuantity(int productId, string userEmail, int quantity);

        Task<bool> RemoveShoppingCartItem(int productId, string userEmail);

        Task<bool> ClearUserShoppingCart(string userEmail);

        IQueryable<FavoriteProductViewModel> GetFavoriteProductsById(IEnumerable<int> productIds);
    }
}