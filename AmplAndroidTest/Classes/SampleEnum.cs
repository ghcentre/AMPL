using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AmplAndroidTest.Classes
{
  internal enum SampleEnum
  {
    [Display(Name = "One", Description = "One Description")]
    One,
    [Display(Name = "Two", Description = "Two Description")]
    Two,
    [Display(Name = "Three", Description = "Three Description")]
    Three
  }
}