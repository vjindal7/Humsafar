namespace Contracts.Responses.Traffic
{
    public class TrafficResponse
    {
        public int Distance { get; set; }
        public int TravelTime { get; set; }
        public int TrafficDelay { get; set; }
        public int NoTrafficTime { get; set; }
        public string ArrivalTime { get; set; } = string.Empty;
    }
}
