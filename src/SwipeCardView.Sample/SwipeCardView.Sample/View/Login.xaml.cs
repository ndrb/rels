using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FireAuth;

namespace SwipeCardView.Sample.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        IAuth auth;

        public Login()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }

        async void LoginClicked(object sender, EventArgs e)
        {
            if (auth == null)
            {
                Console.WriteLine("Object is null FUCK!!!");
                Console.WriteLine("This is C#");
                return;
            }
            else 
            {
                Console.WriteLine("Object is NOT NULL!!!");
            }
            if (auth.LoginWithEmailPassword(EmailInput.Text, PasswordInput.Text) == null)
            {
                Console.WriteLine("KMS");
            }
            else
            {
                Console.WriteLine("KMS: "+ EmailInput.Text);
            }
            
            string Token = await auth.LoginWithEmailPassword(EmailInput.Text, PasswordInput.Text);

            if (Token == null)
            {
                Console.WriteLine("Floor it!");
            }

            if (Token != "")
            {
                await Navigation.PushAsync(new LoggedPage());
            }
            else
            {
                ShowError();
            }
        }

        async private void ShowError()
        {
            await DisplayAlert("Authentication Failed", "E-mail or password are incorrect. Try again!", "OK");
        }


    }
}