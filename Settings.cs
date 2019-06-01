using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace Binoculars
{
    public static class Settings
    {
        internal static JToken consent;
        internal static JToken collect;
        internal static JToken dashboard;
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
                Settings.dashboard = jobj["dashboard"];
                Settings.export = jobj["export"];
            }

            // If using Google Sheets and its value is the default placeholder then notify the user
            if ((String)Settings.export["method"] == "googleSheets" && (String)Settings.export["googleSheets"]["id"] == "YOUR GOOGLE SHEETS ID")
            {
                // @todo Use a view framework to improve the UI/UX
                // @todo Add a button linking to the Getting Started guide
                // @todo Add a button to open the settings.json file
                string[] messages = new string[] {
                    $"It looks like you haven't configured Binoculars yet!",
                    $"It's easy to do, you just need to change a few variables in the settings.json file which you'll find in the following location:",
                    path
                };
                string title = "Binoculars";
                MessageBox.Show(string.Join("\n\n", messages), title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }

}
