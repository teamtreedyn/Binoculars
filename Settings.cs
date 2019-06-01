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
            // Build the path
            string assembly = Utils.AssemblyDirectory;
            string path = Path.GetFullPath(Path.Combine(assembly, @"..\", "settings.json"));

            using (StreamReader r = new StreamReader(path))
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
