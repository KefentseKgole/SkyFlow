using System.Text;
using SkyFlow.Models;

namespace SkyFlow.Utilities
{
    public static class TablePrinter
    {
        public static void PrintFlightOverview(List<Flight> flights)
        {
            if (flights.Count == 0) { Console.WriteLine("No flights."); return; }
            var sb = new StringBuilder();
            sb.AppendLine("+----+----------------+----------------------+----------------------+---------------------+--------+----------+------------+");
            sb.AppendLine("| ID | Flight Number  | Origin               | Destination          | Departure Time      | Capacity| Occupancy| Status     |");
            sb.AppendLine("+----+----------------+----------------------+----------------------+---------------------+--------+----------+------------+");
            foreach (var f in flights)
                sb.AppendLine($"| {f.FlightId,-3} | {f.FlightNumber,-14} | {f.Origin,-20} | {f.Destination,-20} | {f.DepartureTime:yyyy-MM-dd HH:mm} | {f.Capacity,6} | {f.CurrentOccupancy,8} | {f.FlightStatus,-10} |");
            sb.AppendLine("+----+----------------+----------------------+----------------------+---------------------+--------+----------+------------+");
            Console.WriteLine(sb);
        }

        public static void PrintFlightList(List<Flight> flights)
        {
            var sb = new StringBuilder();
            sb.AppendLine("+----+----------------+----------------------+----------------------+---------------------+--------+------------+");
            sb.AppendLine("| ID | Flight Number  | Origin               | Destination          | Departure Time      | Capacity| Status     |");
            sb.AppendLine("+----+----------------+----------------------+----------------------+---------------------+--------+------------+");
            foreach (var f in flights)
                sb.AppendLine($"| {f.FlightId,-3} | {f.FlightNumber,-14} | {f.Origin,-20} | {f.Destination,-20} | {f.DepartureTime:yyyy-MM-dd HH:mm} | {f.Capacity,6} | {f.FlightStatus,-10} |");
            sb.AppendLine("+----+----------------+----------------------+----------------------+---------------------+--------+------------+");
            Console.WriteLine(sb);
        }

        public static void PrintPassengerManifest(List<PassengerManifestItem> passengers)
        {
            if (passengers.Count == 0) { Console.WriteLine("No passengers."); return; }
            var sb = new StringBuilder();
            sb.AppendLine("+----------+--------------------------------+---------------------+-----------+------------+");
            sb.AppendLine("| BookingId| Passenger Name                 | Passport Number     | Seat      | Status     |");
            sb.AppendLine("+----------+--------------------------------+---------------------+-----------+------------+");
            foreach (var p in passengers)
                sb.AppendLine($"| {p.BookingId,-8} | {p.PassengerName,-30} | {p.PassportNumber,-19} | {p.SeatNumber,-9} | {p.Status,-10} |");
            sb.AppendLine("+----------+--------------------------------+---------------------+-----------+------------+");
            Console.WriteLine(sb);
        }
    }
}