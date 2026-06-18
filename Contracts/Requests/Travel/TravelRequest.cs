namespace Contracts.Requests.Travel
{
    public class TravelRequest
    {
        public string Destination { get; set; }

        public string TripType { get; set; }

        public string CompanionType { get; set; }

        public string Mood { get; set; }

        public string Budget { get; set; }
    }
}

