namespace Contracts.Requests.Route
{
    public class RouteRequest
    {
        public string Source { get; set; }

        public string Destination { get; set; }

        public string TripType { get; set; }

        public string Vibe { get; set; }

        public string Travelers { get; set; }

        public int Budget { get; set; }

        public bool ScenicRoute { get; set; }

        public string CurrentLatitude { get; set; }
        public string CurrentLongitude { get; set; }
    }
}
