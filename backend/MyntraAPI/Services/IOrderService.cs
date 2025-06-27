using MyntraAPI.Models;

namespace MyntraAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<Order?> GetOrderByIdAsync(int orderId, int userId);
        Task<Order> CreateOrderAsync(int userId, string shippingAddress, string phoneNumber, string? notes);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }
} 