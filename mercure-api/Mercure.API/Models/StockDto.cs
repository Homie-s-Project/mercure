namespace Mercure.API.Models;

public class StockDto
{
    public StockDto(Stock stock)
    {
        Quantity = stock.StockQuantityAvailable;
    }
    
    public int Quantity {get; set;}
}