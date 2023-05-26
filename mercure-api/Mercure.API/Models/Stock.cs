using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercure.API.Models;

public class Stock
{
    /// <summary>
    /// Stock model
    /// </summary>
    /// <param name="stockQuantityAvailable"></param>
    public Stock(int stockQuantityAvailable)
    {
        StockQuantityAvailable = stockQuantityAvailable;
    }

    public Stock()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StockId { get; set; }
    public int StockQuantityAvailable { get; set; }
}