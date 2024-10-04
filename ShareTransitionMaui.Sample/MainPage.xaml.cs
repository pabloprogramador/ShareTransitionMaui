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

    void List_Clicked(System.Object sender, System.EventArgs e)
    {
        var page = (ContentPage)Shell.Current.CurrentPage;

        var result = ShareTransitionMaui.ViewExtensions.GetAbsolutePosition(Shell.Current.CurrentPage);

        Console.WriteLine("CURRENT PAGE " + result.X + " , " + result.Y);

        Console.WriteLine("CURRENT PAGE " + page.Height);

        result = ShareTransitionMaui.ViewExtensions.GetAbsolutePosition(pgGround);
        Console.WriteLine("BACKGROUND IMAGE " + result.X + " , " + result.Y);

        result = ShareTransitionMaui.ViewExtensions.GetAbsolutePosition(pgLogoFim);
        Console.WriteLine("RED POINT " + result.X + " , " + result.Y);

        // Obter a resolução do ecrã em iOS ou Android
        var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

        var density = mainDisplayInfo.Density;

        Console.WriteLine($"Density: {density}");


        var altura = mainDisplayInfo.Height / density;
        Console.WriteLine("ALTURA: " + altura);
    }
}

