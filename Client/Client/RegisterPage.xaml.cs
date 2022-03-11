using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using Client.Models;

namespace Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BackgroundColor = UserSettings.setBackgroundcolour();
        }

        //This method will first call the ValidInput method and if that returns true then it will add the user to the database
        private async void buttonToRegister_Clicked(object sender, EventArgs e)
        {
           if (validateInput() == true)
            {
             await App.Database.SaveContact(new Data.Contact
             {
                Name = registrationUsername.Text,
                Password = registrationPassword.Text,
                Email = registationEmail.Text,
                PhoneNumber = Convert.ToInt32(registrationPhoneNumber.Text)
              });
                await DisplayAlert("Success", "Account created", "Ok");
            }

        }
        //This method checks basic errors such as the fields being left blank or passwords not matching. It also calls other error checking funtions.
        private bool validateInput()
        {
            if((string.IsNullOrWhiteSpace(registrationUsername.Text)) || (string.IsNullOrWhiteSpace(registrationPassword.Text)) || (string.IsNullOrWhiteSpace(registationEmail.Text)) || (string.IsNullOrWhiteSpace(registrationPhoneNumber.Text)))
            {
                DisplayAlert("Account not created", "Input cannot be left blank", "Ok");
            }
            else if (registrationPassword.Text != confirmPassword.Text)
            {
                DisplayAlert("Account not created", "Passwords do not match", "Ok");
            }
            else if (validPassword() == false)
            {
            }
            else if (validEmail() == false)
            {
                DisplayAlert("Account not created", "Email is not valid", "Ok");
            }
            else if(registrationUsername.Text.Length > 15)
            {
                DisplayAlert("Account not created", "Username can not be longer than 15 characters", "Ok");
            }
            else if ((registrationPhoneNumber.Text.Length < 11) && (registrationPhoneNumber.Text.Length > 5))
            {
                DisplayAlert("Invalid Phone number", "Phone numbers not right length", "Ok");
            }
            else if (int.TryParse(registrationPhoneNumber.Text, out int result) == false)
            {
                DisplayAlert("Invalid Phone number", "Phone numbers can only contain numbers", "Ok");
            }
            else
            {
                return true;
            }
            return false;
        }

        //This method checks if the email is in the correct form. It uses C# regex funtion. It checks that the users email has an @ and ends in one of the specifed domains.
        private bool validEmail()
        {
            Regex rx = new Regex(@".+\@.+[.com]|[.co.uk]|[.org]");
            if (rx.IsMatch(registationEmail.Text))
            {
                return true;
            }
            return false;
        }
        private bool validPassword()
        {
            bool containsNum = false;
            bool containsUpper = false;
            bool containsSpecial = false;
            bool correctLength = false;
            bool isStrong = true;

            char[] arrayOfSpecials = { '!', '£', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '[', ']', '{', '}', '#', '~', '@', ';', ':', '?', '>', '<' };
            //This is an array of commonly used passwords that the entered password will be checked to not contain
            string[] arraryOfWeakPassword = { "password", "qwerty", "123", "football", "azerty", "password1", "login", "abc123", "abc", "soccer" };

            //This loop goes through and checks that there is at least one digit, one uppercase letter and one character from the array of special characters
            foreach (char c in confirmPassword.Text)
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

            if (confirmPassword.Text.Length > 8)
            {
                correctLength = true;
            }

            //This will set isStrong variable to false is the any of the password entry contains a string from the array of weak passwords. It is set to lower.
            if(arraryOfWeakPassword.Any(confirmPassword.Text.ToLower().Contains))
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
        //If the user clicks the reveal password button it will invert the .IsPassword property allowing the text to be seen
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (confirmPassword.IsPassword == true)
            {
                confirmPassword.IsPassword = false;
                registrationPassword.IsPassword = false;
            }
            else if (confirmPassword.IsPassword == false)
            {
                confirmPassword.IsPassword = true;
                registrationPassword.IsPassword = true;
            }
        }
    }
}