using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercure.API.Models;

/// <summary>
/// Category of product
/// </summary>
public class Category
{
    public Category(string categoryTitle, string categoryDescription)
    {
        CategoryTitle = categoryTitle;
        CategoryDescription = categoryDescription;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryId { get; set; }
    public string CategoryTitle { get; set; }
    public string CategoryDescription { get; set; }
    
    public virtual ICollection<Product> Products { get; set; }
}