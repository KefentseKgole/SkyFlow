namespace SkyFlow.Models
{
    public class Passenger
    {
        public int PassengerId { get; set; }
        public string FullName { get; set; }
        public string PassportNumber { get; set; }
        public string Contact { get; set; }
    }

    public class PassengerManifestItem
    {
        public int BookingId { get; set; }
        public string PassengerName { get; set; }
        public string PassportNumber { get; set; }
        public string SeatNumber { get; set; }
        public string Status { get; set; }
    }
}
