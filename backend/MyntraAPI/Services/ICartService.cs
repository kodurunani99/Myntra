using MyntraAPI.Models;

namespace MyntraAPI.Services
{
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetUserCartAsync(int userId);
        Task<CartItem> AddToCartAsync(int userId, int productId, int quantity);
        Task<bool> UpdateCartItemAsync(int userId, int productId, int quantity);
        Task<bool> RemoveFromCartAsync(int userId, int productId);
        Task<bool> ClearCartAsync(int userId);
    }
} 