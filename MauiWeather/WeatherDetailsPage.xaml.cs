using MauiWeather.Viewmodels;

namespace MauiWeather;

public partial class WeatherDetailsPage : ContentPage
{
	public WeatherDetailsPage(WeatherDetailsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}