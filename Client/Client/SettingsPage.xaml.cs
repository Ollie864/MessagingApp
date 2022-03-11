using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Client.Models;
using Xamarin.Essentials;

namespace Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : CarouselPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            //The dark theme toggle is set to what is the saved value in the prefrences
            darkThemeToggle.IsToggled = Preferences.Get("darkThemePrefrence", false);
            BackgroundColor = UserSettings.setBackgroundcolour();

        }
        //This saves and applies the prefrences
        private void Button_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("darkThemePrefrence", darkThemeToggle.IsToggled);
            BackgroundColor = UserSettings.setBackgroundcolour();
        }

        //This clears the prefrences
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Preferences.Clear();
        }

        //Deleted account
        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Delete Account?", "This action can not be undone", "Yes", "No");
            if (answer)
            {
                   //This will delete the account using the ID currently associated with login session after deleting it will return the user to the login page
                    await App.Database.DeleteContact(UserSettings.getUserID());
                    await DisplayAlert("Success, Account deleted", "You are now being returned to the login page", "Ok");
                    await Navigation.PushAsync(new LoginPage());
            }
        }

        //Update account
        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UpdateAccount());

        }
    }
}