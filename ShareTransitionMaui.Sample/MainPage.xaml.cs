namespace ShareTransitionMaui.Sample;

public partial class MainPage : ContentPage
{
	bool flag;
    
    public MainPage()
	{
		InitializeComponent();
	}

   

    private void OnCounterClicked(object sender, EventArgs e)
	{
		
        double xp = 0;
        double yp = 0;

        if (!flag)
		{
            var pointEnd = pgEnd.GetAbsolutePosition();
            Console.WriteLine($"::X: {pointEnd.X} , Y: {pointEnd.Y}");

            var point = pgStart.GetAbsolutePosition();
            Console.WriteLine($"::X: {point.X} , Y: {point.Y}");

            xp = pointEnd.X - point.X;
            yp = pointEnd.Y - point.Y;
        }

		flag = !flag;
		

        pgStart.TranslateTo(xp, yp, 1500, easing:Easing.SpringOut);

		
    }
}


//[DOTNET] ::X: 419 , Y: 596
//[DOTNET] ::X: 0 , Y: 176
//[DOTNET] ::X: 60 , Y: 176
//[DOTNET] ::X: 60 , Y: 904

//::X: 100.66666666666667 , Y: 307.6666666666667
//::X: 0 , Y: 97.66666666666669
//::X: 30 , Y: 97.66666666666669
//::X: 30 , Y: 467
