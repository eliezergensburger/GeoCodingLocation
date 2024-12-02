using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocationWPF
{

    /// <summary>
    /// Represents a location with coordinates and an address.
    /// </summary>
    public class Location
    {
        public double Latitude { get; }
        public double Longitude { get; }
        public string? DisplayName { get; }

        public Location(LocationIQResult result)
        {
            if (!double.TryParse(result.Latitude, out var lat) || !double.TryParse(result.Longitude, out var lon))
                throw new ArgumentException("Invalid latitude or longitude values in API response.");

            Latitude = lat;
            Longitude = lon;
            DisplayName = result.DisplayName;
        }
    }

    /// <summary>
    /// Represents the result from LocationIQ forward geocoding API.
    /// </summary>
    public class LocationIQResult
    {
        [JsonProperty("lat")]
        public string? Latitude { get; set; }

        [JsonProperty("lon")]
        public string? Longitude { get; set; }

        [JsonProperty("display_name")]
        public string? DisplayName { get; set; }
    }

    /// <summary>
    /// Represents the result from LocationIQ reverse geocoding API.
    /// </summary>
    public class LocationIQReverseResult
    {
        [JsonProperty("display_name")]
        public string? DisplayName { get; set; }
    }

}
