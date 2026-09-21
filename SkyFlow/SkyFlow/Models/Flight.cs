using System;

namespace SkyFlow.Models
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public int Capacity { get; set; }
        public string FlightStatus { get; set; }
        public int CurrentOccupancy { get; set; }
    }
}