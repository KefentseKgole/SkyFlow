namespace SkyFlow.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public string PassengerName { get; set; }
        public string SeatNumber { get; set; }
        public string Status { get; set; }
    }
}
