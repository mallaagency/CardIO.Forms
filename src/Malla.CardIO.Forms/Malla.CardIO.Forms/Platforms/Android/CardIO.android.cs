using Android.App;
using Android.Content;
using Xamarin.Forms;

using Malla.CardIO.Android;

[assembly: Dependency(typeof(CardIO))]
namespace Malla.CardIO.Android
{
    using System.Threading.Tasks;
    using Malla.CardIO;
    using System;
    using Card.IO;

    /// <summary>
    /// Android implementation of the PayPal CardIO plugin
    /// </summary>
    public class CardIO : ICardIO
    {
        private const int ScanActivityResultCode = 2220;

        private static CardIO _currentScan;
        private static Activity _activity;

        private CardIOResult _result;
        private bool _finished;

        /// <summary>
        /// Initialize the <c>CardIO</c> plugin with the activity instance. This is a must!
        /// </summary>
        /// <param name="activity">Activity instance</param>
        public static void Initialize(Activity activity)
        {
            _activity = activity;
        }

        /// <summary>
        /// Forwards <c>OnActivityResult</c> to the <c>CardIO</c> plugin. This is a must!
        /// </summary>
        /// <param name="requestCode">Request code</param>
        /// <param name="resultCode">Result code</param>
        /// <param name="data">Intent data</param>
        public static void ForwardActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == ScanActivityResultCode)
            {
                if (data != null && data.HasExtra(CardIOActivity.ExtraScanResult))
                {
                    CreditCard scanResult = (CreditCard)data.GetParcelableExtra(CardIOActivity.ExtraScanResult);
                    
                    _currentScan._result = new CardIOResult
                    {
                        CreditCardType = scanResult.CardType.ToSharedCardType(),
                        CardNumber = scanResult.CardNumber,
                        Cvv = scanResult.Cvv,
                        Expiry = new DateTime(scanResult.ExpiryYear, scanResult.ExpiryMonth, 1),
                        PostalCode = scanResult.PostalCode,
                        Success = true
                    };
                }
                else
                    _currentScan._result = new CardIOResult { Success = false };

                _currentScan._finished = true;
            }
        }

        /// <inheritdoc/>
        public async Task<CardIOResult> Scan(CardIOConfig config = null)
        {
            if (config == null) 
                config = new CardIOConfig();

            _currentScan = this;

            if (_activity == null) 
                throw new InvalidOperationException("No Activity found!");

            Intent scanIntent = new Intent(_activity, typeof(CardIOActivity));

            scanIntent.PutExtra(CardIOActivity.ExtraRequireExpiry, config.RequireExpiry);
            scanIntent.PutExtra(CardIOActivity.ExtraRequireCvv, config.RequireCvv);
            scanIntent.PutExtra(CardIOActivity.ExtraRequirePostalCode, config.RequirePostalCode);
            scanIntent.PutExtra(CardIOActivity.ExtraUsePaypalActionbarIcon, config.ShowPaypalLogo);

            if (!string.IsNullOrEmpty(config.ScanInstructions))
                scanIntent.PutExtra(CardIOActivity.ExtraScanInstructions, config.ScanInstructions);
            if (!string.IsNullOrEmpty(config.Localization))
                scanIntent.PutExtra(CardIOActivity.ExtraLanguageOrLocale, config.Localization);

            Device.BeginInvokeOnMainThread(() =>
            {
                _activity.StartActivityForResult(scanIntent, ScanActivityResultCode);
            });

            while (!this._finished) 
                await Task.Delay(100);

            return this._result;
        }

    }
}