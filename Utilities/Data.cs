using System;
using System.Collections.Generic;
using System.Net;
using Dynamo.Graph.Workspaces;
using Dynamo.Wpf.Extensions;
using Newtonsoft.Json.Linq;

namespace Binoculars.Utilities
{
    
    /// <summary>
    /// What Data is Binoculars collecting?
    /// </summary>
    
    public static class Data
    {
        internal static string User { get; set; }
        internal static string ComputerName { get; set; }
        internal static string DynamoVersion { get; set; }
        internal static string RevitVersion { get; set; }
        internal static string Ip { get; set; }
        internal static string LatLng { get; set; }
        internal static string City { get; set; }
        internal static string Country { get; set; }
        internal static string Filename { get; set; }
        internal static string RevitBuild { get; set; }
        private static string Date { get; set; }
        private static string Id { get; set; }
        
        
        public static void Collect(ViewLoadedParams vlp)
        {
            // todo ->  provide a visual clue to the user that we are in the process of gathering data/geolocating IP which is delaying startup..

            // Dynamo StartupParams
            DynamoVersion = vlp.StartupParams.DynamoVersion.ToString();

            // todo -> determine whether the script is run from DynamoPlayer..

            // Store the local environment data
            User = Environment.UserName;
            ComputerName = Environment.MachineName;

            // todo -> define the RevitVersion, leave blank or null if opened from any other environment (Sandbox, Civil3D etc)
            RevitVersion = "2019.0.2";

            // Request the IP and geolocation from ipinfo.io
            var client = new WebClient();
            client.Headers.Set("Accept", "application/json");
            var json = client.DownloadString("https://ipinfo.io");
            // todo ->  we need to check the request was actually sent successfully and gracefully deal with cases where the API is unavailable

            // Parse the JSON
            var ipInfo = JObject.Parse(json);
            
            //todo -> I've broken this somehow! Sorry everyone!
            // Store geolocation data
            Ip = (string)ipInfo["Ip"];
            LatLng = (string)ipInfo["loc"];
            City = (string)ipInfo["City"];
            Country = (string)ipInfo["Country"];
        }

        /// <summary>
        /// Record data from the Graph
        /// </summary>
        
        public static void Record(WorkspaceModel workspace)
        {
            Filename = workspace.Name;
            Id = workspace.Guid.ToString();
            //DynamoFilePath = workspace.FileName;
            //EvaluationCount = workspace.EvaluationCount;
            //Packages = workspace.Dependencies;
            //RevitFilePath = ?
        }

        /// <summary>
        /// Generate the export message to be recorded in Google Sheets
        /// </summary>

        internal static IList<IList<object>> Export()
        {
            Date = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK");
            IList<IList<object>> export = new List<IList<object>>();

            // Create and return a list of all the Data strings
            var exportData = new List<object>
                {User, ComputerName, Ip, LatLng, City, Country, DynamoVersion, RevitVersion, Filename, Date, Id};

            // Add Revit information if it exists
            if (!string.IsNullOrEmpty(RevitBuild))
                exportData.Add(RevitBuild);

            export.Add(exportData);
            return export;
        }

    }

}