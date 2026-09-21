using Microsoft.Data.Sqlite;
using SkyFlow.Models;

namespace SkyFlow.Data
{
    public class SqliteDataRepository : IDataRepository
    {
        private readonly string _connectionString;

        public SqliteDataRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private void EnsureDatabase()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var sql = @"
                CREATE TABLE IF NOT EXISTS Users (
                    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE NOT NULL,
                    Password TEXT NOT NULL,
                    Role TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Flights (
                    FlightId INTEGER PRIMARY KEY AUTOINCREMENT,
                    FlightNumber TEXT UNIQUE NOT NULL,
                    Origin TEXT NOT NULL,
                    Destination TEXT NOT NULL,
                    DepartureTime TEXT NOT NULL,
                    Capacity INTEGER NOT NULL,
                    FlightStatus TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Passengers (
                    PassengerId INTEGER PRIMARY KEY AUTOINCREMENT,
                    FullName TEXT NOT NULL,
                    PassportNumber TEXT UNIQUE NOT NULL,
                    Contact TEXT
                );
                CREATE TABLE IF NOT EXISTS Bookings (
                    BookingId INTEGER PRIMARY KEY AUTOINCREMENT,
                    FlightId INTEGER NOT NULL,
                    PassengerId INTEGER NOT NULL,
                    SeatNumber TEXT NOT NULL,
                    Status TEXT NOT NULL,
                    FOREIGN KEY (FlightId) REFERENCES Flights(FlightId),
                    FOREIGN KEY (PassengerId) REFERENCES Passengers(PassengerId)
                );
                INSERT OR IGNORE INTO Users (Username, Password, Role) VALUES 
                    ('admin', 'admin123', 'Admin'),
                    ('gate1', 'gate123', 'GateAgent');
                INSERT OR IGNORE INTO Flights (FlightNumber, Origin, Destination, DepartureTime, Capacity, FlightStatus) VALUES
                    ('SA101', 'Johannesburg', 'Cape Town', '2026-06-01 08:00:00', 150, 'Scheduled'),
                    ('SA202', 'Durban', 'Johannesburg', '2026-06-01 10:30:00', 120, 'Scheduled');
                INSERT OR IGNORE INTO Passengers (FullName, PassportNumber, Contact) VALUES
                    ('Neroshen Govender', 'PASSPORT12345', 'neroshen@example.com'),
                    ('Alice Mkhize', 'PASSPORT67890', 'alice@example.com');
                INSERT OR IGNORE INTO Bookings (FlightId, PassengerId, SeatNumber, Status) VALUES
                    (1, 1, '12A', 'Booked'),
                    (1, 2, '14B', 'Booked');
            ";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public User AuthenticateUser(string username, string password)
        {
            EnsureDatabase();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var sql = "SELECT UserId, Username, Password, Role FROM Users WHERE Username = @u AND Password = @p";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string role = reader.GetString(3);
                User user = role == "Admin" ? new Admin() : new GateAgent();
                user.UserId = reader.GetInt32(0);
                user.Username = reader.GetString(1);
                user.Password = reader.GetString(2);
                user.Role = role;
                return user;
            }
            return null;
        }

        public List<Flight> GetAllFlights()
        {
            EnsureDatabase();
            var flights = new List<Flight>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var sql = "SELECT FlightId, FlightNumber, Origin, Destination, DepartureTime, Capacity, FlightStatus FROM Flights";
            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                flights.Add(new Flight
                {
                    FlightId = reader.GetInt32(0),
                    FlightNumber = reader.GetString(1),
                    Origin = reader.GetString(2),
                    Destination = reader.GetString(3),
                    DepartureTime = DateTime.Parse(reader.GetString(4)),
                    Capacity = reader.GetInt32(5),
                    FlightStatus = reader.GetString(6)
                });
            }
            return flights;
        }

        public Flight GetFlightById(int flightId)
        {
            EnsureDatabase();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var sql = "SELECT FlightId, FlightNumber, Origin, Destination, DepartureTime, Capacity, FlightStatus FROM Flights WHERE FlightId = @id";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", flightId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Flight
                {
                    FlightId = reader.GetInt32(0),
                    FlightNumber = reader.GetString(1),
                    Origin = reader.GetString(2),
                    Destination = reader.GetString(3),
                    DepartureTime = DateTime.Parse(reader.GetString(4)),
                    Capacity = reader.GetInt32(5),
                    FlightStatus = reader.GetString(6)
                };
            }
            return null;
        }

