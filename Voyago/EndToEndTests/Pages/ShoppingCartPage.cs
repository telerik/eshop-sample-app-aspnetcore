namespace EndToEndTests.Pages
{
    public class ShoppingCartPage
    {
        private readonly IWebDriver webDriver;
        private const string URI = "https://localhost:62094/Account/ShoppingCart";
        private const string testAccountEmail = "shoppingCartTests@mail.com";
        private const string testAccountPassword = "shoppingCartTests";

        private IWebElement ShoppingCartHeading => webDriver.FindElement(By.ClassName("shopping-cart-heading"));
        private IWebElement ContinueShoppingHref => webDriver.FindElement(By.ClassName("continue-shopping"));
        private IWebElement EmptyCartHref => webDriver.FindElement(By.CssSelector("div.emtpy-cart-text > a"));
        private IWebElement Grid => webDriver.FindElement(By.Id("shoppingCartGrid"));
        private IWebElement CheckoutButton => webDriver.FindElement(By.Id("checkoutButton"));
        private IWebElement RemoveItemButton => webDriver.FindElements(By.CssSelector("[id^='remove_']"))[0];
        private IWebElement EditQuantityInput => webDriver.FindElements(By.CssSelector(".quantity-editor.k-input-inner"))[0];
        private IWebElement SpinnerIncrease => webDriver.FindElements(By.ClassName("k-spinner-increase"))[0];
        private IWebElement SpinnerDecrease => webDriver.FindElements(By.ClassName("k-spinner-decrease"))[0];

        public string Title => this.webDriver.Title;
        public string TestAccountEmail => testAccountEmail;
        public string TestAccountPassword => testAccountPassword;

        public ShoppingCartPage(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        public void NavigateTo()
        {
            webDriver.Navigate().GoToUrl(URI);
        }
        public void Unfocus()
        {
            ShoppingCartHeading.Click();
        }
        public void RemoveItem()
        {
            RemoveItemButton.Click();
            Unfocus();
        }
        public void EditItemQuantity(string input)
        {
            EditQuantityInput.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + input + Keys.Enter);
            Unfocus();
        }        
        public void IncreaseItemQuantity()
        {
            SpinnerIncrease.Click();
            Unfocus();
        }
        public void DecreaseItemQuantity()
        {
            SpinnerDecrease.Click();
            Unfocus();
        }
        public void Checkout()
        {
            if (CheckoutButton != null)
            {
                CheckoutButton.Click();
                Unfocus();
            }
        }
        public void FillShoppingCart()
        {
            webDriver.Navigate().GoToUrl("https://localhost:62094/Products/Category?subCategory=Touring%20Bikes");
            var elements = webDriver.FindElements(By.CssSelector("[id^='addToCartButton_']")).Take(3);
            foreach (var element in elements)
            {
                element.Click();
            }
        }

        public string GetMainHeadingText()
        {
            return ShoppingCartHeading.Text;
        }
        public string GetContinueShoppingHrefText()
        {
            return ContinueShoppingHref.Text;
        }
        public string GetContinueShoppingHrefValue()
        {
            return ContinueShoppingHref.GetAttribute("href");
        }
        public string GetEmptyCartHeadingText()
        {
            return webDriver.FindElement(By.CssSelector("h4.empty-cart-heading")).Text;
        }
        public string GetEmptyCartDetailsText()
        {
            return webDriver.FindElement(By.CssSelector("div.emtpy-cart-text > p")).Text;
        }
        public string GetEmptyCartHrefText()
        {
            return EmptyCartHref.Text;
        }
        public string GetEmptyCartHrefValue()
        {
            return EmptyCartHref.GetAttribute("href");
        }
        public string GetSubTotalText()
        {
            return webDriver.FindElement(By.CssSelector("div.sub-total > h5:nth-child(1)")).Text;
        }
        public string GetSubTotalValue()
        {
            return webDriver.FindElement(By.CssSelector("div.sub-total > h5:nth-child(2)")).Text;
        }
        public double GetSubTotalValueAsNumber()
        {
            var subTotal = 0D;

            try
            { subTotal = double.Parse(GetSubTotalValue().Substring(1).Trim()); }
            catch (Exception)
            { throw; }

            return subTotal;
        }
        public string GetTaxAndShippingText()
        {
            return webDriver.FindElement(By.ClassName("tax-and-shipping")).Text;
        }
        public string GetCheckoutButtonText()
        {
            return CheckoutButton.FindElement(By.ClassName("k-button-text")).Text;
        }
        public string GetItemQuantityText()
        {
            return EditQuantityInput.GetAttribute("aria-valuenow");
        }
        public string GetGridText()
        {
            return Grid.Text;
        }
    }
}