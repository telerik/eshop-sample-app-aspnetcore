using Microsoft.EntityFrameworkCore;
using Services.ServiceExtensions;
using Services.Interfaces;
using System.Reflection;
using Models.ViewModels;
using Data;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly EShopDatabaseContext dbContext;

        public OrderService(EShopDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> AddSalesOrder(List<ShoppingCartItemViewModel> items, string userEmail)
        {
            var orederNumberLast = dbContext.SalesOrders.Max(x => x.OrderNumber) != null ? dbContext.SalesOrders.Max(x => x.OrderNumber) : 0;
            var total = items.Sum(items => items.Total);
            var userID = dbContext.Contacts.FirstOrDefault(u => u.EmailAddress == userEmail);
            if (userID != null)
            {
                foreach(ShoppingCartItemViewModel item in items)
                {
                    var orderForDb = new SalesOrder
                    {
                        ProductName = item.ProductName,
                        OrderNumber = orederNumberLast + 1,
                        ProductId = item.ProductId,
                        Total = total,
                        Status = 1,
                        ShipDate = DateTime.Now,
                        UnitPrice = item.ProductPrice,
                        LineTotal = item.Total,
                        Quantity = (short)item.Quantity,
                        ContactId = userID.ContactId,
                    };
                    await dbContext.SalesOrders.AddAsync(orderForDb);
                }
                return await dbContext.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }

        public IQueryable<OrderViewModel?> GetAllOrders(string userEmail)
        {
            var allUserOrders = (from order in dbContext.SalesOrders
                          join contact in dbContext.Contacts on order.ContactId equals contact.ContactId
                          where contact.EmailAddress == userEmail
                          select new OrderViewModel
                          {
                              OrderID = order.OrderId,
                              OrderDate = order.ShipDate,
                              Total = order.Total,
                              Status = order.Status,
                              OrderNumber = order.OrderNumber
                          });

            var uniqueOrders = allUserOrders.GroupBy(g => g.OrderNumber).Select(group => group.FirstOrDefault());

            return uniqueOrders;
        }

        public IQueryable<OrderDetailsViewModel?> GetOrderDetailsById(int orderNumber, string userEmail)
        {
            var orderDetails = (from order in dbContext.SalesOrders
                                where orderNumber == order.OrderNumber
                                join contact in dbContext.Contacts on order.ContactId equals contact.ContactId
                                where contact.EmailAddress == userEmail
                                select new OrderDetailsViewModel
                                {
                                    OrderID = order.OrderId,
                                    OrderNumber = orderNumber,
                                    OrderDate = order.ShipDate,
                                    Total = order.Total,
                                    Status = order.Status,
                                    FirstName = contact.FirstName,
                                    LastName = contact.LastName,
                                    Phone = contact.Phone,
                                    Street = contact.Street,
                                    State = contact.State,
                                    City = contact.City,
                                    Country = contact.Country,
                                    ProductID = order.ProductId,
                                    ProductName = order.ProductName,
                                    ProductPhoto = order.Product.ThumbNailPhoto,
                                    Quantity = order.Quantity,
                                    UnitPrice = order.UnitPrice,
                                    LineTotal = order.LineTotal
                                });

            return orderDetails;
        }
    }
}
