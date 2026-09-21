using SkyFlow.Models;
using SkyFlow.Data;

class Program
{
    static string connectionString = "Data Source=SkyFlow.db";

    static void Main()
    {
        IDataRepository repo = new SqliteDataRepository(connectionString);
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== SkyFlow Terminal Manager ===");
            Console.Write("Username: ");
            string user = Console.ReadLine();
            Console.Write("Password: ");
            string pass = Console.ReadLine();
            var loggedIn = repo.AuthenticateUser(user, pass);

            if (loggedIn == null)
            {
                Console.WriteLine("Invalid credentials. Press any key...");
                Console.ReadKey();
                continue;
            }
            Console.WriteLine($"Welcome, {loggedIn.Username} ({loggedIn.Role})");
            Console.WriteLine("Press any key...");
            Console.ReadKey();
            loggedIn.DisplayDashboard(repo);
            Console.WriteLine("Logged out. Press any key...");
            Console.ReadKey();
        }
    }
}