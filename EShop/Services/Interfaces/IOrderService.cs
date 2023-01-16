using Models.ViewModels;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task<bool> AddSalesOrder(List<ShoppingCartItemViewModel> items, string userEmail);
        IQueryable<OrderViewModel?> GetAllOrders(string userEmail);
        IQueryable<OrderDetailsViewModel?> GetOrderDetailsById(int orderNumber, string userEmail);
    }
}
