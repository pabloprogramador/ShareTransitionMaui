using Microsoft.Maui.Controls;

namespace ShareTransitionMaui.Sample.Pages;

public partial class MasterCard : ContentPage
{
    private uint duration = 300;
	public MasterCard()
	{
		InitializeComponent();
        pgShareTransition.HasFade = false;
        pgShareTransition.FadeDuration = (int)duration;
        pgShareTransition.ImageDuration = 400;
	}

   async void Go1_Clicked(System.Object sender, System.EventArgs e)
    {
        if (IsBusy) return; IsBusy = true;

        pg1_p1.TranslateTo(0, -50, duration);
        pg1_p1.FadeTo(0, duration);

        pg2_p1.TranslationY = -50;
        pg2_p1.Opacity = 0;
        pg2_p1.TranslateTo(0, 0, duration);
        pg2_p1.FadeTo(1, duration);


        pg1_p2.TranslateTo(0, 50, duration);
        pg1_p2.FadeTo(0, duration);
        pg2_p2.TranslationY = 50;
        pg2_p2.Opacity = 0;
        pg2_p2.TranslateTo(0, 0, duration);
        pg2_p2.FadeTo(1, duration);


        pgShareTransition.GoTo(1);

        await Task.Delay(200);
        pgGrid.FadeTo(0, duration);

        await Task.Delay(500);

        IsBusy = false;
    }

  async void Go0_Clicked(System.Object sender, System.EventArgs e)
    {
        if (IsBusy) return; IsBusy = true;

        pg1_p1.TranslateTo(0, 0, duration);
        pg1_p1.FadeTo(1, duration);

        pg2_p1.TranslateTo(0, -50, duration);
        pg2_p1.FadeTo(0, duration);

        pg1_p2.TranslateTo(0, 0, duration);
        pg1_p2.FadeTo(1, duration);

        pg2_p2.TranslateTo(0, 50, duration);
        pg2_p2.FadeTo(0, duration);

        pgGrid.FadeTo(1, duration);

        await pgShareTransition.GoTo(0);

        await Task.Delay(500);
        IsBusy = false;
    }
}
