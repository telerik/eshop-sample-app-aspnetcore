using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Kendo.Mvc.Extensions;
using Services.Interfaces;
using Models.InputModels;
using Models.ViewModels;
using Web.Extensions;
using Kendo.Mvc.UI;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext == null || HttpContext.User == null || HttpContext.User.Identity == null)
            {
                throw new ArgumentNullException("HttpContext", "HttpContext is NULL");
            }

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserInpuModel input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await userService.GetUserByLoginCredentials(input);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim(ClaimTypes.Name, user.FirstName),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            { };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Redirect("/Home/Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext == null || HttpContext.User == null || HttpContext.User.Identity == null)
            {
                throw new ArgumentNullException("HttpContext", "HttpContext is NULL");
            }

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserInpuModel input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (await userService.UserExists(input.Email))
            {
                ModelState.AddModelError(string.Empty, "User already exists.");
                return View();
            }

            if (!await userService.CreateNewUser(input))
            {
                ModelState.AddModelError(string.Empty, "An error occured. Please try again");
                return View();
            }

            return Redirect("/Account/Login");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ShoppingCart()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ViewBag.ItemsCount = await userService.GetUserShoppingCartItemsCount(userEmail);

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProductToShoppingCart(int productId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (!await userService.AddShoppingCartItem(productId, userEmail))
            {
                return Redirect("/Home/Error");
            }

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserShoppingCartItems([DataSourceRequest] DataSourceRequest request)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var items = userService.GetUserShoppingCartItems(userEmail);

            return Json(await items.ToDataSourceResultAsync(request));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUserShoppingCartItem([DataSourceRequest] DataSourceRequest request, ShoppingCartItemViewModel item)
        {
            if (item != null && ModelState.IsValid)
            {
                await userService.ChangeShoppingCartItemQuantity(item.ProductId, item.ShoppingCartId, item.Quantity);
            }

            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoverUserShoppingCartItem([DataSourceRequest] DataSourceRequest request, ShoppingCartItemViewModel item)
        {
            if (item != null && ModelState.IsValid)
            {
                await userService.RemoveShoppingCartItem(item.ProductId, item.ShoppingCartId);
            }

            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetShoppingCartItemsCount()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var itemsCount = await userService.GetUserShoppingCartItemsCount(userEmail);

            return Json(itemsCount);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckoutShoppingCart()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            await userService.ClearUserShoppingCart(userEmail);

            return Redirect("/Account/ShoppingCart");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Favorites()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddProductToFavorites(int productId)
        {
            var key = "_Favorites";
            var value = HttpContext.Session.Get<List<int>>(key);

            if (value == default)
            {
                value = new List<int>();
                value.Add(productId);
            }
            else if (!value.Contains(productId))
            {
                value.Add(productId);
            }

            HttpContext.Session.Set<List<int>>(key, value);

            return Json(productId);
        }

        [HttpPost]
        [Authorize]
        public IActionResult RemoveProductFromFavorites(int productId)
        {
            var key = "_Favorites";
            var value = HttpContext.Session.Get<List<int>>(key);

            if (value != default)
            {
                value.Remove(productId);
                HttpContext.Session.Set<List<int>>(key, value);
            }

            return Json(productId);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFavoriteProducts([DataSourceRequest] DataSourceRequest request)
        {
            var favoriteProductIds = HttpContext.Session.Get<List<int>>("_Favorites");
            if (favoriteProductIds == default)
            {
                return Json(new { });
            }

            var favoriteProducts = userService.GetFavoriteProductsById(favoriteProductIds);

            return Json(await favoriteProducts.ToDataSourceResultAsync(request));
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetFavoritesCount()
        {
            var favoriteProductIds = HttpContext.Session.Get<List<int>>("_Favorites");

            if (favoriteProductIds == default)
            {
                return Json(0);
            }

            return Json(favoriteProductIds.Count);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ProductIsInFavorites(int productId)
        {
            var favoriteProductIds = HttpContext.Session.Get<List<int>>("_Favorites");
            if (favoriteProductIds == default)
            {
                return Json(false);
            }

            return Json(favoriteProductIds.Contains(productId));
        }
    }
}