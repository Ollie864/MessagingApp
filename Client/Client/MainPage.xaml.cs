using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Client.Models;
using System.Security.Cryptography;
using System.IO;


namespace Client
{
    public partial class MainPage 
    {
        //Creates a new hub connection type called connection
        HubConnection connection;
        public string Username;
        private Aes cipher = CreateEncryption();
        private string IV;

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
                    string plainMessage = DecryptMessage(message);
                    Label label = new Label { Text = $"{user}: {plainMessage}", HorizontalOptions = LayoutOptions.Start, FontSize = 20};
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

                //await connection.InvokeAsync("SendMessage", Username, messageBox.Text);
                await connection.InvokeAsync("SendMessage", Username, EncryptMessage(messageBox.Text));
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
        public string EncryptMessage(string plainMessage)
        {

            IV = Convert.ToBase64String(cipher.IV);
            ICryptoTransform cryptoTransform = cipher.CreateEncryptor();
            byte[] plainMessageBytes = Encoding.UTF8.GetBytes(plainMessage);
            byte[] cipherText = cryptoTransform.TransformFinalBlock(plainMessageBytes, 0, plainMessageBytes.Length);

            string CipherText = Convert.ToBase64String(cipherText);
            return CipherText;
        }
        public string DecryptMessage(string cipherMessage)
        {
            //Using the same Initialisation vector for decryption
            cipher.IV = Convert.FromBase64String(IV);

            ICryptoTransform cryptoTransform = cipher.CreateDecryptor();
            byte[] cipherText = Convert.FromBase64String(cipherMessage);
            byte[] plainText = cryptoTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);
            //string PlainText = Convert.ToBase64String(plainText);
            string PlainText = Encoding.UTF8.GetString(plainText);

            return PlainText;

        }
        public static Aes CreateEncryption()
        {
            //Creates a default encryption with default settings
            Aes encryptionScheme = Aes.Create();

            //encryptionScheme.Mode = CipherMode.ECB
            string stringKey = "02onvfdkkhgj8723";

            encryptionScheme.Key = Encoding.ASCII.GetBytes(stringKey);

            return encryptionScheme;


        }

    }
}
