using System;
using System.Collections.Generic;

namespace Contracts.Responses.Traffic
{
    public class TrafficResponse
    {
        public List<TrafficRoute> Routes { get; set; } = new();
    }

    public class TrafficRoute
    {
        public TrafficSummary Summary { get; set; } = new();
    }

    public class TrafficSummary
    {
        public int LengthInMeters { get; set; }
        public int TravelTimeInSeconds { get; set; }
        public int TrafficDelayInSeconds { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
