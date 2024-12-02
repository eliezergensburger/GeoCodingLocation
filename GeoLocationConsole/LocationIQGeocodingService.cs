using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class LocationIQGeocodingService
{
    private const string API_KEY = "pk.264a902768a4fb469098f6a4adcec227"; // Replace with your actual API key
    private const string BaseUrl = "https://us1.locationiq.com/v1/";
   
    /// <summary>
    /// Gets coordinates for a given address.
    /// </summary>
    public async Task<Location?> GetCoordinatesAsync(string address)
    {
        if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Address cannot be null or empty", nameof(address));

        var url = $"{BaseUrl}search.php?key={API_KEY}&q={Uri.EscapeDataString(address)}&format=json";

        var results = await SendRequestAsync<LocationIQResult[]>(url);
        return results?.Length > 0 ? new Location(results[0]) : null;
    }

    /// <summary>
    /// Gets an address for given coordinates.
    /// </summary>
    public async Task<string?> GetAddressAsync(double latitude, double longitude)
    {
        var url = $"{BaseUrl}reverse.php?key={API_KEY}&lat={latitude}&lon={longitude}&format=json";

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
