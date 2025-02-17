public class RepairManager
{
    public void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1. Dodaj zgłoszenie naprawy");
            Console.WriteLine("2. Wyświetl zgłoszenia napraw");
            Console.WriteLine("3. Zmień status zgłoszenia");
            Console.WriteLine("4. Usuń zgłoszenie naprawy");
            Console.WriteLine("0. Powrót");
            Console.Write("Wybierz opcję: ");

            switch (Console.ReadLine())
            {
                case "1":
                    CreateRepairRequest();
                    break;
                case "2":
                    DisplayRepairRequests();
                    break;
                case "3":
                    UpdateRepairRequestStatus();
                    break;
                case "4":
                    DeleteRepairRequest();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Niepoprawny wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    private void CreateRepairRequest()
    {
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

        if (!AppData.Instance.Users.Any(u => u.Id == userId))
        {
            Console.WriteLine("Użytkownik o podanym ID nie istnieje.");
            return;
        }

        Console.Write("Podaj urządzenie: ");
        string device = Console.ReadLine()!.Trim();

        Console.Write("Opis usterki: ");
        string description = Console.ReadLine()!.Trim();

        var repairRequest = new RepairRequest
        {
            Id = GenerateRepairRequestId(),
            UserId = userId,
            Device = device,
            Description = description,
            Status = "Oczekujące"
        };

        AppData.Instance.RepairRequests.Add(repairRequest);
        AppData.Instance.SaveAppData();
        Console.WriteLine("Zgłoszenie naprawy zostało dodane.");
    }

    private void DisplayRepairRequests()
    {
        var repairRequests = AppData.Instance.RepairRequests;
        var users = AppData.Instance.Users;

        if (repairRequests.Count == 0)
        {
            Console.WriteLine("Brak zgłoszeń napraw.");
            return;
        }

        foreach (var request in repairRequests)
        {
            var user = users.FirstOrDefault(u => u.Id == request.UserId);
            Console.WriteLine($"ID: {request.Id} | Użytkownik: {user?.Name ?? "Nieznany"} | " +
                              $"Urządzenie: {request.Device} | " +
                              $"Status: {request.Status} | " +
                              $"Data: {request.Date}");
            Console.WriteLine($"Opis: {request.Description}");
        }
    }

    private void DeleteRepairRequest()
    {
        DisplayRepairRequests();
        Console.Write("Podaj ID zgłoszenia do usunięcia: ");
        if (!int.TryParse(Console.ReadLine(), out int requestId))
        {
            Console.WriteLine("Nieprawidłowe ID zgłoszenia.");
            return;
        }

        var repairRequests = AppData.Instance.RepairRequests;
        var request = repairRequests.FirstOrDefault(r => r.Id == requestId);

        if (request == null)
        {
            Console.WriteLine("Zgłoszenie o podanym ID nie istnieje.");
            return;
        }

        repairRequests.Remove(request);
        AppData.Instance.SaveAppData();
        Console.WriteLine("Zgłoszenie naprawy zostało usunięte.");
    }

    private void UpdateRepairRequestStatus()
    {
        DisplayRepairRequests();
        Console.Write("Podaj ID zgłoszenia: ");
        if (!int.TryParse(Console.ReadLine(), out int requestId))
        {
            Console.WriteLine("Nieprawidłowe ID zgłoszenia.");
            return;
        }

        var repairRequests = AppData.Instance.RepairRequests;
        var request = repairRequests.FirstOrDefault(r => r.Id == requestId);

        if (request == null)
        {
            Console.WriteLine("Zgłoszenie o podanym ID nie istnieje.");
            return;
        }

        Console.WriteLine("Dostępne statusy:");
        Console.WriteLine("1. Oczekujące");
        Console.WriteLine("2. W trakcie naprawy");
        Console.WriteLine("3. Zakończone");
        Console.WriteLine("0. Powrót");
        Console.Write("Wybierz nowy status: ");

        switch (Console.ReadLine())
        {
            case "1":
                request.Status = "Oczekujące";
                break;
            case "2":
                request.Status = "W trakcie naprawy";
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
        Console.WriteLine("Status zgłoszenia został zaktualizowany.");
    }

    private int GenerateRepairRequestId()
    {
        var repairRequests = AppData.Instance.RepairRequests;
        return repairRequests.Count != 0 ? repairRequests.Max(r => r.Id) + 1 : 1;
    }
}
