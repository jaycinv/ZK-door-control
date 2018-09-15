using DoorMan.Droid;
using DoorMan.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using zkemkeeper;
using ZXing.Net.Mobile.Forms;

namespace DoorMan.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
    {
        public Item Item { get; set; }
        public CZKEMClass Client { get; set; }
        public MainPage ()
		{
			InitializeComponent ();
            Item = new Item
            {
                IP = "192.168.1.279",
                Port = "4337"
            };
            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Item Attempt = (Item)sender;
            Client = new CZKEMClass();
            bool connected = Client.Connect_Net(Attempt.IP, int.Parse(Attempt.Port));
            if (connected)
            {
                await Navigation.PushAsync(new NewItemPage(Client));
            }
            else
            {
                await DisplayAlert("Not Connected", "We were unable to connect to the door lock", "Ok");
            }
        }

    }
}