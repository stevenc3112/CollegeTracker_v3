using SClarkC971PA.Services;
using SClarkC971PA.Models;
namespace SClarkC971PA.Views;

public partial class LoginPage : ContentPage
{
    private User _user = new User();
	public LoginPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (await DatabaseService.GetUsers() == null)
        {
            Services.Settings.FirstRun = false;

            Title = "Sign in";
            InstructionLbl.Text = "Sign in with your username and password";
        }

        //await RefreshTermCollectionView();
        //await ShowCourseNotifications();
        //await ShowAssessmentNotifications();
    }
    private void SignInBtn_Clicked(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(UsernameEntry.Text) && !String.IsNullOrEmpty(PasswordEntry.Text)) 
        {
            ErrorLbl.Text = "";
            ErrorLbl.IsVisible = true;
        }
        else
        {
            ErrorLbl.Text = "Username and password must both contain values.";
        }
    }
}