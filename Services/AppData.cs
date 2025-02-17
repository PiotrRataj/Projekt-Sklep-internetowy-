using System.Text.Json;

public sealed class AppData
{
    private static readonly Lazy<AppData> _instance = new(() => new AppData());
    public static AppData Instance => _instance.Value;

    private const string DataDirectory = "Data";
    private readonly string _filePath = Path.Combine(DataDirectory, "app.json");
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public List<Order> Orders { get; set; } = new();
    public List<User> Users { get; set; } = new();
    public List<Product> Products { get; set; } = new();
    public List<RepairRequest> RepairRequests { get; set; } = new();

    private AppData()
    {
        Directory.CreateDirectory(DataDirectory);
        LoadData();
    }

    private void LoadData()
    {
        if (!File.Exists(_filePath))
        {
            SaveAppData();
            return;
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            var jsonData = JsonDocument.Parse(json);
            var root = jsonData.RootElement;

            Orders = root.GetProperty("orders").Deserialize<List<Order>>(_jsonOptions) ?? [];
            Users = root.GetProperty("users").Deserialize<List<User>>(_jsonOptions) ?? [];
            Products = root.GetProperty("products").Deserialize<List<Product>>(_jsonOptions) ?? [];
            RepairRequests = root.GetProperty("repairRequests").Deserialize<List<RepairRequest>>(_jsonOptions) ?? [];
        }
        catch (Exception)
        {
            Console.WriteLine("Błąd odczytu danych.");
            SaveAppData();
        }
    }
    public void SaveAppData()
    {
        try
        {
            var json = JsonSerializer.Serialize(new
            {
                orders = Orders,
                users = Users,
                products = Products,
                repairRequests = RepairRequests
            }, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
        catch (IOException)
        {
            Console.WriteLine("Błąd zapisu danych.");
        }
    }
}