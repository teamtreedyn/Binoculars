using Dynamo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace Tracker
{
    /// <summary>
    /// Dynamo extension that controls the underlying Dynamo application but not its UI.
    /// </summary>
    public class TrackerExtension : IExtension
    {
        public string UniqueId => "3B234622-43B7-4EA8-86DA-54FB390BE29E";

        public string Name => "Binoculars Extension";

        public string DynamoVersion;

        /// <summary>
        /// Method that is called when Dynamo starts, but is not yet ready to be used.
        /// </summary>
        /// <param name="sp">Parameters that provide references to Dynamo settings and version.</param>
        public void Startup(StartupParams sp)
        {
            // On startup event
        }

        /// <summary>
        /// Method that is called when Dynamo has finished loading and is ready to be used.
        /// </summary>
        /// <param name="rp">
        /// Parameters that provide references to Dynamo commands, settings and events.
        /// This object is supplied by Dynamo itself.
        /// </param>
        public void Ready(ReadyParams rp)
        {
            // @todo Subscribe to an event after the Dynamo home screen is shown

            // Display a MessageBox to the user
            // @todo Use a view framework to improve the UI/UX
            string message = "By pressing OK you agreeing to Binoculars 🔍 data collection.";
            string title = "Terms of Use Agreement";
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);

            // @todo if user does not consent, don't store..
            if (true) {

                // Dynamo StartupParams
                ExportData.dynamoVersion = rp.StartupParams.DynamoVersion.ToString();

                // @todo Determine whether the script is run from DynamoPlayer..

                // Store the local environment data
                ExportData.user = Environment.MachineName;
                ExportData.computerName = Environment.UserName;
                
                // @todo Define the revitVersion, leave blank or null if opened from any other environment (Sandbox, Civil3D etc)
                ExportData.revitVersion = "2019.0.2";

                // Request the IP and geolocation from ipinfo.io
                WebClient client = new WebClient();
                client.Headers.Set("Accept", "application/json");
                var json = client.DownloadString("https://ipinfo.io");
                // @todo We need to check the request was actually sent successfully and gracefully deal with cases where the API is unavailable

                // Parse the JSON
                JObject ipinfo = JObject.Parse(json);

                // Store geolocation data
                ExportData.ip = (string)ipinfo["ip"];
                ExportData.latlng = (string)ipinfo["loc"];
                ExportData.city = (string)ipinfo["city"];
                ExportData.country = (string)ipinfo["country"];
            }
            
            // we can register our own events that will be triggered when specific things happen in Dynamo
            // a reference to the ReadyParams is needed to do this, so we pass it on
            TrackerEvents.RegisterEventHandlers(rp);
        }

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown()
        {
            TrackerEvents.UnregisterEventHandlers();
        }

        public void Dispose()
        {
        }
    }
}