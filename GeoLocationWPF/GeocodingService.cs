using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocationWPF
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class GeocodingService
    {
        //private const string API_KEY = "pk.264a902768a4fb469098f6a4adcec227"; // Replace with your actual API key
        private const string API_KEY = "66e985d579380435186975nvc4571ec"; // Replace with your actual API key
        // const string BaseUrl = "https://us1.locationiq.com/v1/";      // Replace with your actual provider URL
        private const string BaseUrl = "https://geocode.maps.co/search?q=address&api_key=66e985d579380435186975nvc4571ec";      // Replace with your actual provider URL

        public async Task<Location?> GetCoordinatesFromAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address cannot be null or empty", nameof(address));

            //var url = $"{BaseUrl}search.php?key={API_KEY}&q={Uri.EscapeDataString(address)}&format=json";
            var url = $"{BaseUrl}search?q={Uri.EscapeDataString(address)}&api_key=={API_KEY}&format=json";

            var results = await SendRequestAsync<LocationIQResult[]>(url);
            return results?.Length > 0 ? new Location(results[0]) : null;

        }

        /// <summary>
        /// Gets an address for given coordinates.
        /// </summary>
        public async Task<string?> GetAddressAsync(double latitude, double longitude)
        {
            //var url = $"{BaseUrl}reverse.php?key={API_KEY}&lat={latitude}&lon={longitude}&format=json";
            var url = $"{BaseUrl}reverse?api_key={API_KEY}&lat={latitude}&lon={longitude}&format=json";

            var result = await SendRequestAsync<LocationIQReverseResult>(url);
            return result?.DisplayName;
        }
        /// <summary>
        /// Sends an HTTP request to LocationIQ API and parses the response.
        /// </summary>
        private async Task<T?> SendRequestAsync<T>(string url) where T : class
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                return null;
            }
        }
    }

}
