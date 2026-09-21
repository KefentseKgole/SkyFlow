-- SkyFlow Database Setup for SQLite
-- (Also works for SQL Server with minor syntax changes)

-- Users table
CREATE TABLE IF NOT EXISTS Users (
    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT UNIQUE NOT NULL,
    Password TEXT NOT NULL,
    Role TEXT NOT NULL
);

-- Flights table
CREATE TABLE IF NOT EXISTS Flights (
    FlightId INTEGER PRIMARY KEY AUTOINCREMENT,
    FlightNumber TEXT UNIQUE NOT NULL,
    Origin TEXT NOT NULL,
    Destination TEXT NOT NULL,
    DepartureTime TEXT NOT NULL,
    Capacity INTEGER NOT NULL,
    FlightStatus TEXT NOT NULL
);

-- Passengers table
CREATE TABLE IF NOT EXISTS Passengers (
    PassengerId INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,
    PassportNumber TEXT UNIQUE NOT NULL,
    Contact TEXT
);

-- Bookings table
CREATE TABLE IF NOT EXISTS Bookings (
    BookingId INTEGER PRIMARY KEY AUTOINCREMENT,
    FlightId INTEGER NOT NULL,
    PassengerId INTEGER NOT NULL,
    SeatNumber TEXT NOT NULL,
    Status TEXT NOT NULL,
    FOREIGN KEY (FlightId) REFERENCES Flights(FlightId),
    FOREIGN KEY (PassengerId) REFERENCES Passengers(PassengerId)
);

-- Sample data
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