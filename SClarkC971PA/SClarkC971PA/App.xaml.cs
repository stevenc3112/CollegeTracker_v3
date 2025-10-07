using SClarkC971PA.Views;
namespace SClarkC971PA
{
    public partial class App : Application
    {
        public App()
        {

        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new LoginPage()));
        }
    }
}
