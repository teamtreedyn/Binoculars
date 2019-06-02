using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using Dynamo.Graph.Workspaces;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;

namespace Binoculars
{
    public static class Data
    {
        internal static string user;
        internal static string computerName;
        internal static string dynamoVersion;
        internal static string revitVersion;
        internal static string ip;
        internal static string latlng;
        internal static string city;
        internal static string country;
        internal static string filename;
        private static string date;
        private static string ID;

        /// <summary>
        /// The general data collecting method
        /// </summary>
        /// <param name="vlp">View Loaded Parameters from Dynamo</param>
        public static void Collect(ViewLoadedParams vlp)
        {
            // Dynamo StartupParams
            if((Boolean)Settings.collect["dynamoVersion"]) Data.dynamoVersion = vlp.StartupParams.DynamoVersion.ToString();

            // @todo Determine whether the script is run from DynamoPlayer..

            // Store the local environment data
            if((Boolean)Settings.collect["user"]) Data.user = Environment.UserName;
            if((Boolean)Settings.collect["computerName"]) Data.computerName = Environment.MachineName;

            if((Boolean)Settings.collect["revitVersion"])
            {
                var dynamoViewModel = vlp.DynamoWindow.DataContext as DynamoViewModel;

                // Add Revit data, if run from inside Revit
                // 10x Brendan Cassidy https://knowledge.autodesk.com/community/screencast/2f26aab4-bbdb-4935-84e1-bdd0e012a1dc
                if (dynamoViewModel.HostName.ToLower().Contains("revit"))
                {
                    Data.revitVersion = Utils.GetRevitData();
                }
            }

            if((Boolean)Settings.collect["geolocation"])
            {
                try
                {
                    // Request the IP and geolocation from ipinfo.io
                    WebClient client = new WebClient();
                    client.Headers.Set("Accept", "application/json");
                    var json = client.DownloadString("https://ipinfo.io");

                    // Parse the JSON
                    JObject ipinfo = JObject.Parse(json);

                    // Store geolocation data
                    if ((Boolean)Settings.collect["ip"]) Data.ip = (string)ipinfo["ip"];
                    if ((Boolean)Settings.collect["latlng"]) Data.latlng = (string)ipinfo["loc"];
                    if ((Boolean)Settings.collect["city"]) Data.city = (string)ipinfo["city"];
                    if ((Boolean)Settings.collect["country"]) Data.country = (string)ipinfo["country"];
                }

                catch (System.Net.WebException e) {
                    //Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
                    //Debug.AutoFlush = true;
                    //Debug.Indent();
                    Debug.WriteLine(string.Join("\n\n", new string[] {
                        "",
                        $"'{e}'",
                        "Connection to ipinfo.io failed.",
                        "Are you offline?",
                        e.Message,
                        "Continuing without collecting IP and geolocation data.",
                        ""
                    }));
                    //Debug.Unindent();
                }
            }
        }
        /// <summary>
        /// Record data from the Graph
        /// </summary>
        /// <param name="workspace">The Workspace - ie the Dynamo Graph</param>
        public static void Record(WorkspaceModel workspace)
        {
            if((Boolean)Settings.collect["filename"]) Data.filename = workspace.Name;
            Data.ID = workspace.Guid.ToString();
            // Data.filepath = workspace.FileName;
            // Data.evaluationCount = workspace.EvaluationCount;
            // Data.packages = workspace.Dependencies;
        }
        /// <summary>
        /// Generate the export message to be recorded in Google Sheets
        /// </summary>
        /// <returns></returns>
        internal static IList<IList<object>> Export()
        {
            IList<IList<object>> export = new List<IList<object>>();

            // Set the date
            // @todo "hh" is incorrectly returning values in the afternoon. It should be "14" not "2" etc for any hour after midday
            if((Boolean)Settings.collect["date"]) date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            // Create and return a list of all the Data strings
            var exportData = new List<object> { user, computerName, ip, latlng, city, country, dynamoVersion, revitVersion, filename, date, ID };

            export.Add(exportData);
            return export;
        }
    }
    /// <summary>
    /// Exports the collected data to Google Sheets
    /// </summary>
    public static class ExportSheets
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Binoculars Export to Google Sheets";

        public static void Execute(IList<IList<object>> list)
        {
            try
            {
                // Set some basic variable to initialise Google Sheets API
                string[] Scopes = { SheetsService.Scope.Spreadsheets };
                string serviceAccountEmail = (String)Settings.export["googleSheetsServiceAccount"]["client_email"];
                string key = (String)Settings.export["googleSheetsServiceAccount"]["private_key"];

                // Initialise the Google API ServiceAccount
                var initializer = new ServiceAccountCredential.Initializer(serviceAccountEmail)
                {
                    User = serviceAccountEmail,
                    Scopes = Scopes
                };

                // Set the ServiceAccount Key
                ServiceAccountCredential credential = new ServiceAccountCredential(initializer.FromPrivateKey(key));

                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define request parameters.
                String spreadsheetId = (string)Settings.export["googleSheets"]["id"];
                String spreadsheetTab = (string)Settings.export["googleSheets"]["sheet"];

                // Define the sheet range
                var rng = string.Format("{0}!A1:A{1}", spreadsheetTab, list.Count);
                var vRange = new ValueRange
                {
                    Range = rng,
                    Values = list,
                    MajorDimension = "ROWS"
                };

                // Send the request to the Google Sheets API
                var rqst = service.Spreadsheets.Values.Append(vRange, spreadsheetId, rng);
                rqst.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                rqst.Execute();
            }

            catch (System.Net.Http.HttpRequestException e)
            {
                Debug.WriteLine(string.Join("\n\n", new string[] {
                    "",
                    $"'{e}'",
                    "Connection to Google Sheets API failed.",
                    "Are you offline?",
                    e.Message,
                    "Continuing without exporting to Google Sheets.",
                    ""
                }));
            }

            catch (Google.GoogleApiException e)
            {
                if (e.Error.Code == 403)
                {
                    Debug.WriteLine(string.Join("\n\n", new string[] {
                        "",
                        $"'{e}'",
                        "Google Sheets API request was forbidden.",
                        "Have you enabled the Google Sheets API?.",
                        e.Error.Message,
                        "Continuing without exporting to Google Sheets.",
                        ""
                    }));
                }
                else if (e.Error.Code == 404)
                {
                    Debug.WriteLine(string.Join("\n\n", new string[] {
                        "",
                        $"'{e}'",
                        "The Google Sheet could not be found.",
                        "Check export.googleSheets.id is set correctly in settings.json",
                        e.Error.Message,
                        "Continuing without exporting to Google Sheets.",
                        ""
                    }));
                }
                else if (e.Error.Code == 400)
                {
                    Debug.WriteLine(string.Join("\n\n", new string[] {
                        "",
                        $"'{e}'",
                        "The data range or sheet/tab name within the Google Sheet could not be found.",
                        "Check export.googleSheets.sheet is correctly set to the sheet/tab name in settings.json",
                        "The default is usually 'Sheet1'",
                        e.Error.Message,
                        "Continuing without exporting to Google Sheets.",
                        ""
                    }));
                }
                else
                {
                    throw;
                }
            }

            catch (System.ArgumentException e)
            {
                if (e.ParamName == "pkcs8PrivateKey")
                {
                    Debug.WriteLine(string.Join("\n\n", new string[] {
                        "",
                        $"'{e}'",
                        "Invalid Google Service Account API Key.",
                        "Check export.googleSheetsServiceAccount.private_key is set correctly in settings.json",
                        e.Message,
                        "Continuing without exporting to Google Sheets.",
                        ""
                    }));
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
