using System.Collections.Generic;
using System.Linq;

namespace Mercure.API.Models;

public class CategoryDto
{
    public CategoryDto(Category category)
    {
        CategoryTitle = category.CategoryTitle;
        CategoryDescription = category.CategoryDescription;
    }
    
    public CategoryDto(Category category, bool loadProducts)
    {
        CategoryTitle = category.CategoryTitle;
        CategoryDescription = category.CategoryDescription;

        if (loadProducts)
        {
            if (category.Products != null)
            {
                Products = category.Products.Select(p => new ProductDto(p, false)).ToList();
            }
        }
    }

    public string CategoryTitle { get; set; }
    public string CategoryDescription { get; set; }
    
    public List<ProductDto> Products { get; set; }
}