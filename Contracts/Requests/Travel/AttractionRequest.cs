using System.Collections.Generic;

namespace Contracts.Requests.Travel
{
    public class AttractionRequest
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Mood { get; set; }

        public List<string> Preferences { get; set; }
    }
}
