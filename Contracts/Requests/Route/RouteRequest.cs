namespace Contracts.Requests.Route
{
    public class RouteRequest
    {
        public string Source { get; set; }

        public string Destination { get; set; }

        public string TripType { get; set; }

        public string Vibe { get; set; }

        public int Travelers { get; set; }

        public decimal Budget { get; set; }

        public bool ScenicRoute { get; set; }
    }
}
