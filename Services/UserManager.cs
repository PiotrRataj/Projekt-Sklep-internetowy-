public class UserManager
{
    public void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1. Dodaj użytkownika");
            Console.WriteLine("2. Wyświetl użytkowników");
            Console.WriteLine("3. Usuń użytkownika");
            Console.WriteLine("0. Powrót");
            Console.Write("Wybierz opcję: ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddUser();
                    break;
                case "2":
                    DisplayUsers();
                    break;
                case "3":
                    DeleteUser();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Niepoprawny wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    private void AddUser()
    {
        Console.Write("Nazwa: ");
        string name = Console.ReadLine()!.Trim();
        Console.Write("Email: ");
        string email = Console.ReadLine()!.Trim();
        Console.Write("Hasło: ");
        string password = Console.ReadLine()!.Trim();
        Console.Write("Adres: ");
        string address = Console.ReadLine()!.Trim();

        var users = AppData.Instance.Users;
        int newId = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;

        users.Add(new User { Id = newId, Name = name, Email = email, Password = password, Address = address });

        AppData.Instance.SaveAppData();
        Console.WriteLine("Użytkownik dodany!");
    }

    private void DisplayUsers()
    {
        var users = AppData.Instance.Users;
        if (users.Count == 0)
        {
            Console.WriteLine("Brak użytkowników.");
            return;
        }

        foreach (var u in users)
        {
            Console.WriteLine($"Id: {u.Id} | {u.Name} ({u.Email}) | Adres: {u.Address}");
        }
    }

    private void DeleteUser()
    {
        Console.Write("Podaj Id użytkownika do usunięcia: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Niepoprawne Id!");
            return;
        }

        var users = AppData.Instance.Users;
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            Console.WriteLine("Użytkownik nie istnieje!");
            return;
        }

        users.Remove(user);
        AppData.Instance.SaveAppData();

        Console.WriteLine("Użytkownik usunięty.");
    }
}
