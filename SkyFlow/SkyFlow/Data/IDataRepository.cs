using SkyFlow.Models;

namespace SkyFlow.Data
{
    public interface IDataRepository
    {
        User AuthenticateUser(string username, string password);
        List<Flight> GetAllFlights();
        Flight GetFlightById(int flightId);
        void AddFlight(Flight flight);
        List<Flight> GetAllFlightsWithOccupancy();
        void DepartFlight(int flightId);
        void AddUser(string username, string password, string role);
        List<PassengerManifestItem> GetPassengersByFlight(int flightId);
        Booking GetBookingByFlightAndPassengerIdentifier(int flightId, string identifier);
        void UpdateBookingStatus(int bookingId, string newStatus);
    }
}