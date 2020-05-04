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
    public partial class SignUpPage : ContentPage
    {
        IAuth auth;

        public SignUpPage()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }

        async void SignUpClicked(object sender, EventArgs e)
        {
            bool created = auth.SignUpWithEmailPassword(EmailInput.Text, PasswordInput.Text);

            string Token = await auth.LoginWithEmailPassword(EmailInput.Text, PasswordInput.Text);

            if (Token != "")
            {
                await Navigation.PushAsync(new Profile());
            }
            else
            {
                ShowError();
            }

            /*
            if (created)
            {
                await DisplayAlert("Success", "Welcome to our system. Log in to have full access", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Sign Up Failed", "Something went wrong. Try again!", "OK");
            }*/
        }
        async private void ShowError()
        {
            await DisplayAlert("Authentication Failed", "E-mail or password are incorrect. Try again!", "OK");
        }

    }
}