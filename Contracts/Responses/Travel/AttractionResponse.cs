using System.Collections.Generic;

namespace Contracts.Responses.Travel
{
    public class AttractionItem
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public double DistanceKm { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Rating { get; set; }
    }

    public class AttractionResponse
    {
        public string Mood { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int TotalResults { get; set; }

        public List<AttractionItem> Attractions { get; set; }
    }
}
