using Microsoft.EntityFrameworkCore;
using Models.InputModels;
using Services;
using Data;

namespace Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private readonly AdventureWorksContext dbContext;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            this.dbContext = new AdventureWorksContext(options);
        }

        [TestMethod]
        public async Task GetUserByLoginCredentialsReturnsNullWhenUserDoesNotExist()
        {
            //Arrange
            var loginCredentials = new LoginUserInpuModel
            {
                Email = "Mock Email",
                Password = "Test Password",
            };
            var userService = new UserService(this.dbContext);

            //Act
            var userViewModel = await userService.GetUserByLoginCredentials(loginCredentials);

            //Assert
            Assert.IsNull(userViewModel);
        }

        [TestMethod]
        public async Task GetUserByLoginCredentialsReturnsNullWhenInvalidPasswordIsGiven()
        {
            //Arrange
            var loginCredentials = new LoginUserInpuModel
            {
                Email = "test_email@mail.com",
                Password = "TestPassword",
            };
            var user = new Contact
            {
                FirstName = "FirstName",
                LastName = "LastName",
                EmailAddress = "test_email@mail.com",
                PasswordHash = "MockPasswordHash",
                PasswordSalt = "MockPasswordSalt",
            };
            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var userService = new UserService(this.dbContext);

            //Act
            var userViewModel = await userService.GetUserByLoginCredentials(loginCredentials);

            //Assert
            Assert.IsNull(userViewModel);
        }

        [TestMethod]
        public async Task GetUserByLoginCredentialsReturnsViewModelWhenValidCredentialsAreGiven()
        {
            //Arrange
            var loginCredentials = new LoginUserInpuModel
            {
                Email = "test_email@mail.com",
                Password = "TestPassword",
            };
            var user = new Contact
            {
                FirstName = "First Name",
                LastName = "Last Name",
                EmailAddress = "test_email@mail.com",
                PasswordHash = "da03031a984dc770e6384758654e3043fc5f8de71c98221595216c21704d30b3",
                PasswordSalt = "cQBVZ1Q=",
            };
            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();

            var userService = new UserService(this.dbContext);

            //Act
            var userViewModel = await userService.GetUserByLoginCredentials(loginCredentials);

            //Assert
            Assert.IsNotNull(userViewModel);
            Assert.AreEqual(user.FirstName, userViewModel.FirstName);
            Assert.AreEqual(user.LastName, userViewModel.LastName);
            Assert.AreEqual(user.EmailAddress, userViewModel.EmailAddress);
        }

        [TestMethod]
        public async Task UserExistsReturnsTrueWhenUserExists()
        {
            //Arrange
            var user = new Contact
            {
                FirstName = "Mock First Name",
                LastName = "Mock Last Name",
                EmailAddress = "Mock Email",
                PasswordHash = "Mock Password Hash",
                PasswordSalt = "Mock Password Salt",
            };
            await this.dbContext.AddAsync(user);
            await this.dbContext.SaveChangesAsync();
            var userService = new UserService(this.dbContext);

            //Act
            var exists = await userService.UserExists(user.EmailAddress);

            //Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task UserExistsReturnsFalseWhenUserDoesNotExist()
        {
            //Arrange
            var nonExistantUserEmail = "Mock Email";
            var userService = new UserService(this.dbContext);

            //Act
            var exists = await userService.UserExists(nonExistantUserEmail);

            //Assert
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public async Task CreateNewUserReturnsFalseAndNoNewUserIsAddedWhenInvalidNameIsGiven()
        {
            var registerInputModel1 = new RegisterUserInpuModel
            {
                FirstAndLastName = "InvalidName1",
                Email = "valid_email1@mail.com",
                Password = "TestPassword1",
            };
            var registerInputModel2 = new RegisterUserInpuModel
            {
                FirstAndLastName = " InvalidName2 ",
                Email = "valid_email2@mail.com",
                Password = "TestPassword2",
            };
            var registerInputModel3 = new RegisterUserInpuModel
            {
                FirstAndLastName = "Invalid Name 3",
                Email = "valid_email3@mail.com",
                Password = "TestPassword3",
            };

            var userService = new UserService(this.dbContext);

            //Act
            var result1 = await userService.CreateNewUser(registerInputModel1);
            var result2 = await userService.CreateNewUser(registerInputModel2);
            var result3 = await userService.CreateNewUser(registerInputModel3);

            //Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(await this.dbContext.Contacts.AnyAsync());
        }

        [TestMethod]
        public async Task CreateNewUserReturnsFalseAndNoNewUserIsAddedWhenInvalidPasswordIsGiven()
        {
            var registerInputModel1 = new RegisterUserInpuModel
            {
                FirstAndLastName = "Valid Name",
                Email = "valid_email1@mail.com",
                Password = "123",
            };
            var registerInputModel2 = new RegisterUserInpuModel
            {
                FirstAndLastName = " Valid Name2",
                Email = "valid_email2@mail.com",
                Password = "1234567890123456789012345",
            };

            var userService = new UserService(this.dbContext);

            //Act
            var result1 = await userService.CreateNewUser(registerInputModel1);
            var result2 = await userService.CreateNewUser(registerInputModel2);

            //Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(await this.dbContext.Contacts.AnyAsync());
        }

        [TestMethod]
        public async Task CreateNewUserReturnsTrueAndNewUserIsAddedWhenValidModelIsGiven()
        {
            //Arrange
            var firstName = "Mock";
            var lastName = "User";
            var registerInputModel = new RegisterUserInpuModel
            {
                FirstAndLastName = firstName + ' ' + lastName,
                Email = "test_email@mail.com",
                Password = "TestPassword",
            };

            var userService = new UserService(this.dbContext);

            //Act
            var result = await userService.CreateNewUser(registerInputModel);
            var addedUser = await this.dbContext.Contacts.FirstAsync(c => c.EmailAddress == registerInputModel.Email);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(await this.dbContext.Contacts.AnyAsync());
            Assert.AreEqual(firstName, addedUser.FirstName);
            Assert.AreEqual(lastName, addedUser.LastName);
            Assert.AreEqual(registerInputModel.Email, addedUser.EmailAddress);
            Assert.IsNotNull(addedUser.PasswordHash);
            Assert.IsNotNull(addedUser.PasswordSalt);
        }

        [TestMethod]
        public async Task GetUserShoppingCartItemsMapsDataCorrectly()
        {
            //Arrange
            var userEmail = "user@mail.com";
            var product1 = new Product
            {
                ProductId = 1,
                Name = "Product 1",
                ListPrice = 1234.5M,
                ProductNumber = "pr_1",
                FinishedGoodsFlag = true,
                MakeFlag = true,
            };
            var product2 = new Product
            {
                ProductId = 2,
                Name = "Product 2",
                ListPrice = 12345.6M,
                ProductNumber = "pr_2",
                FinishedGoodsFlag = true,
                MakeFlag = true,
            };
            var productPhotoId1 = new ProductProductPhoto
            {
                ProductId = 1,
                ProductPhotoId = 987
            };
            var productPhotoId2 = new ProductProductPhoto
            {
                ProductId = 2,
                ProductPhotoId = 9876
            };
            var discount1 = new SpecialOffer
            {
                SpecialOfferId = 1,
                DiscountPct = 1.2M,
                Category = "cat_1",
                Description = "desc_1",
                Type = "type_1",
            };
            var discount2 = new SpecialOffer
            {
                SpecialOfferId = 2,
                DiscountPct = 12.3M,
                Category = "cat_2",
                Description = "desc_2",
                Type = "type_2",
            };
            var specialOfferProduct1 = new SpecialOfferProduct
            {
                SpecialOfferId = 1,
                ProductId = 2
            };
            var specialOfferProduct2 = new SpecialOfferProduct
            {
                SpecialOfferId = 2,
                ProductId = 2
            };
            var shoppingCartItem1 = new ShoppingCartItem
            {
                ProductId = 1,
                Quantity = 3,
                ShoppingCartId = userEmail
            };
            var shoppingCartItem2 = new ShoppingCartItem
            {
                ProductId = 2,
                Quantity = 2,
                ShoppingCartId = userEmail
            };

            await this.dbContext.AddAsync(product1);
            await this.dbContext.AddAsync(product2);
            await this.dbContext.AddAsync(productPhotoId1);
            await this.dbContext.AddAsync(productPhotoId2);
            await this.dbContext.AddAsync(discount1);
            await this.dbContext.AddAsync(discount2);
            await this.dbContext.AddAsync(specialOfferProduct1);
            await this.dbContext.AddAsync(specialOfferProduct2);
            await this.dbContext.AddAsync(shoppingCartItem1);
            await this.dbContext.AddAsync(shoppingCartItem2);
            await this.dbContext.SaveChangesAsync();

            var userService = new UserService(this.dbContext);

            //Act
            var resultItems = userService.GetUserShoppingCartItems(userEmail).ToList();

            //Assert
            Assert.AreEqual(2, resultItems.Count());
            var item1 = resultItems.FirstOrDefault(i => i.ProductId == 1);
            var item2 = resultItems.FirstOrDefault(i => i.ProductId == 2);
            Assert.IsNotNull(item1);
            Assert.IsNotNull(item2);
            Assert.AreEqual(product1.Name, item1.ProductName);
            Assert.AreEqual(product1.ListPrice, item1.ProductPrice);
            Assert.AreEqual(0M, item1.DiscountPcnt);
            Assert.AreEqual(shoppingCartItem1.Quantity, item1.Quantity);
            Assert.AreEqual(productPhotoId1.ProductPhotoId, item1.ProductPhotoId);

            Assert.AreEqual(product2.Name, item2.ProductName);
            Assert.AreEqual(product2.ListPrice, item2.ProductPrice);
            Assert.AreEqual(12.3M, item2.DiscountPcnt);
            Assert.AreEqual(shoppingCartItem2.Quantity, item2.Quantity);
            Assert.AreEqual(productPhotoId2.ProductPhotoId, item2.ProductPhotoId);
        }

        [TestMethod]
        public async Task GetUserShoppingCartItemsCountReturnsSumCorrectly()
        {
            //Arrange
            var userEmail = "user@mail.com";

            var shoppingCartItem1 = new ShoppingCartItem
            {
                ProductId = 1,
                Quantity = 3,
                ShoppingCartId = userEmail
            };
            var shoppingCartItem2 = new ShoppingCartItem
            {
                ProductId = 2,
                Quantity = 2,
                ShoppingCartId = userEmail
            };
            var shoppingCartItem3 = new ShoppingCartItem
            {
                ProductId = 3,
                Quantity = 0,
                ShoppingCartId = userEmail
            };
            var shoppingCartItem4 = new ShoppingCartItem
            {
                ProductId = 4,
                Quantity = 1,
                ShoppingCartId = userEmail
            };
            var shoppingCartItem5 = new ShoppingCartItem
            {
                ProductId = 5,
                Quantity = 100,
                ShoppingCartId = "different_user@mail.com"
            };
            var expectedCount = 6;

            await this.dbContext.AddAsync(shoppingCartItem1);
            await this.dbContext.AddAsync(shoppingCartItem2);
            await this.dbContext.AddAsync(shoppingCartItem3);
            await this.dbContext.AddAsync(shoppingCartItem4);
            await this.dbContext.AddAsync(shoppingCartItem5);
            await this.dbContext.SaveChangesAsync();

            var userService = new UserService(this.dbContext);

            //Act
            var itemsCount = await userService.GetUserShoppingCartItemsCount(userEmail);

            //Assert
            Assert.AreEqual(expectedCount, itemsCount);
        }

        [TestMethod]
        public async Task ClearUserShoppingCartReturnsFalseWhenNoItemsAreFound()
        {
            //Arrange
            var userEmail = "user@mail.com";

            var userService = new UserService(this.dbContext);

            //Act
            var result = await userService.ClearUserShoppingCart(userEmail);

            //Assert
            Assert.IsFalse(result);
            Assert.IsFalse(await this.dbContext.ShoppingCartItems.AnyAsync());
        }

        [TestMethod]
        public async Task ClearUserShoppingCartReturnsTrueAndEmptiesBasketWhenItemsAreFound()
        {
            //Arrange
            var userEmail = "user@mail.com";

            var shoppingCartItem1 = new ShoppingCartItem
            {
                ProductId = 1,
                Quantity = 3,
                ShoppingCartId = userEmail
            };
            var shoppingCartItem2 = new ShoppingCartItem
            {
                ProductId = 2,
                Quantity = 2,
                ShoppingCartId = userEmail
            };
            var shoppingCartItem3 = new ShoppingCartItem
            {
                ProductId = 3,
                Quantity = 0,
                ShoppingCartId = userEmail
            };

            await this.dbContext.AddAsync(shoppingCartItem1);
            await this.dbContext.AddAsync(shoppingCartItem2);
            await this.dbContext.AddAsync(shoppingCartItem3);
            await this.dbContext.SaveChangesAsync();

            var userService = new UserService(this.dbContext);

            //Act
            var result = await userService.ClearUserShoppingCart(userEmail);

            //Assert
            Assert.IsTrue(result);
            Assert.IsFalse(await this.dbContext.ShoppingCartItems.AnyAsync());
        }

        [TestMethod]
        public async Task AddShoppingCartItemCreatesNewItemIfItDoesNotExist()
        {
            //Arrange
            var userEmail = "user@mail.com";
            var productId = 1;

            var userService = new UserService(this.dbContext);

            //Act
            var result = await userService.AddShoppingCartItem(productId, userEmail);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(await this.dbContext.ShoppingCartItems.AnyAsync());
            Assert.AreEqual(1, await this.dbContext.ShoppingCartItems.CountAsync());
            var resultItem = await this.dbContext.ShoppingCartItems.FirstAsync();
            Assert.AreEqual(1, resultItem.Quantity);
            Assert.AreEqual(userEmail, resultItem.ShoppingCartId);
            Assert.AreEqual(productId, resultItem.ProductId);
        }

        [TestMethod]
        public async Task AddShoppingCartItemIncreasesQuantityByOneIfItExists()
        {
            //Arrange
            var userEmail = "user@mail.com";
            var productId = 1;

            var shoppingCartItem = new ShoppingCartItem
            {
                ShoppingCartId = userEmail,
                ProductId = productId,
                Quantity = 1,
            };
            await this.dbContext.AddAsync(shoppingCartItem);
            await this.dbContext.SaveChangesAsync();

            var userService = new UserService(this.dbContext);

            //Act
            var result = await userService.AddShoppingCartItem(productId, userEmail);

            //Assert
            Assert.IsTrue(result);
            Assert.IsTrue(await this.dbContext.ShoppingCartItems.AnyAsync());
            Assert.AreEqual(1, await this.dbContext.ShoppingCartItems.CountAsync());
            var resultItem = await this.dbContext.ShoppingCartItems.FirstAsync();
            Assert.AreEqual(2, resultItem.Quantity);
        }

        [TestMethod]
        public async Task ChangeShoppingCartItemQuantityRemovesItemIfQuantityIsLessThanOne()
        {
            //Arrange
            var userEmail1 = "user1@mail.com";
            var productId1 = 1;
            var quantity1 = 12;
            var userEmail2 = "user2@mail.com";
            var productId2 = 2;
            var quantity2 = 123;

            var shoppingCartItem1 = new ShoppingCartItem
            {
                ShoppingCartId = userEmail1,
                ProductId = productId1,
                Quantity = quantity1,
            };
            var shoppingCartItem2 = new ShoppingCartItem
            {
                ShoppingCartId = userEmail2,
                ProductId = productId2,
                Quantity = quantity2,
            };
            await this.dbContext.AddAsync(shoppingCartItem1);
            await this.dbContext.AddAsync(shoppingCartItem2);
            await this.dbContext.SaveChangesAsync();

            var userService = new UserService(this.dbContext);

            //Act
            var result1 = await userService.ChangeShoppingCartItemQuantity(productId1, userEmail1, 0);
            var result2 = await userService.ChangeShoppingCartItemQuantity(productId2, userEmail2, -5);

            //Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(await this.dbContext.ShoppingCartItems.AnyAsync());
        }

        [TestMethod]
        public async Task ChangeShoppingCartItemQuantityUpdatesQuantityCorrectly()
        {
            //Arrange
            var userEmail = "user1@mail.com";
            var productId = 1;
            var quantity = 1;

            var shoppingCartItem = new ShoppingCartItem
            {
                ShoppingCartId = userEmail,
                ProductId = productId,
                Quantity = quantity,
            };
            await this.dbContext.AddAsync(shoppingCartItem);
            await this.dbContext.SaveChangesAsync();

            var userService = new UserService(this.dbContext);

            var expectedQuantity = 3;

            //Act
            var result1 = await userService.ChangeShoppingCartItemQuantity(productId, userEmail, expectedQuantity);

            //Assert
            Assert.IsTrue(result1);
            Assert.AreEqual(1, await this.dbContext.ShoppingCartItems.CountAsync());
            var item = await this.dbContext.ShoppingCartItems.FirstAsync();
            Assert.IsNotNull(item);
            Assert.AreEqual(productId, item.ProductId);
            Assert.AreEqual(userEmail, item.ShoppingCartId);
            Assert.AreEqual(expectedQuantity, item.Quantity);
        }

        [TestMethod]
        public async Task RemoveShoppingCartItemReturnsFalseWhenItemDoesNotExist()
        {
            //Arrange
            var userEmail = "user1@mail.com";
            var productId = 1;

            var userService = new UserService(this.dbContext);

            //Act
            var result = await userService.RemoveShoppingCartItem(productId, userEmail);

            //Assert
            Assert.IsFalse(result);
            Assert.IsFalse(await this.dbContext.ShoppingCartItems.AnyAsync());
        }

        [TestMethod]
        public async Task RemoveShoppingCartItemReturnsTrueAndRemovesItem()
        {
            //Arrange
            var userEmail = "user1@mail.com";
            var productId = 1;

            var shoppingCartItem = new ShoppingCartItem
            {
                ShoppingCartId = userEmail,
                ProductId = productId,
                Quantity = 123,
            };
            await this.dbContext.AddAsync(shoppingCartItem);
            await this.dbContext.SaveChangesAsync();

            Assert.IsTrue(await this.dbContext.ShoppingCartItems.AnyAsync());

            var userService = new UserService(this.dbContext);

            //Act
            var result = await userService.RemoveShoppingCartItem(productId, userEmail);

            //Assert
            Assert.IsTrue(result);
            Assert.IsFalse(await this.dbContext.ShoppingCartItems.AnyAsync());
        }

        [TestMethod]
        public async Task GetFavoriteProductsByIdIgnoresInvalidIds()
        {
            //Arrange
            var product = new Product
            {
                ProductId = 1,
                Name = "Product 1",
                ListPrice = 1234.5M,
                ProductNumber = "pr_1",
                FinishedGoodsFlag = true,
                MakeFlag = true,
            };
            var productPhotoId = new ProductProductPhoto
            {
                ProductId = 1,
                ProductPhotoId = 987
            };

            await this.dbContext.AddAsync(product);
            await this.dbContext.AddAsync(productPhotoId);
            await this.dbContext.SaveChangesAsync();

            var userService = new UserService(this.dbContext);
            var ids = Enumerable.Range(1, 5);

            //Act
            var resulProducts = userService.GetFavoriteProductsById(ids).ToList();

            //Assert
            Assert.AreEqual(1, resulProducts.Count());
            var item = resulProducts.FirstOrDefault(i => i.Id == product.ProductId);
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public async Task GetFavoriteProductsByIdMapsDataCorrectly()
        {
            //Arrange
            var descriptionText = "Mock description";

            var product1 = new Product
            {
                ProductId = 1,
                Name = "Product 1",
                ListPrice = 1234.5M,
                ProductNumber = "pr_1",
                FinishedGoodsFlag = true,
                MakeFlag = true,
                ProductModelId = 13
            };
            var productPhotoId1 = new ProductProductPhoto
            {
                ProductId = 1,
                ProductPhotoId = 987
            };
            var product2 = new Product
            {
                ProductId = 2,
                Name = "Product 2",
                ListPrice = 1234.5M,
                ProductNumber = "pr_2",
                FinishedGoodsFlag = true,
                MakeFlag = true,
            };
            var productPhotoId2 = new ProductProductPhoto
            {
                ProductId = 2,
                ProductPhotoId = 532
            };
            var productModel = new ProductModel
            {
                ProductModelId = 13,
                Name = "Model 1",
            };
            var description = new ProductDescription
            {
                ProductDescriptionId = 17,
                Description = descriptionText,
            };
            var descriptionCulture = new ProductModelProductDescriptionCulture
            {
                CultureId = Guid.NewGuid().ToString(),
                ProductDescriptionId = 17,
                ProductModelId = 13,
            };
            var rating1 = new ProductReview
            {
                EmailAddress = "email1@mail.com",
                ReviewerName = "Reviewer1",
                ProductId = 2,
                Rating = 3,
            };
            var rating2 = new ProductReview
            {
                EmailAddress = "email2@mail.com",
                ReviewerName = "Reviewer2",
                ProductId = 2,
                Rating = 4,
            };

            await this.dbContext.AddAsync(product1);
            await this.dbContext.AddAsync(productPhotoId1);
            await this.dbContext.AddAsync(product2);
            await this.dbContext.AddAsync(productPhotoId2);
            await this.dbContext.AddAsync(productModel);
            await this.dbContext.AddAsync(description);
            await this.dbContext.AddAsync(descriptionCulture);
            await this.dbContext.AddAsync(rating1);
            await this.dbContext.AddAsync(rating2);
            await this.dbContext.SaveChangesAsync();

            var userService = new UserService(this.dbContext);
            var ids = Enumerable.Range(1, 2);

            //Act
            var resulProducts = userService.GetFavoriteProductsById(ids).ToList();

            //Assert
            Assert.AreEqual(2, resulProducts.Count());
            var viewModel1 = resulProducts.FirstOrDefault(i => i.Id == product1.ProductId);
            var viewModel2 = resulProducts.FirstOrDefault(i => i.Id == product2.ProductId);
            Assert.IsNotNull(viewModel1);
            Assert.IsNotNull(viewModel2);

            Assert.AreEqual(product1.ProductId, viewModel1.Id);
            Assert.AreEqual(product1.Name, viewModel1.Name);
            Assert.AreEqual(product1.ListPrice, viewModel1.Price);
            Assert.AreEqual(descriptionText, viewModel1.Description);
            Assert.AreEqual(productModel.ProductModelId, viewModel1.ModelId);
            Assert.AreEqual(0, viewModel1.AverageRating);
            Assert.AreEqual(productPhotoId1.ProductPhotoId, viewModel1.PhotoId);

            Assert.AreEqual(product2.ProductId, viewModel2.Id);
            Assert.AreEqual(product2.Name, viewModel2.Name);
            Assert.AreEqual(product2.ListPrice, viewModel2.Price);
            Assert.AreEqual("...", viewModel2.Description);
            Assert.AreEqual(null, viewModel2.ModelId);
            Assert.AreEqual(3.5, viewModel2.AverageRating);
            Assert.AreEqual(productPhotoId2.ProductPhotoId, viewModel2.PhotoId);
        }
    }
}