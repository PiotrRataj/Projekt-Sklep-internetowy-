public static class Program
{
    public static void Main()
    {
        ProductManager productManager = new();
        UserManager userManager = new();
        OrderManager orderManager = new();
        RepairManager repairManager = new();
        Console.WriteLine("Witaj w sklepie internetowym!");
        while (true)
        {
            Console.WriteLine("1. Produkty");
            Console.WriteLine("2. Użytkownicy");
            Console.WriteLine("3. Zamówienia");
            Console.WriteLine("4. Naprawy");
            Console.WriteLine("0. Wyjście");
            string choice = Console.ReadLine()!.ToString();
            switch (choice)
            {
                case "1":
                    productManager.Main();
                    break;
                case "2":
                    userManager.Main();
                    break;
                case "3":
                    orderManager.Main();
                    break;
                case "4":
                    repairManager.Main();
                    break;
                case "0":
                    return;
            }
        }
    }
}