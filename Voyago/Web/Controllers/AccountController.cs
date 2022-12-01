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
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        public static UserProfileViewModel userProfile;
        public static List<StateViewModel> states = new List<StateViewModel> {
          new StateViewModel("AL", "Alabama"),
          new StateViewModel("AK", "Alaska"),
          new StateViewModel("AZ", "Arizona"),
          new StateViewModel("AR", "Arkansas"),
          new StateViewModel("CA", "California"),
          new StateViewModel("CO", "Colorado"),
          new StateViewModel("CT", "Connecticut"),
          new StateViewModel("DE", "Delaware"),
          new StateViewModel("DC", "District Of Columbia"),
          new StateViewModel("FL", "Florida"),
          new StateViewModel("GA", "Georgia"),
          new StateViewModel("HI", "Hawaii"),
          new StateViewModel("ID", "Idaho"),
          new StateViewModel("IL", "Illinois"),
          new StateViewModel("IN", "Indiana"),
          new StateViewModel("IA", "Iowa"),
          new StateViewModel("KS", "Kansas"),
          new StateViewModel("KY", "Kentucky"),
          new StateViewModel("LA", "Louisiana"),
          new StateViewModel("ME", "Maine"),
          new StateViewModel("MD", "Maryland"),
          new StateViewModel("MA", "Massachusetts"),
          new StateViewModel("MI", "Michigan"),
          new StateViewModel("MN", "Minnesota"),
          new StateViewModel("MS", "Mississippi"),
          new StateViewModel("MO", "Missouri"),
          new StateViewModel("MT", "Montana"),
          new StateViewModel("NE", "Nebraska"),
          new StateViewModel("NV", "Nevada"),
          new StateViewModel("NH", "New Hampshire"),
          new StateViewModel("NJ", "New Jersey"),
          new StateViewModel("NM", "New Mexico"),
          new StateViewModel("NY", "New York"),
          new StateViewModel("NC", "North Carolina"),
          new StateViewModel("ND", "North Dakota"),
          new StateViewModel("OH", "Ohio"),
          new StateViewModel("OK", "Oklahoma"),
          new StateViewModel("OR", "Oregon"),
          new StateViewModel("PA", "Pennsylvania"),
          new StateViewModel("RI", "Rhode Island"),
          new StateViewModel("SC", "South Carolina"),
          new StateViewModel("SD", "South Dakota"),
          new StateViewModel("TN", "Tennessee"),
          new StateViewModel("TX", "Texas"),
          new StateViewModel("UT", "Utah"),
          new StateViewModel("VT", "Vermont"),
          new StateViewModel("VA", "Virginia"),
          new StateViewModel("WA", "Washington"),
          new StateViewModel("WV", "West Virginia"),
          new StateViewModel("WI", "Wisconsin"),
          new StateViewModel("WY", "Wyoming")
        };

        public AccountController(IUserService userService, IOrderService orderService, IProductService productService)
        {
            this.userService = userService;
            this.orderService = orderService;
            this.productService = productService;
            if (userProfile == null)
            {
                userProfile = new UserProfileViewModel();
            }

            this.productService = productService;
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
                return View(new LoginUserInpuModel() { Email = "jaxons.danniels@company.com",  Password = "User1234" });
            }
            else
            {
                return RedirectToAction("Index", "Home");
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
                ModelState.AddModelError("Email", "Account does not exists.");
                return View(new LoginUserInpuModel() { Email = "jaxons.danniels@company.com", Password = "User1234" });
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

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            var valueFavs = new List<int>();
            var valueRecently = new Queue<int>(4);
            HttpContext.Session.Set<List<int>>("_Favorites", valueFavs);
            HttpContext.Session.Set<Queue<int>>("_RecentlyViewed", valueRecently);
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
                return RedirectToAction("Index", "Home");
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
                ModelState.AddModelError("Email", "User already exists.");
                return View();
            }

            if (!await userService.CreateNewUser(input))
            {
                ModelState.AddModelError(string.Empty, "An error occured. Please try again");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ShoppingCart()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ViewBag.ItemsCount = await userService.GetUserShoppingCartItemsCount(userEmail);

            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var userDetails = await userService.GetUserPersonalDetails(userEmail);

                if (userDetails != null)
                {
                    userProfile = userDetails;
                }
                return View(userProfile);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveUserPersonalDetails(ProfileUserInputModel personalDetails)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (personalDetails != null && ModelState.IsValid)
            {
                if (!await userService.EditUserDetails(personalDetails, userEmail))
                {
                    return RedirectToAction("Error", "Home");
                }

                userProfile.FirstName = personalDetails.FirstName;
                userProfile.LastName = personalDetails.LastName;
                userProfile.Phone = personalDetails.Phone;
            }

            return View("Profile", userProfile);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveUserPassword(PasswordUserInputModel input)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (input != null)
            {
                if (!await userService.EditUserPassword(input, userEmail))
                {
                    return RedirectToAction("Error", "Home");
                }
                userProfile.Password = input.Password;
                userProfile.ConfimPassword = input.ConfimPassword;
            }

            return View("Profile", userProfile);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveUserShippingDetails(AddressUserInputModel input)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (input != null)
            {
                if (!await userService.EditUserAddress(input, userEmail))
                {
                    return RedirectToAction("Error", "Home");
                }
                userProfile.Street = input.Street;
                userProfile.State = input.State;
                userProfile.Country = input.Country;
                userProfile.City = input.City;
                userProfile.Zipcode = input.Zipcode;
            }

            return View("Profile", userProfile);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProductToShoppingCart(int productId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (!await userService.AddShoppingCartItem(productId, userEmail))
            {
                return RedirectToAction("Error", "Home");
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
            var shoppingCartItems = userService.GetUserShoppingCartItems(userEmail).ToList();

            if(!await orderService.AddSalesOrder(shoppingCartItems, userEmail))
            {
                return RedirectToAction("Error", "Home");
            }

            if(!await userService.ClearUserShoppingCart(userEmail))
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index", "Orders");
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

        [HttpGet]
        public JsonResult GetStates()
        {
            return Json(states);
        }

        [HttpGet]
        public async Task<ActionResult> FavouritesReport()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userDetails = await userService.GetUserPersonalDetails(userEmail);
            var userName = userDetails.FirstName + " " + userDetails.LastName;

            var reportData = new FavoriteUserProductViewModel()
            {
                UserName = userName,
                FavoriteReportProducts = new List<FavoriteReportProductViewModel>()
            };

            var favoriteProductIds = HttpContext.Session.Get<List<int>>("_Favorites");
            if (favoriteProductIds == default)
            {
                return NotFound();
            }
            var favoriteProducts = userService.GetFavoriteProductsById(favoriteProductIds);

            foreach (var favoriteProduct in favoriteProducts)
            {
                byte[]? largePhotoData = await productService.GetProductLargePhotoById((int)favoriteProduct.PhotoId);
                reportData.FavoriteReportProducts.Add(new FavoriteReportProductViewModel
                {
                    Id = favoriteProduct.Id,
                    ProductName = favoriteProduct.Name,
                    Price =(decimal)favoriteProduct.FinalPrice,
                    LargePhoto = largePhotoData
                });
            }

            var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
            var reportSource = new Telerik.Reporting.UriReportSource();
            reportSource.Uri = "./Reports/FavouritesNew.trdp";
            string parameterValue = Newtonsoft.Json.JsonConvert.SerializeObject(reportData);
            reportSource.Parameters.Add("JSONData", parameterValue);
            Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", reportSource, null);
            if (!result.HasErrors)
            {
                return File(result.DocumentBytes, "application/pdf", "favourites.pdf");
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
    }
}