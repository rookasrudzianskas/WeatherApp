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
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Android_Weather_App_maybe_full
{
    class HttpHelper
    {
        
        public static async Task<JContainer> GetJsonData(string source)
        {
            JContainer data = null;
            HttpClient client = new HttpClient();
            string response  = await client.GetStringAsync(source);
            if(response!=null)
            {
                //convert response string to jcontainer
                data = JsonConvert.DeserializeObject(response) as JContainer;
            }
            return data;
        }
    }
}