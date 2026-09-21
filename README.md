# SkyFlow Terminal Manager

## 1. Project Overview

SkyFlow Terminal Manager is a C# console-based Airport and Airline Management System.

The system allows airport staff to manage flights and passengers through a simple console interface. It uses Object-Oriented Programming (OOP) principles and a SQL Server database to store information.

## 2. Technologies Used

* C#
* .NET
* SQL Server
* ADO.NET
* Visual Studio

## 3. Main Features

### Authentication

Users log in using their username and password.

The system identifies the user's role and displays the correct dashboard.

### Admin Features

The Admin can:

* Create new flights
* View all flights
* View flight occupancy
* Add staff members

### Gate Agent Features

The Gate Agent can:

* View flight passenger manifests
* Search for passengers
* Check in passengers
* Board passengers
* Depart flights

## 4. Object-Oriented Programming

The project demonstrates the four main OOP principles.

### Inheritance

The `User` class is the base class.

It is inherited by:

* `Admin`
* `GateAgent`

### Encapsulation

Classes use private fields and public properties/methods to control access to data.

For example, flight status is controlled through methods instead of being changed directly.

### Polymorphism

The `DisplayDashboard()` method is overridden by different user roles so that each role receives its own dashboard.

### Abstraction

The `IDataRepository` interface separates database operations from the rest of the application.

## 5. Database

The application uses SQL Server to store persistent data.

The database contains the following tables:

### Users

Stores login information and user roles.

### Flights

Stores flight information such as:

* Flight number
* Origin
* Destination
* Departure time
* Capacity
* Status

### Passengers

Stores passenger information and check-in status.

### Bookings

Connects passengers to flights and stores booking status.

## 6. Database Setup

1. Open SQL Server or SQL Server LocalDB.
2. Create a database named:

```text
SkyFlowDB
```

3. Open the `SkyFlow.sql` script located in the `Database` folder.
4. Execute the script.
5. The script creates the required tables and initial users.

## 7. Default Login Details

### Admin

```text
Username: admin
Password: admin123
```

### Gate Agent

```text
Username: agent
Password: agent123
```

## 8. Project Structure

```text
SkyFlowTerminalManager
│
├── Models
│   ├── User.cs
│   ├── Admin.cs
│   ├── GateAgent.cs
│   ├── Flight.cs
│   ├── Passenger.cs
│   └── Booking.cs
│
├── Interfaces
│   └── IDataRepository.cs
│
├── Repositories
│   └── SqlRepository.cs
│
├── Services
│   ├── AuthService.cs
│   ├── FlightService.cs
│   └── PassengerService.cs
│
├── Utilities
│   └── TableRenderer.cs
│
├── Database
│   └── SkyFlow.sql
│
├── Program.cs
└── README.md
```

## 9. Running the Application

1. Open the project in Visual Studio.
2. Make sure SQL Server/LocalDB is running.
3. Check the database connection string.
4. Make sure the `SkyFlowDB` database exists.
5. Build the project.
6. Run the application.
7. Log in using one of the default accounts.

## 10. Validation

The application validates user input and prevents invalid operations such as:

* Empty input
* Invalid numbers
* Invalid flight capacity
* Invalid passenger information
* Checking in a passenger incorrectly
* Boarding a passenger when the flight cannot be boarded
* Departing an already departed flight

## 11. Console Tables

Flight and passenger information is displayed using formatted ASCII tables.

Example:

```text
+----+----------+-----------+-------------+
| ID | Flight   | Origin    | Destination |
+----+----------+-----------+-------------+
| 1  | SF101    | Durban    | Johannesburg|
+----+----------+-----------+-------------+
```

## 12. Project Requirements

The project was developed to meet the requirements of the Programming 741 SkyFlow Terminal Manager assignment, including OOP, SQL persistence, console table formatting, business validation, input validation, and documentation.
