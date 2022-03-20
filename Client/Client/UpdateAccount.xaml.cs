using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Client.Models;
using System.Text.RegularExpressions;

namespace Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateAccount : ContentPage
    {
        public UpdateAccount()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BackgroundColor = UserSettings.setBackgroundcolour();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            List<Data.Contact> users = await App.Database.GetSpecificContact(UserSettings.getUserID());
                if (updatePassword.Text != null)
                {
                    if (validPassword() == true)
                    {
                        users[0].Password = updatePassword.Text;
                    }
                }
                if (updateUsername.Text != null)
                {
                    if(updateUsername.Text.Length < 15)
                    {
                        users[0].Name = updateUsername.Text;
                    }
                }
                if (updatePhoneNumber.Text != null)
                {
                    if(validPhone() == true)
                    {
                        users[0].PhoneNumber = Convert.ToInt32(updatePhoneNumber.Text);
                    }
                }
                if (updateEmail.Text != null)
                {
                    if(validEmail() == true)
                    {
                        users[0].Email = updateEmail.Text;
                    }
                }
                await DisplayAlert("Success", "Account details updated", "ok");

        }
        private bool validateInput()
        {
            if (validPassword() == false)
            {
            }
            else if (validEmail() == false)
            {
                DisplayAlert("Account not updated", "Email is not valid", "Ok");
            }
            else if(updateUsername.Text.Length > 15)
            {
                DisplayAlert("Account not created", "Username can not be longer than 15 characters", "Ok");
            }
            else if ((updatePhoneNumber.Text.Length < 11) && (updatePhoneNumber.Text.Length < 5))
            {
                DisplayAlert("Invalid Phone number", "Phone numbers not right length", "Ok");
            }
            else if (int.TryParse(updatePhoneNumber.Text, out int result) == false)
            {
                DisplayAlert("Invalid Phone number", "Phone numbers can only contain numbers", "Ok");
            }
            else
            {
                return true;
            }
            return false;
        }
        private bool validPhone()
        {
            if (updatePhoneNumber.Text.Length < 5)
            {
                DisplayAlert("Invalid Phone number", "Phone numbers too short", "Ok");
                return false;
            }
            else if (updatePhoneNumber.Text.Length > 11)
            {
                DisplayAlert("Invalid Phone number", "Phone numbers too long", "Ok");
                return false;
            }
            else if (int.TryParse(updatePhoneNumber.Text, out int result) == false)
            {
                DisplayAlert("Invalid Phone number", "Phone numbers can only contain numbers", "Ok");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool validEmail()
        {
            Regex rx = new Regex(@".+\@.+[.com]|[.co.uk]|[.org]");
            if (rx.IsMatch(updateEmail.Text))
            {
                return true;
            }
            return false;
        }
        private bool validPassword()
        {
            if (updatePassword.Text == null)
            {
                return true;
            }
            bool containsNum = false;
            bool containsUpper = false;
            bool containsSpecial = false;
            bool correctLength = false;
            bool isStrong = true;

            char[] arrayOfSpecials = { '!', '£', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '[', ']', '{', '}', '#', '~', '@', ';', ':', '?', '>', '<' };
            string[] arraryOfWeakPassword = { "password", "qwerty", "123", "football", "azerty", "password1", "login", "abc123", "abc", "soccer" };

            foreach (char c in updatePassword.Text)
            {
                if (char.IsDigit(c))
                    containsNum = true;
                else if (char.IsUpper(c))
                    containsUpper = true;
                for (int i = 0; i < arrayOfSpecials.Length; i++)
                {
                    if (c == arrayOfSpecials[i])
                    {
                        containsSpecial = true;
                    }
                }
            }

            if (updatePassword.Text.Length > 8)
            {
                correctLength = true;
            }

            if(arraryOfWeakPassword.Any(updatePassword.Text.ToLower().Contains))
            {
                isStrong = false;
            }



            if (!correctLength)
            {
                DisplayAlert("Account not created", "Password is too short", "Ok");
            }
            else if (!containsNum)
            {
                DisplayAlert("Account not created", "Password does not contain a number", "Ok");
            }
            else if (!containsSpecial)
            {
                DisplayAlert("Account not created", "Password does not contain a special character", "Ok");
            }
            else if (!containsUpper)
            {
                DisplayAlert("Account not created", "Password does not contain an uppercase character", "Ok");
            }
            else if (!isStrong)
            {
                DisplayAlert("Password is not strong enough", "Commonly used passwords are not valid", "Ok");
            }
            else if ((correctLength == true) && (containsNum == true) && (containsSpecial == true) && (containsUpper == true) && (isStrong == true))
            {
                return true;
            }
            return false;
        }
        
    }
}