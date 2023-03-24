using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercure.API.Models;

public class Product
{
    public Product(string productBrandName, string productName, string productDescription, int productPrice, DateTime productCreationDate, DateTime productLastUpdate, int stockId)
    {
        ProductBrandName = productBrandName;
        ProductName = productName;
        ProductDescription = productDescription;
        ProductPrice = productPrice;
        ProductCreationDate = productCreationDate;
        ProductLastUpdate = productLastUpdate;
        StockId = stockId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }
    public string ProductBrandName { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public int ProductPrice { get; set; }
    public DateTime ProductCreationDate { get; set; }
    public DateTime ProductLastUpdate { get; set; }
    
    [ForeignKey("Stock")] public int StockId { get; set; }
    public virtual Stock Stock { get; set; }
    
    public virtual ICollection<Category> Categoriess { get; set; }
    
}