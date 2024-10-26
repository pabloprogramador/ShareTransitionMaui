using Microsoft.Maui.Controls;
using ShareTransitionMaui.Sample.Models;

namespace ShareTransitionMaui.Sample.Pages;

public partial class ListPage : ContentPage
{
    private int index = 0;

    private BackButtonBehavior backBlock = new BackButtonBehavior
    {
        IsEnabled = true,
        IsVisible = true,
        TextOverride = "Go Back",
        IconOverride = "back_icon.png",
        Command = new Command(() => { }),
        CommandParameter = "Custom Parameter"
    };

    public ListPage()
	{
		InitializeComponent();
        pgShareTransition.FrameDuration = 700;
        pgShareTransition.LabelDuration = 600;
        pgShareTransition.FadeDuration = 500;
        pgShareTransition.FrameEasing = Easing.CubicOut;

        foreach (var item in UsersMock.GetUsers())
        {
            var view = new Views.UserView(item.Avatar,
                item.Name,
                item.Position,
                item.Appointment,
                item.SummarySmall,
                item.Summary
                );

            view.Command = new Command<Views.UserView>((Views.UserView selected) =>
            {
                foreach (Views.UserView temp in pgListStack.Children)
                {
                    temp.ClearClassIds();
                }
                selected.SetClassIds();
                pgSummary.Text = selected.Summary;
                GoTo1();
            });

            pgListStack.Children.Add(view);
        }
	
    }

  
  async void Back_Clicked(System.Object sender, System.EventArgs e)
    {
        GoTo0();
    }

   protected override bool OnBackButtonPressed()
    {
        if (index == 0)
        {
            return base.OnBackButtonPressed();
        }
        else
        {
            GoTo0();
            return false;
        }
    }

    private void OnBackButton()
    {
        GoTo0();
    }

    private void GoTo0()
    {
        
        index = 0;
        pgShareTransition.GoTo(index);
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior());
    }

    private void GoTo1()
    {
        
        BackButtonBehavior backDefault = new BackButtonBehavior
        {
            Command = new Command(OnBackButton)
        };
        Shell.SetBackButtonBehavior(this, backDefault);
        index = 1;
        pgShareTransition.GoTo(index);
    }

}
