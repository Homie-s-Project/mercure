namespace Mercure.API.Models;

public class StockDto
{
    public StockDto(Stock stock)
    {
        Quantity = stock.StockQuantityAvailable;
    }

    public StockDto(StockDto productStock)
    {
        Quantity = productStock.Quantity;
    }

    public int Quantity {get; set;}
}