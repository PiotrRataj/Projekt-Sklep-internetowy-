using System.Globalization;

public class RepairRequest : BaseEntity
{
    public required int UserId { get; set; }
    public required string Device { get; set; }
    public string Date { get; set; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
    public required string Description { get; set; }
    public required string Status { get; set; }
}