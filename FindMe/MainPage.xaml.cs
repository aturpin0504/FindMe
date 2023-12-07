namespace FindMe
{
    public partial class MainPage : ContentPage
    {
        CancellationTokenSource _cancelTokenSource;
        bool _isCheckingLocation;
        string _baseUrl = "https://bing.com/maps/default.aspx?cp=";

        public string UserName { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnFindMeClicked(object sender, EventArgs e)
        {
            // check status of location when in use permission
            var permissions = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            // if not granted, request permission
            if (permissions == PermissionStatus.Granted)
            {
                await ShareLocation();
            }
            else
            {
                // display alert to user
                await DisplayAlert("Location Permission", "Location permission is required to share your location", "OK");

                // request permission
                permissions = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                // if permission granted, share location
                if (permissions == PermissionStatus.Granted)
                {
                    await ShareLocation();
                }
                else
                {
                    // present error to user
                    await DisplayAlert("Location Permission", "Location permission was not granted", "OK");
                }
            }

        }

        public async Task ShareLocation()
        {
            UserName = UsernameEntry.Text;

            var locationRequest = new GeolocationRequest(GeolocationAccuracy.Best);

            var location = await Geolocation.GetLocationAsync(locationRequest);

            await Share.RequestAsync(new ShareTextRequest
            {
                Subject = "Find Me!",
                Title = "Find Me!",
                Text = $"{UserName} is sharing their location with you",
                Uri = $"{_baseUrl}{location.Latitude}~{location.Longitude}"
            });
        }
    }

}
