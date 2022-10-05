namespace EndToEndTests
{
    [TestClass]
    public class ShoppingCartTests
    {
        protected IWebDriver webDriver = null!;
        protected ShoppingCartPage shoppingCartPage = null!;

        [TestInitialize]
        public void CreateDriver()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            webDriver = new ChromeDriver();
            shoppingCartPage = new ShoppingCartPage(webDriver);
            Login(shoppingCartPage.TestAccountEmail, shoppingCartPage.TestAccountPassword);
        }

        [TestCleanup]
        public void QuitDriver()
        {
            webDriver.Quit();
        }

        public void Dispose()
        {
            webDriver.Dispose();
        }

        [TestMethod]
        public void EmptyCartHasNecessaryElements()
        {
            shoppingCartPage.NavigateTo();

            Assert.AreEqual("Shopping Cart - Web", shoppingCartPage.Title);
            Assert.AreEqual("Your cart items", shoppingCartPage.GetMainHeadingText());            
            Assert.AreEqual("Unfortunately, your cart is empty...", shoppingCartPage.GetEmptyCartHeadingText());
            Assert.AreEqual("Looks like you have not added anything to your cart. Go ahead and", shoppingCartPage.GetEmptyCartDetailsText());
            Assert.AreEqual("explore our top-picks.", shoppingCartPage.GetEmptyCartHrefText());
            Assert.AreEqual("https://localhost:62094/Home/Index#top-picks", shoppingCartPage.GetEmptyCartHrefValue());
        }

        [TestMethod]
        public void NonEmptyCartHasNecessaryElements()
        {
            shoppingCartPage.FillShoppingCart();
            shoppingCartPage.NavigateTo();

            Assert.AreEqual("Shopping Cart - Web", shoppingCartPage.Title);
            Assert.AreEqual("Your cart items", shoppingCartPage.GetMainHeadingText());
            Assert.AreEqual("Continue shopping", shoppingCartPage.GetContinueShoppingHrefText());
            Assert.AreEqual("https://localhost:62094/Products/Summary", shoppingCartPage.GetContinueShoppingHrefValue());
            Assert.AreEqual("Sub-total:", shoppingCartPage.GetSubTotalText());
            Assert.IsTrue(shoppingCartPage.GetSubTotalValue().StartsWith("$ "));
            Assert.AreEqual("Tax and shipping cost will be calculated later", shoppingCartPage.GetTaxAndShippingText());
            Assert.AreEqual("Check-out", shoppingCartPage.GetCheckoutButtonText());
            Assert.IsNotNull(shoppingCartPage.GetGridText());

            shoppingCartPage.Checkout();
        }

        [TestMethod]
        public void QuantityInputUpdatesItemQuantity()
        {
            shoppingCartPage.FillShoppingCart();
            shoppingCartPage.NavigateTo();
            const int input = 5;

            shoppingCartPage.EditItemQuantity(input.ToString());
            shoppingCartPage.NavigateTo();

            Assert.AreEqual(input, int.Parse(shoppingCartPage.GetItemQuantityText()));

            shoppingCartPage.Checkout();
        }

        [TestMethod]
        public void MinimumAllowedItemQuantityIsOne()
        {
            shoppingCartPage.FillShoppingCart();
            shoppingCartPage.NavigateTo();
            const int input = 0;
            const int expected = 1;

            shoppingCartPage.EditItemQuantity(input.ToString());
            shoppingCartPage.NavigateTo();

            Assert.AreEqual(expected, int.Parse(shoppingCartPage.GetItemQuantityText()));

            shoppingCartPage.Checkout();
        }

        [TestMethod]
        public void MaximumAllowedItemQuantityIsOneHundred()
        {
            shoppingCartPage.FillShoppingCart();
            shoppingCartPage.NavigateTo();
            const int input = 101;
            const int expected = 100;

            shoppingCartPage.EditItemQuantity(input.ToString());
            shoppingCartPage.NavigateTo();

            Assert.AreEqual(expected, int.Parse(shoppingCartPage.GetItemQuantityText()));

            shoppingCartPage.Checkout();
        }

        [TestMethod]
        public void SpinnerIncreaseUpdatesItemQuantityByOne()
        {
            shoppingCartPage.FillShoppingCart();
            shoppingCartPage.NavigateTo();

            var before = int.Parse(shoppingCartPage.GetItemQuantityText());
            shoppingCartPage.IncreaseItemQuantity();
            shoppingCartPage.NavigateTo();

            Assert.AreEqual(before + 1, int.Parse(shoppingCartPage.GetItemQuantityText()));

            shoppingCartPage.Checkout();
        }

        [TestMethod]
        public void SpinnerDecreaseUpdatesItemQuantityByOne()
        {
            shoppingCartPage.FillShoppingCart();
            shoppingCartPage.NavigateTo();
            const int greaterThanOne = 17;

            shoppingCartPage.EditItemQuantity(greaterThanOne.ToString());
            var before = int.Parse(shoppingCartPage.GetItemQuantityText());
            shoppingCartPage.DecreaseItemQuantity();
            shoppingCartPage.NavigateTo();

            Assert.AreEqual(before - 1, int.Parse(shoppingCartPage.GetItemQuantityText()));

            shoppingCartPage.Checkout();
        }

        [TestMethod]
        public void SubTotalIsCalculatedAccurately()
        {
            shoppingCartPage.FillShoppingCart();
            shoppingCartPage.NavigateTo();

            var expectedSubTotal = CalculateSubTotal();
            var actualSubTotal = shoppingCartPage.GetSubTotalValueAsNumber();            

            Assert.AreEqual(expectedSubTotal, actualSubTotal);

            shoppingCartPage.Checkout();
        }

        [TestMethod]
        public void SubTotalIsUpdatedDynamically()
        {
            shoppingCartPage.FillShoppingCart();
            shoppingCartPage.NavigateTo();

            var firstSubTotal = shoppingCartPage.GetSubTotalValueAsNumber();
            var firstExpectedSubTotal = CalculateSubTotal();
            shoppingCartPage.IncreaseItemQuantity();
            var secondSubTotal = shoppingCartPage.GetSubTotalValueAsNumber();
            var secondExpectedSubTotal = CalculateSubTotal();
            shoppingCartPage.DecreaseItemQuantity();
            var thirdSubTotal = shoppingCartPage.GetSubTotalValueAsNumber();
            var thirdExpectedSubTotal = CalculateSubTotal();

            Assert.AreEqual(firstSubTotal, firstExpectedSubTotal);
            Assert.AreEqual(secondSubTotal, secondExpectedSubTotal);
            Assert.AreEqual(thirdSubTotal, thirdExpectedSubTotal);
            Assert.AreEqual(firstSubTotal, thirdSubTotal);
            Assert.IsTrue(firstSubTotal < secondSubTotal);

            shoppingCartPage.Checkout();
        }

        [TestMethod]
        public void RemoveButtonDeletesItemFromShoppingCart()
        {
            shoppingCartPage.FillShoppingCart();
            shoppingCartPage.NavigateTo();

            var itemsCountBeforeRemove = shoppingCartPage.GetGridText().Split(Environment.NewLine).Skip(1).Count() /4;
            shoppingCartPage.RemoveItem();
            shoppingCartPage.NavigateTo();
            var itemsCountAfterRemove = shoppingCartPage.GetGridText().Split(Environment.NewLine).Skip(1).Count() / 4;

            Assert.AreEqual(itemsCountBeforeRemove, itemsCountAfterRemove + 1);

            shoppingCartPage.Checkout();
        }

        [TestMethod]
        public void CheckoutButtonEmptiesShoppingCart()
        {
            shoppingCartPage.FillShoppingCart();
            shoppingCartPage.NavigateTo();

            shoppingCartPage.Checkout();
            shoppingCartPage.NavigateTo();

            Assert.AreEqual("Unfortunately, your cart is empty...", shoppingCartPage.GetEmptyCartHeadingText());
            Assert.AreEqual("Looks like you have not added anything to your cart. Go ahead and", shoppingCartPage.GetEmptyCartDetailsText());
            Assert.AreEqual("explore our top-picks.", shoppingCartPage.GetEmptyCartHrefText());
            Assert.AreEqual("https://localhost:62094/Home/Index#top-picks", shoppingCartPage.GetEmptyCartHrefValue());
        }

        private double CalculateSubTotal()
        {
            var gridText = shoppingCartPage.GetGridText().Split(Environment.NewLine);
            var total = 0D;
            for (int i = 4; i < gridText.Count(); i += 4)
            {
                var currentNumberStr = gridText[i].Substring(1);
                if (currentNumberStr == null)
                {
                    throw new InvalidDataException("Item Total is empty");
                }
                try
                { total += double.Parse(currentNumberStr); }
                catch (Exception)
                { throw; }
            }

            return Math.Round(total, 2);
        }

        private void Login(string email, string password)
        {
            const string loginURI = "https://localhost:62094/Account/Login";
            webDriver.Navigate().GoToUrl(loginURI);

            var emailField = webDriver.FindElement(By.Id("Email"));
            var passwordField = webDriver.FindElement(By.Id("Password"));
            var loginButton = webDriver.FindElement(By.CssSelector("button.k-button.k-form-submit"));

            emailField.SendKeys(email);
            passwordField.SendKeys(password);
            loginButton.Click();
        }
    }
}