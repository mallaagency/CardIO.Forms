# CardIO.Forms

CardIO.Forms is a library for Xamarin.Forms Android and iOS that can scan credit card details from the device's camera similar to a barcode scanner.  CardIO.Form makes scanning and entering Credit Card information simple.

>> CardIO.Forms is an implementation of the old Xamarin Components CardIO binding native libraries for Android and iOS. **Use with your own risk.**

## Features

- **Live Camera Card Scanning** - Quick and simple integration.  Get up and running in 5 minutes.
- **Optional Manual Entry** - Users can choose to type their credit cards in.  Card.io provides a slick interface for manual card entry.
- **Secure** - No credit card information is stored or transmitted.
- **Free** - SDK's are free for both Android and iOS.

## Adding CardIO.Forms to your proyect

To get your project working some setup is needed. You will need to initialize the library on each platform in your platform-specific app project.

### Android

```c#
...
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Malla.CardIO.Android.CardIO.Initialize(this);

            LoadApplication(new App());
        }
        
        protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Malla.CardIO.Android.CardIO.ForwardActivityResult(requestCode, resultCode, data);
        }
    }
...
```

### iOS

```c#
//code here
```

## Usage

```c#
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
```

## Learn More

Learn more about Card.io by visiting http://www.card.io