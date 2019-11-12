using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using Android.Graphics;
using Android.Preferences;

namespace Android_Weather_App_maybe_full
{
    class ApiHelper
    {
        public static async Task<Weather> GetCurrentWeather()
        {
            Weather weatherData = null;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            string units = prefs.GetString("temp_key", Application.Context.GetString(Resource.String.default_temp));
            string city = prefs.GetString("city_key", Application.Context.GetString(Resource.String.default_city));
            string lang = prefs.GetString("lang_key", Application.Context.GetString(Resource.String.default_lang));
            string key = "a2be6c9cc5748c28c6b333f0f65c3b00";
            string source = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units={2}&lang={3}",city ,key, units, lang);

            JContainer data = await HttpHelper.GetJsonData(source);

            //check if data present
            if (data == null) return null;
            Console.WriteLine("=====================");
            Console.WriteLine(data);
            Console.WriteLine("=====================");
            //create weather object
            weatherData = new Weather();
            //get data from jcontainer
            weatherData.City = data["name"].ToString();
            weatherData.Cloudiness = (byte)data["clouds"]["all"];
            weatherData.Country = data["sys"]["country"].ToString();
            weatherData.Descr = data["weather"][0]["description"].ToString();
            weatherData.Humidity = (byte)data["main"]["humidity"];
            weatherData.Pressure = (short)data["main"]["pressure"];
            weatherData.Sunrise = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds((double)data["sys"]["sunrise"]).ToLocalTime();

            weatherData.Sunset = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds((double)data["sys"]["sunset"]).ToLocalTime();

            string tempT;
            string tempW;
            if(units == "metric")
            {
                tempT = " °C";
                tempW = "m/s";
                
            } else if( units == "Imperial")
            {
                tempT = " F";
                tempW = "mph";
            } else
            {
                tempT = " K";
                tempW = " m/s";
            }






            weatherData.Temp = (int)data["main"]["temp"] + tempT;
            weatherData.TempMin = (int)data["main"]["temp_min"] + tempT;
            weatherData.TempMax = (int)data["main"]["temp_max"] + tempT;
            weatherData.WindDeg = (float)data["wind"]["deg"];
            weatherData.WindDir = WindToDirection(weatherData.WindDeg);
            weatherData.WindSpeed = (float)data["wind"]["speed"] + tempW;


            string imgURL = "http://openweathermap.org/img/w/" + data["weather"][0]["icon"]+".png";

            Task tsk = Task.Run(() =>
            {
            weatherData.Icon = BitmapFactory.DecodeStream(new Java.Net.URL(imgURL).OpenStream());
            });
            tsk.Wait();

            return weatherData;
        }

        private static string WindToDirection(float deg)
        {
            if ((deg >= 348.75 && deg < 360) || (deg >= 0 && deg < 11.25))
                return "N";
            else if ((deg >= 11.25 && deg < 33.75))
                return "NNE";
            else if ((deg >= 33.75 && deg < 56.25))
                return "NE";
            else if ((deg >= 56.25 && deg < 78.75))
                return "ENE";
            else if ((deg >= 78.75 && deg < 101.25))
                return "E";
            else if ((deg >= 101.25 && deg < 123.75))
                return "ESE";
            else if ((deg >= 123.75 && deg < 146.25))
                return "SE";
            else if ((deg >= 146.25 && deg < 168.75))
                return "SSE";
            else if ((deg >= 168.75 && deg < 191.25))
                return "S";
            else if ((deg >= 191.25 && deg < 213.75))
                return "SSW";
            else if ((deg >= 213.75 && deg < 236.25))
                return "SW";
            else if ((deg >= 236.25 && deg < 258.75))
                return "WSW";
            else if ((deg >= 258.75 && deg < 281.25))
                return "W";
            else if ((deg >= 281.25 && deg < 303.75))
                return "WNW";
            else if ((deg >= 303.75 && deg < 326.25))
                return "NW";
            else if ((deg >= 326.25 && deg < 348.75))
                return "NNW";
            return "";
        }
    }
}