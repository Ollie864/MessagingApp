using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Client.Models;


namespace Client
{
    public partial class MainPage 
    {
        //Creates a new hub connection type called connection
        HubConnection connection;
        public string Username;

        //The constructor takes a string username from the previous login page and assigns it to another string called Username
        public MainPage(string username)
        {
            InitializeComponent();
            Username = username;
            usernameLabel.Text = Username;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BackgroundColor = UserSettings.setBackgroundcolour();
        }

        async void StartConnection()
        {
            //Android does not use local host and must instad use the ip of the sever.
            string serverURL = "http://localhost:21230/chatHub";
            if (Device.RuntimePlatform == Device.Android)
            {
                serverURL = "http://10.0.2.2:21230/chatHub";
            }
            //The URL of the hub connection is assigned to the string serverURL
            connection = new HubConnectionBuilder().WithUrl(serverURL).Build();
                try
             {
                //This will try to contact the server and connect
                await connection.StartAsync();

                //On receivng messages adds the message to the UI
                //This section was done in the code behind as it is dynamic
                connection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    Label label = new Label { Text = $"{user}: {message}", HorizontalOptions = LayoutOptions.Start, FontSize = 20};
                    stackMessages.Children.Add(label);

                });
                await DisplayAlert("Success", "You have connected to the server", "Ok");
             }
            //If unable to connect to the server will display an error message
             catch (Exception ex)
             {
                await DisplayAlert("Error", ex.Message, "Ok");
             }
        }

        //When the user clicks the connect button it will call the method to connect to the server
        private void Button_Clicked(object sender, EventArgs e)
        {
            StartConnection();
        }

        //This calls when the user clicks the send message button
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            //If the user is not connect then it will display an error and connect the user
            if (connection == null)
            {
                StartConnection();
                await DisplayAlert("Error", "The message could not be sent, connection is not active", "Ok");
            }
            else
            {

                await connection.InvokeAsync("SendMessage", Username, messageBox.Text);
            }
            messageBox.Text = string.Empty;

        }

        //If the user clicks the setting button it will take them to the settings page
        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        //If the user selects the userlist button it will take them to the userlist page
        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserList());
        }
    }
}
