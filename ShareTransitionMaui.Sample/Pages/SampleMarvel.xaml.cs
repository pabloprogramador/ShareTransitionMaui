using ShareTransitionMaui.Sample.Models;
namespace ShareTransitionMaui.Sample.Pages;

public partial class SampleMarvel : ContentPage
{
	public SampleMarvel()
	{
        InitializeComponent();
        this.BindingContext = Heroes;

        pgShareTransition.ImageDuration = 900;
        pgShareTransition.ImageEasing = Easing.SpringOut;

        pgShareTransition.ShapeDuration = 300;
        

    }
    private double depth = 200; // FORCE OF PARALLAX
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

    public void Handle_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        var _currentIndex = e.CenterItemIndex;
        var widthCarousel = pgCarousel.Width - 32;
        var posOffset = (widthCarousel * (_currentIndex + 1)) - (e.HorizontalOffset / ajustDevice);
        var pos = (((posOffset * depth) / widthCarousel) - depth);
        double sizeP = (((posOffset * (sizeMax - sizeMin)) / widthCarousel) + sizeMin);

        if (sizeP > sizeMax)
        {
            sizeP = sizeMax + sizeMax - sizeP;
            sizeP = Math.Min(sizeP, sizeMax);
        }
        else
        {
            sizeP = Math.Max(sizeP, sizeMin);
        }

        if (sizeP <= 120)
        {
            return;
        }

        if (ValidArray(e.LastVisibleItemIndex, Heroes.Count()))
        {
            Heroes[e.LastVisibleItemIndex].Position = pos + depth;
            Heroes[e.LastVisibleItemIndex].Size = sizeMin + (sizeMax - sizeP);
        }

        if (ValidArray(e.FirstVisibleItemIndex, Heroes.Count()))
        {
            Heroes[e.FirstVisibleItemIndex].Size = sizeMin + (sizeMax - sizeP);
        }

        if (ValidArray(_currentIndex, Heroes.Count()))
        {
            Heroes[_currentIndex].Position = pos;
            Heroes[_currentIndex].Size = sizeP;
        }

        Heroes[e.LastVisibleItemIndex].ClassIdImage = null;
        Heroes[e.FirstVisibleItemIndex].ClassIdImage = null;

        Heroes[e.LastVisibleItemIndex].ClassIdBorder = null;
        Heroes[e.FirstVisibleItemIndex].ClassIdBorder = null;

        Heroes[e.LastVisibleItemIndex].ClassIdTitle = null;
        Heroes[e.FirstVisibleItemIndex].ClassIdTitle = null;

        Heroes[e.LastVisibleItemIndex].ClassIdPrice = null;
        Heroes[e.FirstVisibleItemIndex].ClassIdPrice = null;

        Heroes[_currentIndex].ClassIdImage = "imageHero";
        Heroes[_currentIndex].ClassIdBorder = "shapeBackground";
        Heroes[_currentIndex].ClassIdTitle = "titleShare";
        Heroes[_currentIndex].ClassIdPrice = "priceShare";

    }

    private bool ValidArray(int index, int length)
    {
        if (index >= 0 && index < length)
        {
            return true;
        }
        return false;
    }

    void ImageButton_Clicked(System.Object sender, System.EventArgs e)
    {
        pgShareTransition.LabelDuration = 700;
        pgShareTransition.LabelEasing = Easing.BounceOut;
        pgShareTransition.GoTo(1);
    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        pgShareTransition.LabelDuration = 400;
        pgShareTransition.LabelEasing = Easing.Linear;
        pgShareTransition.GoTo(0);
    }

    void Back_Clicked(System.Object sender, System.EventArgs e)
    {
        Navigation.PopAsync();
    }
}
