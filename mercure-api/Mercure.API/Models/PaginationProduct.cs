using System.Collections.Generic;

namespace Mercure.API.Models;

/// <summary>
/// Pagination for products
/// </summary>
public class PaginationProduct
{
    /// <summary>
    /// Total pages
    /// </summary>
    public int TotalPages { get; set; }
    /// <summary>
    /// Total products
    /// </summary>
    public int TotalProducts { get; set; }
    /// <summary>
    /// Current page
    /// </summary>
    public int CurrentPage { get; set; }
    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// Products
    /// </summary>
    public List<ProductDto> Products { get; set; }

    /// <summary>
    /// Pagination for products
    /// </summary>
    /// <param name="products"></param>
    /// <param name="currentPage"></param>
    /// <param name="pageSize"></param>
    /// <param name="totalPages"></param>
    public PaginationProduct(List<ProductDto> products, int currentPage, int pageSize, int totalPages, int totalProducts)
    {
        Products = products;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = totalPages;
        TotalProducts = totalProducts;
    }
}