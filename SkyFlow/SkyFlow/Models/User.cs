using SkyFlow.Data;
using SkyFlow.Utilities;
using System;

namespace SkyFlow.Models
{
    public abstract class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public abstract void DisplayDashboard(IDataRepository repo);
    }

    public class Admin : User
    {
        public override void DisplayDashboard(IDataRepository repo)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Admin Dashboard ===");
                Console.WriteLine("1. Manage Flights (Add New Flight)");
                Console.WriteLine("2. System Oversight (View All Flights & Occupancy)");
                Console.WriteLine("3. Manage Staff (Add Gate Agent)");
                Console.WriteLine("0. Logout");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AddFlight(repo); break;
                    case "2": ViewOverview(repo); break;
                    case "3": AddStaff(repo); break;
                    case "0": exit = true; break;
                    default: Console.WriteLine("Invalid"); Console.ReadKey(); break;
                }
            }
        }

        private void AddFlight(IDataRepository repo)
        {
            Console.Clear();
            try
            {
                Console.Write("Flight Number: "); string num = Console.ReadLine();
                Console.Write("Origin: "); string origin = Console.ReadLine();
                Console.Write("Destination: "); string dest = Console.ReadLine();
                Console.Write("Departure (yyyy-mm-dd hh:mm): "); DateTime dt = DateTime.Parse(Console.ReadLine());
                Console.Write("Capacity: "); int cap = int.Parse(Console.ReadLine());
                repo.AddFlight(new Flight { FlightNumber = num, Origin = origin, Destination = dest, DepartureTime = dt, Capacity = cap, FlightStatus = "Scheduled" });
                Console.WriteLine("Flight added.");
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
            Console.ReadKey();
        }

        private void ViewOverview(IDataRepository repo)
        {
            Console.Clear();
            TablePrinter.PrintFlightOverview(repo.GetAllFlightsWithOccupancy());
            Console.ReadKey();
        }

        private void AddStaff(IDataRepository repo)
        {
            Console.Clear();
            Console.Write("Username: "); string u = Console.ReadLine();
            Console.Write("Password: "); string p = Console.ReadLine();
            repo.AddUser(u, p, "GateAgent");
            Console.WriteLine("Gate Agent added.");
            Console.ReadKey();
        }
    }

    public class GateAgent : User
    {
        public override void DisplayDashboard(IDataRepository repo)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Gate Agent Dashboard ===");
                Console.WriteLine("1. Flight Manifest");
                Console.WriteLine("2. Passenger Check-in");
                Console.WriteLine("3. Boarding Gate (Depart Flight)");
                Console.WriteLine("0. Logout");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ShowManifest(repo); break;
                    case "2": CheckIn(repo); break;
                    case "3": DepartFlight(repo); break;
                    case "0": exit = true; break;
                    default: Console.WriteLine("Invalid"); Console.ReadKey(); break;
                }
            }
        }

        private void ShowManifest(IDataRepository repo)
        {
            Console.Clear();
            var flights = repo.GetAllFlights();
            if (flights.Count == 0) { Console.WriteLine("No flights."); Console.ReadKey(); return; }
            TablePrinter.PrintFlightList(flights);
            Console.Write("Flight ID: ");
            if (int.TryParse(Console.ReadLine(), out int fid))
                TablePrinter.PrintPassengerManifest(repo.GetPassengersByFlight(fid));
            else Console.WriteLine("Invalid ID.");
            Console.ReadKey();
        }

        private void CheckIn(IDataRepository repo)
        {
            Console.Clear();
            var flights = repo.GetAllFlights();
            if (flights.Count == 0) { Console.WriteLine("No flights."); Console.ReadKey(); return; }
            TablePrinter.PrintFlightList(flights);
            Console.Write("Flight ID: ");
            if (!int.TryParse(Console.ReadLine(), out int fid)) { Console.WriteLine("Invalid"); Console.ReadKey(); return; }
            var flight = repo.GetFlightById(fid);
            if (flight.FlightStatus != "Scheduled") { Console.WriteLine("Flight not scheduled."); Console.ReadKey(); return; }
            Console.Write("Passenger ID or Passport: ");
            string id = Console.ReadLine();
            var booking = repo.GetBookingByFlightAndPassengerIdentifier(fid, id);
            if (booking == null) { Console.WriteLine("Not found on this flight."); Console.ReadKey(); return; }
            if (booking.Status != "Booked") { Console.WriteLine($"Already {booking.Status}."); Console.ReadKey(); return; }
            repo.UpdateBookingStatus(booking.BookingId, "CheckedIn");
            Console.WriteLine($"{booking.PassengerName} checked in.");
            Console.ReadKey();
        }

        private void DepartFlight(IDataRepository repo)
        {
            Console.Clear();
            var flights = repo.GetAllFlights();
            if (flights.Count == 0) { Console.WriteLine("No flights."); Console.ReadKey(); return; }
            TablePrinter.PrintFlightList(flights);
            Console.Write("Flight ID to depart: ");
            if (!int.TryParse(Console.ReadLine(), out int fid)) { Console.WriteLine("Invalid"); Console.ReadKey(); return; }
            var flight = repo.GetFlightById(fid);
            if (flight.FlightStatus != "Scheduled") { Console.WriteLine("Cannot depart."); Console.ReadKey(); return; }
            repo.DepartFlight(fid);
            Console.WriteLine($"Flight {flight.FlightNumber} departed.");
            Console.ReadKey();
        }
    }
}