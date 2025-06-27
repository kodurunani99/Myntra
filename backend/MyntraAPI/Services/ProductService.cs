using Microsoft.EntityFrameworkCore;
using MyntraAPI.Data;
using MyntraAPI.DTOs;
using MyntraAPI.Models;

namespace MyntraAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly MyntraDbContext _context;

        public ProductService(MyntraDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(ProductFilterRequest filter)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            // Apply filters
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(p => p.Name.Contains(filter.SearchTerm) || 
                                        p.Description.Contains(filter.SearchTerm) ||
                                        p.Brand.Contains(filter.SearchTerm));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(filter.Brand))
            {
                query = query.Where(p => p.Brand.Contains(filter.Brand));
            }

            if (!string.IsNullOrEmpty(filter.Size))
            {
                query = query.Where(p => p.Size.Contains(filter.Size));
            }

            if (!string.IsNullOrEmpty(filter.Color))
            {
                query = query.Where(p => p.Color.Contains(filter.Color));
            }

            if (filter.InStock.HasValue && filter.InStock.Value)
            {
                query = query.Where(p => p.StockQuantity > 0);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "name" => filter.SortOrder?.ToLower() == "desc" 
                        ? query.OrderByDescending(p => p.Name)
                        : query.OrderBy(p => p.Name),
                    "price" => filter.SortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price),
                    "createdat" => filter.SortOrder?.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.CreatedAt)
                        : query.OrderBy(p => p.CreatedAt),
                    _ => query.OrderBy(p => p.Name)
                };
            }

            // Apply pagination
            var products = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountedPrice = p.DiscountedPrice,
                    Brand = p.Brand,
                    Size = p.Size,
                    Color = p.Color,
                    ImageUrl = p.ImageUrl,
                    StockQuantity = p.StockQuantity,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            return products;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                Brand = product.Brand,
                Size = product.Size,
                Color = product.Color,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductRequest request)
        {
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                throw new ArgumentException("Invalid category ID");
            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                DiscountedPrice = request.DiscountedPrice,
                Brand = request.Brand,
                Size = request.Size,
                Color = request.Color,
                ImageUrl = request.ImageUrl,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId,
                IsActive = true
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                Brand = product.Brand,
                Size = product.Size,
                Color = product.Color,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = category.Name,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            if (request.Name != null) product.Name = request.Name;
            if (request.Description != null) product.Description = request.Description;
            if (request.Price.HasValue) product.Price = request.Price.Value;
            if (request.DiscountedPrice.HasValue) product.DiscountedPrice = request.DiscountedPrice.Value;
            if (request.Brand != null) product.Brand = request.Brand;
            if (request.Size != null) product.Size = request.Size;
            if (request.Color != null) product.Color = request.Color;
            if (request.ImageUrl != null) product.ImageUrl = request.ImageUrl;
            if (request.StockQuantity.HasValue) product.StockQuantity = request.StockQuantity.Value;
            if (request.CategoryId.HasValue) product.CategoryId = request.CategoryId.Value;
            if (request.IsActive.HasValue) product.IsActive = request.IsActive.Value;

            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
} 