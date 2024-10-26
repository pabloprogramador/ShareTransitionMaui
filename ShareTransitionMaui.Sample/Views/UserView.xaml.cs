namespace ShareTransitionMaui.Sample.Views;

public partial class UserView : ContentView
{
	public Command Command;
	public string Summary;

	public UserView(string avatar, string name, string position, string appointment, string summarySmall, string summary)
	{
		InitializeComponent();
		pgName.Text = name;
		pgAppointment.Text = appointment;
		pgAvatar.Source = avatar;
		pgPosition.Text = position;
		pgSummarySamll.Text = summarySmall;
		Summary = summary;
	}

	public void ClearClassIds()
	{
		pgName.ClassId = "";
		pgPosition.ClassId = "";
        pgAvatarClip.ClassId = "";
	}

    public void SetClassIds()
    {
        pgName.ClassId = "NameST";
		pgPosition.ClassId = "PositionST";
		pgAvatarClip.ClassId = "AvatarST";
    }

    void ImageButton_Clicked(System.Object sender, System.EventArgs e)
    {
		if (Command!=null)
			Command.Execute(this);
    }
}
