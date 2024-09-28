namespace ShareTransitionMaui.Sample.Pages;

public partial class Elements : ContentPage
{
	public Elements()
	{
		InitializeComponent();
	}

   async void Page1_Clicked(System.Object sender, System.EventArgs e)
    {
        if (IsBusy) return; IsBusy = true;
		await pgShareTransition.GoTo(1);
        await Task.Delay(200);
        IsBusy = false;
    }

   async void Page2_Clicked(System.Object sender, System.EventArgs e)
    {
        if (IsBusy) return; IsBusy = true;
        await pgShareTransition.GoTo(0);
        await Task.Delay(200);
        IsBusy = false;
    }
}
