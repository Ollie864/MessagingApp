using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Client.Models;
using Client.Data;

namespace Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        //Overridding the base method to on the page loading it will change the background colour to what the user has selected
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BackgroundColor = UserSettings.setBackgroundcolour();
        }

        //When the user presses the sign in button this method will cal the  valid user function.

        private void Button_Clicked(object sender, EventArgs e)
        {
            ValidUser();

        }

        //If the user presses the Register putton it will navigate to the Register page
        private async void buttonToRegister_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());

        }

        //This also call the valid user method upon completing the password entrybox. This is done for convinience as it is assumed that user will want to naviage to next page
        private void passwordEntry_Completed(object sender, EventArgs e)
        {
            ValidUser();
        }

        //This method checks whether entered username is in the database and if the password matches that username
        private async void ValidUser()
        {
            try
            {
                var users = await App.Database.GetSpecificContact(usernameEntry.Text);
                //If there is no user with the entered username then if will display an error
                if(users[0] == null)
                {
                    await DisplayAlert("Error", "Incorrect username or password", "ok");
                }
                else
                {
                    //Checks whether the password is equal to the password entry and assignes the userID variable to the ID of the user with the given username
                    if (users[0].Password == passwordEntry.Text)
                    {
                        UserSettings.setuserID(users[0].Id);
                        await Navigation.PushAsync(new MainPage(usernameEntry.Text));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Incorrect username or password", "ok");
                    }
                }
            }
            //If the no username matches the text enterd it will display an error
            catch
            {
                    await DisplayAlert("Error", "Please make sure you have entered your details correctly", "ok");
            }
        }


    }
}