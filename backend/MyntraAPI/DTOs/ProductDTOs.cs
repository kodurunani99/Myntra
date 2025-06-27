using System.ComponentModel.DataAnnotations;

namespace MyntraAPI.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateProductRequest
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? DiscountedPrice { get; set; }

        [StringLength(200)]
        public string Brand { get; set; } = string.Empty;

        [StringLength(50)]
        public string Size { get; set; } = string.Empty;

        [StringLength(50)]
        public string Color { get; set; } = string.Empty;

        [StringLength(200)]
        public string ImageUrl { get; set; } = string.Empty;

        public int StockQuantity { get; set; } = 0;

        [Required]
        public int CategoryId { get; set; }
    }

    public class UpdateProductRequest
    {
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? DiscountedPrice { get; set; }

        [StringLength(200)]
        public string? Brand { get; set; }

        [StringLength(50)]
        public string? Size { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        public int? StockQuantity { get; set; }

        public int? CategoryId { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ProductFilterRequest
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Brand { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public bool? InStock { get; set; }
        public string? SortBy { get; set; } // "name", "price", "createdAt"
        public string? SortOrder { get; set; } // "asc", "desc"
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
} 