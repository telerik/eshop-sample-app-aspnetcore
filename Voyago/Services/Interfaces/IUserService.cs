using Data;
using Models.InputModels;
using Models.ViewModels;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel?> GetUserByLoginCredentials(LoginUserInpuModel input);

        Task<UserProfileViewModel?> GetUserPersonalDetails(string email);

        Task<bool> UserExists(string email);

        Task<bool> CreateNewUser(RegisterUserInpuModel input);

        Task<bool> EditUserDetails(ProfileUserInputModel input, string email);

        Task<bool> EditUserPassword(PasswordUserInputModel input, string email);

        Task<bool> EditUserAddress(AddressUserInputModel input, string email);

        IQueryable<ShoppingCartItemViewModel> GetUserShoppingCartItems(string userEmail);

        Task<int> GetUserShoppingCartItemsCount(string userEmail);

        Task<bool> AddShoppingCartItem(int productId, string userEmail);

        Task<bool> ChangeShoppingCartItemQuantity(int productId, string userEmail, int quantity);

        Task<bool> RemoveShoppingCartItem(int productId, string userEmail);

        Task<bool> ClearUserShoppingCart(string userEmail);

        IQueryable<FavoriteProductViewModel> GetFavoriteProductsById(IEnumerable<int> productIds);
    }
}