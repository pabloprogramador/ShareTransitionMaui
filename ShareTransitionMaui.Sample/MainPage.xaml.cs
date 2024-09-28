using ShareTransitionMaui.Sample.Models;
namespace ShareTransitionMaui.Sample;

public partial class MainPage : ContentPage
{
    
    
    public MainPage()
	{
		InitializeComponent();
	}

    void Sample_Clicked(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new Pages.Sample());
    }

    void Elements_Clicked(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new Pages.Elements());
    }

    void Marvel_Clicked(System.Object sender, System.EventArgs e)
    {
        Navigation.PushAsync(new Pages.SampleMarvel());
    }

}

