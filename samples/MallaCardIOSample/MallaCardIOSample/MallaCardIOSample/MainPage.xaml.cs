using Malla.CardIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MallaCardIOSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var config = new CardIOConfig
            {
                Localization = "es",
                ShowPaypalLogo = false,
                RequireExpiry = true,
                ScanExpiry = true,
                RequireCvv = true,
                UseCardIOLogo = true,
                HideCardIOLogo = true,
                SuppressManualEntry = true,
                SuppressConfirmation = true,
            };

            var result = await CardIO.Instance.Scan(config);
            await DisplayAlert("", $"Card: {result.CardNumber} /n Date: {result.Expiry}", "Ok");
        }
    }
}
