using Android.App;
using Android.Widget;
using Android.OS;
using AmplAndroidTest.Classes;
using Ampl.System;

namespace AmplAndroidTest
{
  [Activity(Label = "AmplAndroidTest", MainLauncher = true, Icon = "@drawable/icon")]
  public class MainActivity : Activity
  {
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.Main);

      SampleEnum sampleEnum = SampleEnum.Two;
      FindViewById<TextView>(Resource.Id.EnumTextView).Text =
        $"{sampleEnum.GetDisplayName()} {sampleEnum.GetDisplayDescription()}";
    }
  }
}

