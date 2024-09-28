namespace ShareTransitionMaui.Sample.Pages;

public partial class Sample : ContentPage
{
	private int index = 0;
	
	public Sample()
	{
		InitializeComponent();
	}

    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
		if (IsBusy) return; IsBusy = true;
		index = index == 0 ? 1 : 0;
		await pgShareTransition.GoTo(index);
		await Task.Delay(300);
		IsBusy = false;
    }
}
