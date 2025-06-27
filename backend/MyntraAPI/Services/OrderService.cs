using Microsoft.EntityFrameworkCore;
using MyntraAPI.Data;
using MyntraAPI.Models;

namespace MyntraAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly MyntraDbContext _context;

        public OrderService(MyntraDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId, int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }

        public async Task<Order> CreateOrderAsync(int userId, string shippingAddress, string phoneNumber, string? notes)
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty");
            }

            decimal totalAmount = cartItems.Sum(ci => 
                (ci.Product.DiscountedPrice ?? ci.Product.Price) * ci.Quantity);

            var order = new Order
            {
                UserId = userId,
                OrderNumber = GenerateOrderNumber(),
                Status = "Pending",
                TotalAmount = totalAmount,
                ShippingAddress = shippingAddress,
                PhoneNumber = phoneNumber,
                Notes = notes,
                OrderDate = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderItems = cartItems.Select(ci => new OrderItem
            {
                OrderId = order.Id,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.DiscountedPrice ?? ci.Product.Price,
                TotalPrice = (ci.Product.DiscountedPrice ?? ci.Product.Price) * ci.Quantity
            }).ToList();

            _context.OrderItems.AddRange(orderItems);

            foreach (var cartItem in cartItems)
            {
                cartItem.Product.StockQuantity -= cartItem.Quantity;
            }

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            switch (status.ToLower())
            {
                case "shipped":
                    order.ShippedDate = DateTime.UtcNow;
                    break;
                case "delivered":
                    order.DeliveredDate = DateTime.UtcNow;
                    break;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
} 