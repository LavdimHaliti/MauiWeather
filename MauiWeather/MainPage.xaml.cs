using MauiWeather.Viewmodels;

namespace MauiWeather;

public partial class MainPage : ContentPage
{

    public MainPage(MainWeatherViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