        public void AddFlight(Flight flight)
        {
            EnsureDatabase();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var sql = @"INSERT INTO Flights (FlightNumber, Origin, Destination, DepartureTime, Capacity, FlightStatus)
                        VALUES (@num, @orig, @dest, @dt, @cap, @stat)";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@num", flight.FlightNumber);
            cmd.Parameters.AddWithValue("@orig", flight.Origin);
            cmd.Parameters.AddWithValue("@dest", flight.Destination);
            cmd.Parameters.AddWithValue("@dt", flight.DepartureTime.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@cap", flight.Capacity);
            cmd.Parameters.AddWithValue("@stat", flight.FlightStatus);
            cmd.ExecuteNonQuery();
        }

        public List<Flight> GetAllFlightsWithOccupancy()
        {
            EnsureDatabase();
            var flights = new List<Flight>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var sql = @"
                SELECT f.FlightId, f.FlightNumber, f.Origin, f.Destination, f.DepartureTime, f.Capacity, f.FlightStatus,
                       COUNT(b.BookingId) AS Occupancy
                FROM Flights f
                LEFT JOIN Bookings b ON f.FlightId = b.FlightId AND b.Status IN ('Booked','CheckedIn','Boarded')
                GROUP BY f.FlightId, f.FlightNumber, f.Origin, f.Destination, f.DepartureTime, f.Capacity, f.FlightStatus";
            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                flights.Add(new Flight
                {
                    FlightId = reader.GetInt32(0),
                    FlightNumber = reader.GetString(1),
                    Origin = reader.GetString(2),
                    Destination = reader.GetString(3),
                    DepartureTime = DateTime.Parse(reader.GetString(4)),
                    Capacity = reader.GetInt32(5),
                    FlightStatus = reader.GetString(6),
                    CurrentOccupancy = reader.GetInt32(7)
                });
            }
            return flights;
        }

        public void DepartFlight(int flightId)
        {
            EnsureDatabase();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var sql1 = "UPDATE Flights SET FlightStatus = 'Departed' WHERE FlightId = @id AND FlightStatus = 'Scheduled'";
                using var cmd1 = new SqliteCommand(sql1, conn, tran);
                cmd1.Parameters.AddWithValue("@id", flightId);
                if (cmd1.ExecuteNonQuery() == 0) throw new Exception("Flight cannot be departed.");
                var sql2 = "UPDATE Bookings SET Status = 'Boarded' WHERE FlightId = @id AND Status = 'CheckedIn'";
                using var cmd2 = new SqliteCommand(sql2, conn, tran);
                cmd2.Parameters.AddWithValue("@id", flightId);
                cmd2.ExecuteNonQuery();
                tran.Commit();
            }
            catch { tran.Rollback(); throw; }
        }

        public void AddUser(string username, string password, string role)
        {
            EnsureDatabase();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var sql = "INSERT INTO Users (Username, Password, Role) VALUES (@u, @p, @r)";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);
            cmd.Parameters.AddWithValue("@r", role);
            cmd.ExecuteNonQuery();
        }

        public List<PassengerManifestItem> GetPassengersByFlight(int flightId)
        {
            EnsureDatabase();
            var list = new List<PassengerManifestItem>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var sql = @"
                SELECT b.BookingId, p.FullName, p.PassportNumber, b.SeatNumber, b.Status
                FROM Bookings b
                JOIN Passengers p ON b.PassengerId = p.PassengerId
                WHERE b.FlightId = @fid";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@fid", flightId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new PassengerManifestItem
                {
                    BookingId = reader.GetInt32(0),
                    PassengerName = reader.GetString(1),
                    PassportNumber = reader.GetString(2),
                    SeatNumber = reader.GetString(3),
                    Status = reader.GetString(4)
                });
            }
            return list;
        }

        public Booking GetBookingByFlightAndPassengerIdentifier(int flightId, string identifier)
        {
            EnsureDatabase();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var sql = @"
                SELECT b.BookingId, b.FlightId, b.PassengerId, b.SeatNumber, b.Status, p.FullName
                FROM Bookings b
                JOIN Passengers p ON b.PassengerId = p.PassengerId
                WHERE b.FlightId = @fid AND (p.PassengerId = @id OR p.PassportNumber = @id)";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@fid", flightId);
            cmd.Parameters.AddWithValue("@id", identifier);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Booking
                {
                    BookingId = reader.GetInt32(0),
                    FlightId = reader.GetInt32(1),
                    PassengerId = reader.GetInt32(2),
                    SeatNumber = reader.GetString(3),
                    Status = reader.GetString(4),
                    PassengerName = reader.GetString(5)
                };
            }
            return null;
        }

        public void UpdateBookingStatus(int bookingId, string newStatus)
        {
            EnsureDatabase();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var sql = "UPDATE Bookings SET Status = @stat WHERE BookingId = @id";
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@stat", newStatus);
            cmd.Parameters.AddWithValue("@id", bookingId);
            cmd.ExecuteNonQuery();
        }
    }
}