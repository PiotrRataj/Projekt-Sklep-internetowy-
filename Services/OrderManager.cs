using System.Globalization;

public class OrderManager
{
    public void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1. Dodaj zamówienie");
            Console.WriteLine("2. Wyświetl wszystkie zamówienia");
            Console.WriteLine("3. Zmień status zamówienia");
            Console.WriteLine("4. Usuń zamówienie");
            Console.WriteLine("0. Powrót");
            Console.Write("Wybierz opcję: ");

            switch (Console.ReadLine())
            {
                case "1":
                    CreateNewOrder();
                    break;
                case "2":
                    DisplayAllOrders();
                    break;
                case "3":
                    UpdateOrdertStatus();
                    break;
                case "4":
                    DeleteOrder();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Niepoprawny wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }
    private void UpdateOrdertStatus()
    {
        DisplayAllOrders();
        Console.Write("Podaj ID zamówienia: ");
        if (!int.TryParse(Console.ReadLine(), out int requestId))
        {
            Console.WriteLine("Nieprawidłowe ID zamówienia.");
            return;
        }

        var orders = AppData.Instance.Orders;
        var request = orders.FirstOrDefault(r => r.Id == requestId);

        if (request == null)
        {
            Console.WriteLine("Zamówienie o podanym ID nie istnieje.");
            return;
        }

        Console.WriteLine("Dostępne statusy:");
        Console.WriteLine("1. Oczekujące");
        Console.WriteLine("2. W dostawie");
        Console.WriteLine("3. Zakończone");
        Console.WriteLine("0. Powrót");
        Console.Write("Wybierz nowy status: ");

        switch (Console.ReadLine())
        {
            case "1":
                request.Status = "Oczekujące";
                break;
            case "2":
                request.Status = "W dostawie";
                break;
            case "3":
                request.Status = "Zakończone";
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Nieprawidłowy wybór.");
                return;
        }

        AppData.Instance.SaveAppData();
        Console.WriteLine("Status zamówienia został zaktualizowany.");
    }
    private void CreateNewOrder()
    {
        // Wyświetl dostępnych użytkowników
        var users = AppData.Instance.Users;
        if (users.Count == 0)
        {
            Console.WriteLine("Brak dostępnych użytkowników.");
            return;
        }

        Console.WriteLine("Dostępni użytkownicy:");
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id} | {user.Name}");
        }

        Console.Write("Podaj ID użytkownika: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Nieprawidłowe ID użytkownika.");
            return;
        }

        if (!users.Any(u => u.Id == userId))
        {
            Console.WriteLine("Użytkownik o podanym ID nie istnieje.");
            return;
        }

        var products = AppData.Instance.Products;

        var order = new Order
        {
            Id = GenerateOrderId(),
            UserId = userId,
            Products = new List<OrderProduct>(),
            Status = "Pending",
            Date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
        };

        while (true)
        {
            // Wyświetl aktualny koszyk
            if (order.Products.Count != 0)
            {
                Console.WriteLine("\n--- Aktualny koszyk ---");
                decimal totalPrice = 0;
                foreach (var item in order.Products)
                {
                    var orderProduct = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (orderProduct != null)
                    {
                        decimal itemTotal = orderProduct.Price * item.Quantity;
                        Console.WriteLine($"Produkt: {orderProduct.Name} | Ilość: {item.Quantity} | Cena: {itemTotal} zł");
                        totalPrice += itemTotal;
                    }
                }
                Console.WriteLine($"Suma całkowita: {totalPrice} zł\n");
            }

            Console.WriteLine("Dostępne produkty:");
            foreach (var availableProduct in products)
            {
                Console.WriteLine($"ID: {availableProduct.Id} | {availableProduct.Name} - {availableProduct.Price} zł");
            }

            Console.Write("\nPodaj ID produktu (lub 0 aby zakończyć dodawanie): ");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Nieprawidłowe ID produktu.");
                continue;
            }

            if (productId == 0) break;

            var selectedProduct = products.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct == null)
            {
                Console.WriteLine("Produkt o podanym ID nie istnieje.");
                continue;
            }

            Console.Write("Podaj ilość: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Nieprawidłowa ilość.");
                continue;
            }

            var existingProduct = order.Products.FirstOrDefault(p => p.ProductId == productId);
            if (existingProduct != null)
            {
                Console.Write("Produkt już istnieje w zamówieniu. Zaktualizować ilość? (T/N): ");
                if (Console.ReadLine()?.Trim().ToUpper() == "T")
                {
                    existingProduct.Quantity = quantity;
                    Console.WriteLine("Ilość produktu zaktualizowana.");
                }
            }
            else
            {
                order.Products.Add(new OrderProduct { ProductId = productId, Quantity = quantity });
                Console.WriteLine("Produkt dodany do zamówienia.");
            }
        }

        if (order.Products.Count == 0)
        {
            Console.WriteLine("Zamówienie nie zawiera produktów. Anulowano.");
            return;
        }

        AppData.Instance.Orders.Add(order);
        AppData.Instance.SaveAppData();
        Console.WriteLine("Zamówienie zostało dodane.");
    }

    private void DisplayAllOrders()
    {
        var orders = AppData.Instance.Orders;
        var users = AppData.Instance.Users;
        var products = AppData.Instance.Products;

        if (orders.Count == 0)
        {
            Console.WriteLine("Brak zamówień.");
            return;
        }

        foreach (var order in orders)
        {
            var user = users.FirstOrDefault(u => u.Id == order.UserId);
            Console.WriteLine($"ID: {order.Id}, Użytkownik: {user?.Name ?? "Nieznany"}, Data: {order.Date}, Status: {order.Status}");
            foreach (var product in order.Products)
            {
                var productName = products.FirstOrDefault(p => p.Id == product.ProductId)?.Name ?? "Nieznany";
                Console.WriteLine($"  - Produkt: {productName} (ID: {product.ProductId}), Ilość: {product.Quantity}");
            }
        }
    }
    private void DeleteOrder()
    {
        var orders = AppData.Instance.Orders;
        if (orders.Count == 0)
        {
            Console.WriteLine("Brak zamówień do usunięcia.");
            return;
        }

        // Wyświetl dostępne zamówienia
        Console.WriteLine("Dostępne zamówienia:");
        foreach (var order in orders)
        {
            var user = AppData.Instance.Users.FirstOrDefault(u => u.Id == order.UserId);
            Console.WriteLine($"ID: {order.Id}, Użytkownik: {user?.Name ?? "Nieznany"}, Data: {order.Date}, Status: {order.Status}");
        }

        Console.Write("Podaj ID zamówienia do usunięcia: ");
        if (!int.TryParse(Console.ReadLine(), out int orderId))
        {
            Console.WriteLine("Nieprawidłowe ID zamówienia.");
            return;
        }

        var orderToDelete = orders.FirstOrDefault(o => o.Id == orderId);

        if (orderToDelete == null)
        {
            Console.WriteLine("Zamówienie o podanym ID nie istnieje.");
            return;
        }

        orders.Remove(orderToDelete);
        AppData.Instance.SaveAppData();
        Console.WriteLine("Zamówienie zostało usunięte.");
    }


    private int GenerateOrderId()
    {
        var orders = AppData.Instance.Orders;
        return orders.Count != 0 ? orders.Max(o => o.Id) + 1 : 1;
    }
}