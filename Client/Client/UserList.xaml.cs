using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Client.Models;

namespace Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserList : ContentPage
    {
        public UserList()
        {
            InitializeComponent();
        }
        //Overiding the OnAppearing method so that on the loading of the page it will display a list of users
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BackgroundColor = UserSettings.setBackgroundcolour();
            //This will assign the collectionView source to the list of contacts
            collectView.ItemsSource = await App.Database.GetContacts();
        }
    }
}