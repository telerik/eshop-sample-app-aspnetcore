using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Services.Interfaces;
using Models.InputModels;
using Models.ViewModels;
using System.Text;
using Data;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly EShopDatabaseContext dbContext;

        public UserService(EShopDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<UserViewModel?> GetUserByLoginCredentials(LoginUserInpuModel input)
        {
            if (await dbContext.Contacts.AnyAsync(user => user.EmailAddress == input.Email))
            {
                var attemptedUser = await dbContext.Contacts.FirstAsync(user => user.EmailAddress == input.Email);
                var attemptedUserPasswordSalt = attemptedUser.PasswordSalt;
                var attemptedPasswordWithSalt = input.Password + attemptedUserPasswordSalt;

                if (HashPassword(attemptedPasswordWithSalt) != attemptedUser.PasswordHash)
                {
                    return null;
                }

                var userViewModel = new UserViewModel()
                {
                    Id = attemptedUser.ContactId,
                    EmailAddress = attemptedUser.EmailAddress,
                    FirstName = attemptedUser.FirstName,
                    LastName = attemptedUser.LastName,
                    Phone = attemptedUser.Phone
                };

                return userViewModel;
            }

            return null;
        }

        public async Task<UserProfileViewModel?> GetUserPersonalDetails(string email)
        {
            if (await dbContext.Contacts.AnyAsync(user => user.EmailAddress == email))
            {
                var currentUser = await dbContext.Contacts.FirstAsync(user => user.EmailAddress == email);
                var userViewModel = new UserProfileViewModel()
                {
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    Phone = currentUser.Phone,
                    EmailAddress = currentUser.EmailAddress,
                    City = currentUser.City,
                    Zipcode = currentUser.ZipCode,
                    Street = currentUser.Street,
                    State = currentUser.State,
                    Country = currentUser.Country
                };
                return userViewModel;
            }
            return null;
        }

        public async Task<bool> UserExists(string email)
        {
            return await dbContext.Contacts.AnyAsync(c => c.EmailAddress == email);
        }

        public async Task<bool> CreateNewUser(RegisterUserInpuModel input)
        {
            var inputName = input.FirstAndLastName.Trim();
            if (inputName.Split(' ').Count() != 2)
            {
                return false;
            }

            var firstName = inputName.Split(' ').First();
            var lastName = inputName.Split(' ').Last();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }
            if (input.Password.Length < 8)
            {
                return false;
            }

            var passwordSalt = GenerateSalt(8);
            var passwordWithSalt = input.Password + passwordSalt;
            var hashedPassword = HashPassword(passwordWithSalt);

            var userForDb = new Contact
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = input.Email,
                PasswordHash = hashedPassword,
                PasswordSalt = passwordSalt
            };

            await dbContext.Contacts.AddAsync(userForDb);
            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> EditUserDetails(ProfileUserInputModel input, string email)
        {
            if (!await UserExists(email))
            {
                return false;
            }

            if (string.IsNullOrEmpty(input.FirstName) || string.IsNullOrEmpty(input.LastName))
            {
                return false;
            }

            var userDetailsFromDb = await dbContext.Contacts.FirstAsync(sci => sci.EmailAddress == email);
            userDetailsFromDb.FirstName = input.FirstName;
            userDetailsFromDb.LastName = input.LastName;
            userDetailsFromDb.Phone = input.Phone;

            dbContext.Update(userDetailsFromDb);

            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> EditUserPassword(PasswordUserInputModel input, string email)
        {
            var passwordSalt = GenerateSalt(8);
            var passwordWithSalt = input.Password + passwordSalt;
            var hashedPassword = HashPassword(passwordWithSalt);

            var userDetailsFromDb = await dbContext.Contacts.FirstAsync(sci => sci.EmailAddress == email);
            userDetailsFromDb.PasswordHash = hashedPassword;
            userDetailsFromDb.PasswordSalt = passwordSalt;

            dbContext.Update(userDetailsFromDb);

            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> EditUserAddress(AddressUserInputModel input, string email)
        {
            var userAddressFromDb = await dbContext.Contacts.FirstAsync(sci => sci.EmailAddress == email);
            userAddressFromDb.Street = input.Street;
            userAddressFromDb.City = input.City;
            userAddressFromDb.ZipCode = input.Zipcode;
            userAddressFromDb.Country = input.Country;
            userAddressFromDb.State = input.State;

            dbContext.Contacts.Update(userAddressFromDb);

            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public IQueryable<ShoppingCartItemViewModel> GetUserShoppingCartItems(string userEmail)
        {
            var shoppingCartitems = (from item in dbContext.ShoppingCartItems
                where item.ShoppingCartId == userEmail
                join product in dbContext.Products on item.ProductId equals product.ProductId
                select new ShoppingCartItemViewModel
                {
                    ShoppingCartItemId = item.ShoppingCartItemId,
                    ShoppingCartId = userEmail,
                    ProductId = item.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ListPrice,
                    DiscountPcnt = product.DiscountPct != null ? product.DiscountPct : 0,
                    Quantity = item.Quantity,
                    ProductPhotoId = product.PhotoId
                });
            return shoppingCartitems;
        }

        public async Task<int> GetUserShoppingCartItemsCount(string userEmail)
        {
            return await dbContext.ShoppingCartItems.Where(sci => sci.ShoppingCartId == userEmail).Select(sci => sci.Quantity).SumAsync();
        }

        public async Task<bool> AddShoppingCartItem(int productId, string userEmail)
        {
            if (await ShoppingCartItemExists(productId, userEmail))
            {
                var quantity = (await dbContext.ShoppingCartItems.FirstAsync(sci => sci.ShoppingCartId == userEmail && sci.ProductId == productId)).Quantity;

                return await ChangeShoppingCartItemQuantity(productId, userEmail, quantity + 1);
            }

            var shoppingCartItemForDb = new ShoppingCartItem
            {
                ShoppingCartId = userEmail,
                ProductId = productId,
                Quantity = 1,
                DateCreated = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };

            await dbContext.ShoppingCartItems.AddAsync(shoppingCartItemForDb);

            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> ChangeShoppingCartItemQuantity(int productId, string userEmail, int quantity)
        {
            if (!await ShoppingCartItemExists(productId, userEmail))
            {
                return false;
            }
            if (quantity <= 0)
            {
                return await RemoveShoppingCartItem(productId, userEmail);
            }

            var shoppingCartItemFromDb = await dbContext.ShoppingCartItems.FirstAsync(sci => sci.ShoppingCartId == userEmail && sci.ProductId == productId);
            shoppingCartItemFromDb.Quantity = quantity;

            dbContext.Update(shoppingCartItemFromDb);

            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> RemoveShoppingCartItem(int productId, string userEmail)
        {
            if (!await ShoppingCartItemExists(productId, userEmail))
            {
                return false;
            }

            var shoppingCartItemFromDb = await dbContext.ShoppingCartItems.FirstAsync(sci => sci.ShoppingCartId == userEmail && sci.ProductId == productId);

            dbContext.ShoppingCartItems.Remove(shoppingCartItemFromDb);

            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        private async Task<bool> ShoppingCartItemExists(int productId, string userEmail)
        {
            return await dbContext.ShoppingCartItems.AnyAsync(sci => sci.ShoppingCartId == userEmail && sci.ProductId == productId);
        }

        public async Task<bool> ClearUserShoppingCart(string userEmail)
        {
            var userItems = dbContext.ShoppingCartItems.Where(sci => sci.ShoppingCartId == userEmail);

            if (await userItems.AnyAsync())
            {
                dbContext.ShoppingCartItems.RemoveRange(userItems);
            }

            return await dbContext.SaveChangesAsync() > 0 ? true : false;
        }

        public IQueryable<FavoriteProductViewModel> GetFavoriteProductsById(IEnumerable<int> productIds)
        {
            var products = dbContext.Products.Where(p => productIds.Contains(p.ProductId)).Select(p => new FavoriteProductViewModel
            {
                Id = p.ProductId,
                Name = p.ProductName,
                Price = p.ListPrice,
                DiscountPct = p.DiscountPct != null ? p.DiscountPct : 0 ,
                Description = p.Description != null ? p.Description : "...",
                ModelId = p.ProductModelId,
                AverageRating = p.Rating != null ? p.Rating : 0,
                PhotoId = p.PhotoId
            });

            return products;
        }

        private string HashPassword(string password)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(password));
                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }

        private string GenerateSalt(int length)
        {
            string allowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/+";

            Random random = new Random();

            return new string(Enumerable.Repeat(allowedCharacters, length - 1).Select(s => s[random.Next(s.Length)]).ToArray()) + '=';
        }
    }
}