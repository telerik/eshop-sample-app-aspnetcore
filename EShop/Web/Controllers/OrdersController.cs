using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Newtonsoft.Json;
using Services.Interfaces;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Orders_Read([DataSourceRequest] DataSourceRequest request)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await orderService.GetAllOrders(userEmail).ToList().ToDataSourceResultAsync(request);
            return Json(result);
        }

        [HttpGet]
        public IActionResult Details(int orderNumber)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orderDetails = orderService.GetOrderDetailsById(orderNumber, userEmail).FirstOrDefault();

            if (orderDetails == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("Details", orderDetails);
        }

        [HttpPost]
        public async Task<IActionResult> OrderDetails_Read([DataSourceRequest] DataSourceRequest request, int orderNumber)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orderDetails = await orderService.GetOrderDetailsById(orderNumber, userEmail).ToDataSourceResultAsync(request);
            return Json(orderDetails);
        }

        [HttpGet]
        public IActionResult Invoice(int orderNumber)
        {
            ReportSourceModel report = new ReportSourceModel()
            {
                ReportId = "OrderInvoiceNew.trdp",
                Parameters = new Dictionary<string, object>()
            };
            report.Parameters.Add("OrN", orderNumber);
            return View("~/Views/Shared/ReportViewer.cshtml", report);
        }
    }
}
