namespace Contracts.Requests.Traffic
{
    public class TrafficRequest
    {
        public double SourceLat { get; set; } = 28.6139;
        public double SourceLng { get; set; } = 77.2090;
        public double DestinationLat { get; set; } = 28.7041;
        public double DestinationLng { get; set; } = 77.1025;
        public string TravelMode { get; set; } = "car";
    }
}
