using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercure.API.Models;

public class Stock
{
    public Stock(int stockQuantityAvailable)
    {
        StockQuantityAvailable = stockQuantityAvailable;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StockId { get; set; }
    public int StockQuantityAvailable { get; set; }
}