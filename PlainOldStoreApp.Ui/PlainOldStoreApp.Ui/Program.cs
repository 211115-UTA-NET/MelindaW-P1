using PlainOldStoreApp.Ui;

namespace PlainOldStoreApp.Ui
{
    class Program
    {
        static async Task Main(String[] args)
        {
            Uri server = new Uri("https://localhost:7129");

            bool isRunning = true;
            Console.WriteLine("Welcome to Plain Old Store!");
            Console.WriteLine("What can I help you with today?");
            Console.WriteLine();

            while (isRunning)
            {
                Console.WriteLine("Please select one of the following menu options.");
                Console.WriteLine("1. Place Order\n2. Add Customer\n3. Lookup Order\n4. Exit");
                string? slection = Console.ReadLine()?.Trim().ToLower();
                Console.WriteLine();
                isRunning = await MainMenu(slection, isRunning, server);
            }
        }
        internal static async Task<bool> MainMenu(string? slection, bool isRunning, Uri server)
        {
            switch (slection)
            {
                case "1":
                case "place order":
                    await PlaneOldShop.PlaceOrder(server);
                    break;
                case "2":
                case "add customer":
                    await PlaneOldShop.AddCustomer(server);
                    break;
                case "3":
                case "lookup order":
                    //PlaneOldShop.LookupOrder();
                    break;
                case "4":
                case "exit":
                    Console.WriteLine("Have a nice day. Goodbye!");
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid input. Please select a valid menu option.");
                    break;
            }
            return isRunning;
        }
    }
}
