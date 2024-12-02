public class Program
{
    public static async Task Main(string[] args)
    {
         var geocodingService = new LocationIQGeocodingService();

        try
        {
            // Forward Geocoding
            //var address = "kfar ivri 10, jerusalem, israel";
            //var address = "Ha-Va'ad ha-Le'umi St 21, Jerusalem, ISRAEL";
            Console.WriteLine("Enter a valid location address");
            string address = Console.ReadLine()!;
            var location = await geocodingService.GetCoordinatesAsync(address);

            if (location != null)
            {
                Console.WriteLine("Forward Geocoded Location:");
                Console.WriteLine($"Address: {location.DisplayName}");
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
            }
            else
            {
                Console.WriteLine("Failed to retrieve coordinates.");
            }

            Console.WriteLine("--------------------------------------------------");

            // Reverse Geocoding
            if (location != null)
            {
                var reverseAddress = await geocodingService.GetAddressAsync(location.Latitude, location.Longitude);
                Console.WriteLine($"Reverse Geocoded Address:\n{reverseAddress}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

}