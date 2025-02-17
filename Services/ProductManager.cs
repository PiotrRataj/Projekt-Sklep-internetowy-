public class ProductManager
{
    public void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1. Dodaj produkt");
            Console.WriteLine("2. Wyświetl produkty");
            Console.WriteLine("3. Usuń produkt");
            Console.WriteLine("0. Powrót");
            Console.Write("Wybierz opcję: ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddProduct();
                    break;
                case "2":
                    DisplayProducts();
                    break;
                case "3":
                    DeleteProduct();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Niepoprawny wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    private void AddProduct()
    {
        Console.Write("Nazwa: ");
        string name = Console.ReadLine()!.Trim();
        Console.Write("Opis: ");
        string description = Console.ReadLine()!.Trim();

        Console.Write("Cena: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
        {
            Console.WriteLine("Niepoprawna cena!");
            return;
        }

        var products = AppData.Instance.Products;
        int newId = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;

        products.Add(new Product { Id = newId, Name = name, Description = description, Price = (int)price });

        AppData.Instance.SaveAppData();

        Console.WriteLine("Produkt dodany!");
    }

    private void DisplayProducts()
    {
        var products = AppData.Instance.Products;
        if (products.Count == 0)
        {
            Console.WriteLine("Brak produktów.");
            return;
        }

        foreach (var p in products)
        {
            Console.WriteLine($"Id: {p.Id} | {p.Name} - {p.Price} zł");
        }
    }

    private void DeleteProduct()
    {
        var products = AppData.Instance.Products;
        if (products.Count == 0)
        {
            Console.WriteLine("Brak dostępnych produktów do usunięcia.");
            return;
        }

        // Wyświetl dostępne produkty
        Console.WriteLine("Dostępne produkty:");
        foreach (var p in products)
        {
            Console.WriteLine($"ID: {p.Id} | {p.Name} - {p.Price} zł");
        }

        Console.Write("Podaj ID produktu do usunięcia: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Niepoprawne ID!");
            return;
        }

        var productToDelete = products.FirstOrDefault(p => p.Id == id);
        if (productToDelete == null)
        {
            Console.WriteLine("Produkt o podanym ID nie istnieje!");
            return;
        }

        products.Remove(productToDelete);
        AppData.Instance.SaveAppData();

        Console.WriteLine("Produkt został usunięty.");
    }
}
