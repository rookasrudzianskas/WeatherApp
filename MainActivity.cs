using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using Android.Content;
using Android.Views;
using Android.Net;
using Android.Support.V4.Widget;
using System;

namespace Android_Weather_App_maybe_full
{
    [Activity(Label = "Android_Weather_App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        SwipeRefreshLayout refresh;
        Weather weatherData = null;
        TextView txtTemp;
        TextView txtTempMin;
        TextView txtTempMax;
        TextView txtDescr;
        TextView txtWindSpeed;
        TextView txtWindDeg;
        TextView txtHumidity;
        TextView txtPressure;
        TextView txtSunset;
        TextView txtSunrise;
        ImageView imgIcon;
        NetworkReceiver receiver;

        ConnectivityManager connMan;



        //for layout
        private void InitObjects()
        {
            ActionBar.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.WhiteSmoke));
            //change action bar title color

            refresh = FindViewById(Resource.Id.swipeL) as SwipeRefreshLayout;
            refresh.Refresh += delegate
            {
                LoadCurrentWeather();
            };

            receiver = new NetworkReceiver();

            receiver.NetworkChanged += delegate
            {
                if (CheckConnectivity())
                {
                    LoadCurrentWeather();
                }
                else
                {
                    refresh.Refreshing = false;
                }
            };

            RegisterReceiver(receiver, new IntentFilter(ConnectivityManager.ConnectivityAction));

            txtTemp = FindViewById(Resource.Id.temp) as TextView;
            txtTempMin = FindViewById(Resource.Id.tempMin) as TextView;
            txtTempMax = FindViewById(Resource.Id.tempMax) as TextView;
            txtDescr = FindViewById(Resource.Id.descrWeather) as TextView;
            txtWindSpeed = FindViewById(Resource.Id.windSpeed) as TextView;
            txtWindDeg = FindViewById(Resource.Id.windDeg) as TextView;
            txtHumidity = FindViewById(Resource.Id.humidity) as TextView;
            txtPressure = FindViewById(Resource.Id.pressure) as TextView;
            txtSunrise = FindViewById(Resource.Id.sunrise) as TextView;
            txtSunset = FindViewById(Resource.Id.sunset) as TextView;
            imgIcon = FindViewById(Resource.Id.imgWeather) as ImageView;

            connMan = GetSystemService(Context.ConnectivityService) as ConnectivityManager;
        }

        private bool CheckConnectivity()
        {
            foreach (var item in connMan.GetAllNetworkInfo())
            {
                if ((item.Type == ConnectivityType.Mobile || item.Type == ConnectivityType.Wifi) && item.IsConnected)
                {
                    return true;
                }
            }
            return false;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AppHelper.UpdateLocale(this);
            SetContentView(Resource.Layout.Main);
            InitObjects();
            if (!CheckConnectivity())
            {
                Toast.MakeText(this, "No connection", ToastLength.Short).Show();
                refresh.Refreshing = false;
                return;
            }
            LoadCurrentWeather();
        }

        private async void LoadCurrentWeather()
        {
            try
            {
                refresh.Refreshing = true;
                weatherData = await ApiHelper.GetCurrentWeather();
                if (weatherData == null)
                {
                    Toast.MakeText(this, "Error getting data", ToastLength.Short).Show();
                    return;
                }
                txtTemp.Text = weatherData.Temp.ToString();
                txtTempMin.Text = "Min.:" + weatherData.TempMin.ToString();
                txtTempMax.Text = "Max.:" + weatherData.TempMax.ToString();
                txtDescr.Text = string.Format("Country:{0}\nCity:{1}\nDescription:{2}\nCloudiness:{3}%", weatherData.Country, weatherData.City, weatherData.Descr, weatherData.Cloudiness);
                txtHumidity.Text = weatherData.Humidity + " %";
                txtPressure.Text = weatherData.Humidity + " hPa";
                txtWindSpeed.Text = "Speed:" + weatherData.WindSpeed;
                txtWindDeg.Text = "Direction:" + weatherData.WindDir;
                txtSunrise.Text = weatherData.Sunrise.ToString();
                txtSunset.Text = weatherData.Sunset.ToString();
                imgIcon.SetImageBitmap(weatherData.Icon);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error accured!", ToastLength.Short).Show();
            }
            finally
            {
                refresh.Refreshing = false;
            }
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_settings)
            {
                StartActivity(new Intent(this, typeof(PrefActivity)));
            }
            else if (item.ItemId == Resource.Id.action_refresh)
            {
                Toast.MakeText(this, "Refresh", ToastLength.Short).Show();
                LoadCurrentWeather();
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnPause()
        {
            if (receiver != null)
                UnregisterReceiver(receiver);
            base.OnPause();
        }
        protected override void OnDestroy()
        {
            if (receiver != null)
                UnregisterReceiver(receiver);
            base.OnDestroy();
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (receiver != null)
                RegisterReceiver(receiver, new IntentFilter(ConnectivityManager.ConnectivityAction));
        }


    }
}

