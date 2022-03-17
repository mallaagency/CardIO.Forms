[![card.io logo](https://raw.githubusercontent.com/mallaagency/CardIO.Forms/main/assets/icon.png "CardIO.Forms")](https://malla.agency)

# CardIO.Forms

[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](pull/new/master) [![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-brightgreen.svg?style=flat-square)](graphs/commit-activity) [![Open Source Love png1](https://badges.frapsoft.com/os/v1/open-source.png?v=103)](#contribution) [![licence](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE)

CardIO.Forms is a library for Xamarin.Forms Android and iOS that can scan credit card details from the device's camera similar to a barcode scanner.  CardIO.Form makes scanning and entering Credit Card information simple.

> CardIO.Forms is an implementation of the old Xamarin Components CardIO binding native libraries for Android and iOS. **Use with your own risk.**

[![card.io logo](https://raw.githubusercontent.com/mallaagency/CardIO.Forms/main/assets/card.io_sample.png "CardIO.Forms")](https://malla.agency)

## Features

- **Live Camera Card Scanning** - Quick and simple integration.  Get up and running in 5 minutes.
- **Optional Manual Entry** - Users can choose to type their credit cards in.  Card.io provides a slick interface for manual card entry.
- **Secure** - No credit card information is stored or transmitted.
- **Free** - SDK's are free for both Android and iOS.

## Adding CardIO.Forms to your proyect

To get your project working some setup is needed.

1. Install the package available on Nuget on each platform-specifi project.
2. Initialize the library on each platform in your platform-specific app project.

### NuGet

- [![NuGet](https://img.shields.io/nuget/v/Malla.CardIO.Forms.svg?label=NuGet)](https://www.nuget.org/packages/Malla.CardIO.Forms/)
- ![Build status](https://img.shields.io/badge/build-succeded-brightgreen.svg)

#### Platform Support

|Platform|Version|
| ------------------- | :-----------: |
|.NET Standard|2.0+|
|Xamarin.Forms|4.4+|
|MonoAndroid|9.0+|
|Xamarin.iOS|1.0+|

### Android

You will need to add permissions that card.io requires to your `AndroidManifest.xml` file.  

```xml
<manifest ... >
	<uses-permission android:name="android.permission.INTERNET" />
	<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
	<uses-permission android:name="android.permission.CAMERA" />
	<uses-permission android:name="android.permission.FLASHLIGHT" />
	<uses-permission android:name="android.permission.VIBRATE" />

	<!-- Camera features - recommended -->
	<uses-feature android:name="android.hardware.camera" android:required="false" />
	<uses-feature android:name="android.hardware.camera.autofocus" android:required="false" />
	<uses-feature android:name="android.hardware.camera.flash" android:required="false" />
</manifest>
```

Or you can add the following C# code to your project (can be in the `AssemblyInfo.cs` file):

```c#
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.Camera)]
[assembly: UsesPermission (Android.Manifest.Permission.Flashlight)]
[assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]
```

You should also declare some of the features your app is using.  Again you can do this directly in the manifest, or by adding this C# code to your project:

```c#
[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]
[assembly: UsesFeature("android.hardware.camera.flash", Required = false)]
```

On your `MainActivity` add initialization code:

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

Before you build in **release mode**, make sure to adjust your proguard configuration by adding the following code to your `proguard.cfg` file:

```cfg
# Don't obfuscate DetectionInfo or public fields, since
# it is used by native methods
-keep class io.card.payment.DetectionInfo
-keepclassmembers class io.card.payment.DetectionInfo {
    public *;
}

-keep class io.card.payment.CreditCard
-keep class io.card.payment.CreditCard$1
-keepclassmembers class io.card.payment.CreditCard {
  *;
}

-keepclassmembers class io.card.payment.CardScanner {
  *** onEdgeUpdate(...);
}

# Don't mess with classes with native methods

-keepclasseswithmembers class * {
    native <methods>;
}

-keepclasseswithmembernames class * {
    native <methods>;
}

-keep public class io.card.payment.* {
    public protected *;
}

# required to suppress errors when building on android 22
-dontwarn io.card.payment.CardIOActivity
```

### iOS

Just to prevent the linker from removing the assembly on your `AppDelegate.cs` file add initialization code.

```c#
...
public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
{
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        global::Xamarin.Forms.Forms.Init();
        Malla.CardIO.iOS.CardIO.Init();

        LoadApplication(new App());

        return base.FinishedLaunching(app, options);
    }
}
...
```

Starting in iOS 10, Apple requires that if your app wants to access the camera, your `Info.plist` must specify the reason you wish to access it, which will be displayed to the user when they are prompted to allow or deny your app permission to use the camera.

This means your Info.plist file should contain a string item with a key of `NSCameraUsageDescription` and a value containing a description of why you want to use the camera, to show the user.

Open the `Info.plist` file as source code and add this:

```xml
<key>NSCameraUsageDescription</key>
<string>Camera usage description</string>
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

Learn more about Card.io by visiting [http://www.card.io](http://www.card.io)
