using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Android_Weather_App_maybe_full
{
    class Weather
    {
        public string Temp { get; set; }
        public string TempMin { get; set; }
        public string TempMax { get; set; }
        public string Descr { get; set; }
        public short Pressure { get; set; }
        public byte Humidity { get; set; }
        public string WindSpeed { get; set; }
        public float WindDeg { get; set; }
        public string WindDir { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public byte Cloudiness { get; set; }
        public Bitmap Icon { get; set; }
    }
}