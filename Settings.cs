using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Binoculars
{
    public static class Settings
    {
        internal static JToken consent;
        internal static JToken collect;
        internal static JToken export;

        public static void Load()
        {
            using (StreamReader r = new StreamReader("settings.json"))
            {
                string json = r.ReadToEnd();
                JObject jobj = JObject.Parse(json);
                Settings.consent = jobj["consent"];
                Settings.collect = jobj["collect"];
                Settings.export = jobj["export"];
            }
        }
    }

}
