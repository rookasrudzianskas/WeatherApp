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
using Android.Preferences;

namespace Android_Weather_App_maybe_full
{
    [Activity(Label = "PrefActivity")]
    public class PrefActivity : PreferenceActivity
    {
        Preference city;
        Preference temp;
        Preference lang;
        ISharedPreferences prefs;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AppHelper.UpdateLocale(this);

            if(AppHelper.allCities == null || AppHelper.allCities.Count == 0)
            {
                AppHelper.LoadCities();
            }

            AddPreferencesFromResource(Resource.Layout.PrefLayout);
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            city = PreferenceScreen.FindPreference("city_key");
            temp = PreferenceScreen.FindPreference("temp_key");
            lang = PreferenceScreen.FindPreference("lang_key");

            city.Summary = GetString(Resource.String.current_pref) + prefs.GetString("city_key",GetString(Resource.String.default_city));
            temp.Summary = GetString(Resource.String.current_pref) + prefs.GetString("temp_key",GetString(Resource.String.default_temp));
            lang.Summary = GetString(Resource.String.current_pref) + prefs.GetString("lang_key",GetString(Resource.String.default_lang));

            temp.PreferenceChange += TempLang_PreferenceChange;
            lang.PreferenceChange += TempLang_PreferenceChange;
            city.PreferenceClick += City_PreferenceClick;
        }

        private void City_PreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(GetString(Resource.String.city_alert_title));
            alert.SetMessage(GetString(Resource.String.city_alert_message));
            View cityView = LayoutInflater.Inflate(Resource.Layout.CityLayout, null);
            AutoCompleteTextView cityName = cityView.FindViewById(Resource.Id.cityAutoComplete) as AutoCompleteTextView;
            cityName.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, AppHelper.allCities);
            alert.SetView(cityView);
            alert.SetPositiveButton(GetString(Resource.String.alert_ok), delegate {
                var editor = prefs.Edit();
                editor.PutString("city_key", cityName.Text);
                editor.Commit();
                city.Summary = GetString(Resource.String.current_pref) + prefs.GetString("city_key", GetString(Resource.String.default_city));
            });
            alert.SetNegativeButton(GetString(Resource.String.alert_cancel), delegate { alert.Dispose(); });
            alert.SetCancelable(false);
            alert.Show();
        }

        private void TempLang_PreferenceChange(object sender, Preference.PreferenceChangeEventArgs e)
        {
            var editor = prefs.Edit();

            if(e.Preference.Key=="temp_key")
            {
            editor.PutString("temp_key", e.NewValue.ToString());

                editor.Commit();
            }
            else if(e.Preference.Key == "lang_key")
            {
                editor.PutString("lang_key", e.NewValue.ToString());
                editor.Commit();
                AppHelper.UpdateLocale(this);
                this.Recreate();

            }
            temp.Summary = GetString(Resource.String.current_pref) + prefs.GetString("temp_key", GetString(Resource.String.default_temp));
            lang.Summary = GetString(Resource.String.current_pref) + prefs.GetString("lang_key", GetString(Resource.String.default_lang));
        }
    }
}