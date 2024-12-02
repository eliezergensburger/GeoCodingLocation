using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoLocationWPF
{
    public partial class MainWindow : Window
    {
        private readonly GeocodingService _geocodingService;

        public MainWindow()
        {
            InitializeComponent();
            _geocodingService = new GeocodingService();
        }

        private async void GetCoordinates_Click(object sender, RoutedEventArgs e)
        {
            string address = AddressInput.Text;
            if (string.IsNullOrWhiteSpace(address))
            {
                ResultText.Text = "Please enter a valid address.";
                return;
            }

            var result = await _geocodingService.GetCoordinatesFromAddressAsync(address);
            if (result != null)
            {
                LatitudeInput.Text = result.Latitude.ToString();
                LongitudeInput.Text = result.Longitude.ToString();
                ResultText.Text = $"Coordinates for '{address}':\nLatitude: {result.Latitude}, Longitude: {result.Longitude}";
            }
            else
            {
                ResultText.Text = "Address not found.";
            }
        }

        private async void GetAdress_Click(object sender, RoutedEventArgs e)
        {
           if (string.IsNullOrWhiteSpace(LatitudeInput.Text) || string.IsNullOrWhiteSpace(LongitudeInput.Text))
            {
                ResultText.Text = "Please enter a valid  latitude and longitude coordinates.";
                return;
            }

            double latitude = Double.Parse(LatitudeInput.Text);
            double longitude = Double.Parse(LongitudeInput.Text);
 
            var result = await _geocodingService.GetAddressAsync(latitude, longitude);
            if (result != null)
            {
                 ResultText.Text = $"Adress for Latitude: '{latitude}', Longitude:'{longitude}' is:\n {result}";
            }
            else
            {
                ResultText.Text = "coordinates not valid.";
            }
        }

        // Handle GotFocus event for AddressInput
        private void AddressInput_GotFocus(object sender, RoutedEventArgs e)
        {
            // Clear any previous text when the user clicks into the Address Input field
            if (AddressInput.Text == "Enter address here")
            {
                AddressInput.Clear();
            }
        }
    }
 }