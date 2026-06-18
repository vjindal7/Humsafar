using System;
using System.Collections.Generic;

namespace Contracts.Responses.Travel
{
    public class TravelResponse
    {
        public Guid TripId { get; set; }

        public string JourneySummary { get; set; }

        public List<string> Suggestions { get; set; }
    }
}
