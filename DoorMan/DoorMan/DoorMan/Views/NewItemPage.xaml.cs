using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DoorMan.Models;
using DoorMan.Droid;
using ZXing.Net.Mobile.Forms;
using ZXing.Mobile;
using System.Net;
using System.Text;
using System.IO;
using zkemkeeper;

namespace DoorMan.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }
        public CZKEMClass axCZKEM1 = new CZKEMClass();
        public string QrCode { get; set; }
        public NewItemPage(CZKEMClass client)
        {
            InitializeComponent();
            axCZKEM1 = client;
            BindingContext = this;
            MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions();
            options.UseFrontCameraIfAvailable = true;
            var scan = new ZXingScannerPage(options);
            Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                QrCode = result.Text;
                if(ValidateQrCode())
                {
                    axCZKEM1.ACUnlock(1, 5);
                }
            };
        }

        public bool ValidateQrCode()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://evolve-ict.co.za/validateCode.php");

            var postData = String.Format("QrCode={0}",QrCode);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return bool.Parse(responseString);
        }
    }
}