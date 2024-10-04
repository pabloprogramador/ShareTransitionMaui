namespace ShareTransitionMaui.Sample.Pages;

public partial class Elements : ContentPage
{
	public Elements()
	{
		InitializeComponent();
        pgShareTransition.ImageDuration = 500;
        pgShareTransition.ImageEasing = Easing.SpringOut;

        pgShareTransition.ShapeDuration = 500;

        pgShareTransition.LabelDuration = 400;

    }

   async void Page1_Clicked(System.Object sender, System.EventArgs e)
    {
        if (IsBusy) return; IsBusy = true;
        pgShareTransition.LabelEasing = Easing.CubicOut;
        await pgShareTransition.GoTo(1);
        await Task.Delay(200);
        IsBusy = false;
    }

   async void Page2_Clicked(System.Object sender, System.EventArgs e)
    {
        if (IsBusy) return; IsBusy = true;
        pgShareTransition.LabelEasing = Easing.CubicIn;
        await pgShareTransition.GoTo(0);
        await Task.Delay(200);
        IsBusy = false;
    }
}
