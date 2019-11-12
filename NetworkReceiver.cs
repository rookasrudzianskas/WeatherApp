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

namespace Android_Weather_App_maybe_full
{
    [BroadcastReceiver]
    public class NetworkReceiver : BroadcastReceiver
    {
        public event Action<Context, Intent> NetworkChanged;
        //get network connection change event
        string eventName = Android.Net.ConnectivityManager.ConnectivityAction;
        public override void OnReceive(Context context, Intent intent)
        {
            if(context!=null && intent !=null && intent.Action==eventName)
            {
                this.NetworkChanged(context, intent);
            }
        }
    }
}