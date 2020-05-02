using SwipeCardView.Sample.View;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SwipeCardView.Sample
{
    public partial class App : Application
    {
        public static double ScreenHeight;
        public static double ScreenWidth;

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Login());
            //MainPage = new NavigationPage(new MainViewPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}