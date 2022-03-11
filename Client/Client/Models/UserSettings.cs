using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Client.Models
{
    public static class UserSettings
    {

        //The userID from the database assigned in the login page and used in the account settings page
        private static int userID;

        public static void setuserID(int id)
        {
            userID = id;
        }
        public static int getUserID()
        {
            return userID;
        }


        private static Xamarin.Forms.Color backgroundColor = Xamarin.Forms.Color.White;
        public static Xamarin.Forms.Color setBackgroundcolour()
        {
            if (Preferences.Get("darkThemePrefrence", false) == true)
            {
                backgroundColor = Xamarin.Forms.Color.DimGray;
            }
            else
            {
                backgroundColor = Xamarin.Forms.Color.White;
            }
            return backgroundColor;
        }

    }
}
