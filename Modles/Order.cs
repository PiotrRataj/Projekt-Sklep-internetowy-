using System.Globalization;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<OrderProduct> Products { get; set; } = [];
    public string Status { get; set; } = "Oczekujące";
    public string Date { get; set; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
}

public class OrderProduct
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
