using Client.Data;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace Client
{
    public partial class App : Application
    {


        private static Database database;
        public static Database Database
        {
            get
            {
                //Only creates the database if it is not alreay created
                if (database == null)
                {
                    //Providing the database path, string contact.db3 is the name of the database file
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "contact.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
