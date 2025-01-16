namespace MauiWeather
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(WeatherDetailsPage), typeof(WeatherDetailsPage));
        }
    }
}
