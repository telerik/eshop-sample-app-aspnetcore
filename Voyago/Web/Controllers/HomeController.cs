using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Web.Extensions;
using Models.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProductService productService;

        public HomeController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Other()
        {
            return Redirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult Clothing()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Bikes()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Accessories()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Components()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ProductCatalog()
        {
            ReportSourceModel report = new ReportSourceModel() { ReportId = "ProductCatalogNew.trdp" };

            return View("~/Views/Shared/ReportViewer.cshtml", report);
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        public JsonResult GetSearchSuggestions(string text)
        {
            var products = productService.GetSearchSuggestions(text).ToList();

            return Json(products);
        }

        [HttpPost]
        public async Task<IActionResult> GetTopSellingSubCategories([DataSourceRequest] DataSourceRequest request, int count)
        {
            var topSellingCategories = productService.GetTopSellingSubCategories(count);

            return Json(await topSellingCategories.ToDataSourceResultAsync(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetRecentlyViewedProducts([DataSourceRequest] DataSourceRequest request)
        {
            var recentlyViewedProductIds = HttpContext.Session.Get<Queue<int>>("_RecentlyViewed");
            if (recentlyViewedProductIds == default)
            {
                return Json(new { });
            }

            var recentlyViewedProducts = productService.GetListOfProducts(recentlyViewedProductIds);

            return Json(await recentlyViewedProducts.ToDataSourceResultAsync(request));
        }
    }
}