using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Preferences;
using Android.Content.Res;
using Android.Util;

namespace Android_Weather_App_maybe_full
{
    class AppHelper
    {
        public static List<string> allCities;
        public static string DefaultLang;
        public static Resources res = Application.Context.Resources;
        public static Configuration conf = new Configuration();
        public static DisplayMetrics disp;
        static ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
        public static void LoadCities()
        {
            try {
                using (StreamReader reader = new StreamReader(Application.Context.Resources.Assets.Open("city_list.txt")))
                {
                    string[] line = new string[5];
                    allCities = new List<string>();
                    while(!reader.EndOfStream)
                    {
                        line = reader.ReadLine().Split("\t");
                        allCities.Add(line[1] + "," + line[4]);
                    }
                }

            }
            catch(Exception ex)
            {

            }
        }

        public static void UpdateLocale(Context context)
        {
            DefaultLang = prefs.GetString("lang_key", context.GetString(Resource.String.default_lang));
            res = context.Resources;
            conf = res.Configuration;
            disp = res.DisplayMetrics;
            if (DefaultLang == "en")
            { 
            conf.SetLocale(new Java.Util.Locale("en"));
            res.UpdateConfiguration(conf, disp);
            }
            else
            {
                conf.SetLocale(new Java.Util.Locale("lt"));
                res.UpdateConfiguration(conf, disp);
            }
        }

    }
}