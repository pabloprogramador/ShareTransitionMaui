using ShareTransitionMaui.Sample.Models;
namespace ShareTransitionMaui.Sample;

public partial class MainPage : ContentPage
{
    private double depth = 300; // FORCE OF PARALLAX
    private double sizeMin = 120; // FORCE OF PARALLAX
    private double sizeMax = 200; // FORCE OF PARALLAX
    private int ajustDevice = DeviceInfo.Current.Platform == DevicePlatform.iOS ? 1 : 1;

    List<Hero> Heroes = new List<Hero>
	{
		new Hero{
			Name="Hulk",
			Price="€ 34,90",
			Color1="#47c93a",
			Color2="#13550d",
			Image="hulk.png",
            Size=1
        },
        new Hero{
            Name="Spider",
            Price="€ 25,00",
            Color1="#e10000",
            Color2="#640b0b",
            Image="spider1.png",
            Size=.7
        },
        new Hero{
            Name="Spider 2",
            Price="€ 34,90",
            Color1="#640b0b",
            Color2="#350404",
            Image="spider2.png",
            Size=.7
        },
        new Hero{
            Name="Ironman",
            Price="€ 70,20",
            Color1="#eeaa0e",
            Color2="#bb6805",
            Image="iron.png",
            Size=.7
        },
    };

    
    public MainPage()
	{
		InitializeComponent();
        this.BindingContext = Heroes;
        //pgCarousel.ItemsSource = Heroes;
	}

   

    public void Handle_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        var _currentIndex = e.CenterItemIndex;
        var widthCarousel = pgCarousel.Width - 32;
        var posOffset = (widthCarousel * (_currentIndex + 1)) - (e.HorizontalOffset / ajustDevice);
        var pos = (((posOffset * depth) / widthCarousel) - depth);
        double sizeP = (((posOffset * (sizeMax - sizeMin)) / widthCarousel) + sizeMin) ;

        if (sizeP > sizeMax)
        {
            sizeP = sizeMax + sizeMax - sizeP;
            sizeP = Math.Min(sizeP, sizeMax);
        }
        else
        {
            sizeP = Math.Max(sizeP, sizeMin);
        }


        Heroes[e.LastVisibleItemIndex].Position = pos + depth;
        Heroes[_currentIndex].Position = pos;

        Heroes[e.LastVisibleItemIndex].Size = sizeMin + (sizeMax - sizeP);
        Heroes[e.FirstVisibleItemIndex].Size = sizeMin + (sizeMax - sizeP);
        Heroes[_currentIndex].Size = sizeP;

    }






    private void OnCounterClicked(object sender, EventArgs e)
	{
  //      // Suponha que você tenha um StackLayout chamado stackLayout
  //      var children = pgVertical.Children;

  //      // Filtrar Labels pelo grupo "group1"
  //      var group1Labels = children
  //          .OfType<Label>() // Apenas Labels
  //          .Where(label => label.ClassId == "group1"); // Filtra pelo ClassId

  //      // Exemplo: alterar a cor de fundo de todas as Labels do grupo "group1"
  //      foreach (var label in group1Labels)
  //      {
  //          label.BackgroundColor = Colors.LightBlue;
  //      }


  //      double xp = 0;
  //      double yp = 0;

  //      if (!flag)
		//{
  //          var pointEnd = pgEnd.GetAbsolutePosition();
  //          Console.WriteLine($"::X: {pointEnd.X} , Y: {pointEnd.Y}");

  //          var point = pgStart.GetAbsolutePosition();
  //          Console.WriteLine($"::X: {point.X} , Y: {point.Y}");

  //          xp = pointEnd.X - point.X;
  //          yp = pointEnd.Y - point.Y;
  //      }

		//flag = !flag;
		

  //      pgStart.TranslateTo(xp, yp, 1500, easing:Easing.SpringOut);

		
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
