using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Dynamo.Graph.Workspaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Tracker
{
    public static class ExportData
    {
        private static string user;
        private static string computerName;
        private static string geo_x;
        private static string geo_y;
        private static string dynamoVersion;
        private static string revitVersion;
        private static string ip;
        private static string filename;
        private static string date;

        internal static IList<IList<object>> Export(string filename, string dynamoversion)
        {
            IList<IList<object>> export = new List<IList<object>>();

            user = Environment.MachineName;
            computerName = Environment.UserName;
            date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            WebClient client = new WebClient();
            client.Headers.Set("Accept", "application/json");
            var json = client.DownloadString("https://ipinfo.io");

            JObject ipinfo = JObject.Parse(json);

            string ip = Regex.Replace((string)ipinfo["ip"], @"\s+", "");
            string latlng = (string)ipinfo["loc"];
            string city = (string)ipinfo["city"];
            string country = (string)ipinfo["country"];

            revitVersion = "2019.0.2";

            export.Add(new List<object> { user, computerName, ip, latlng, city, country, dynamoversion, revitVersion, filename, date } );

            return export;
        }
    }

    public static class ExportSheets
    {        
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        public static void Execute(IList<IList<object>> list)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "users",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1-NNRsKhonKzNmTrl2H3IfwIZvGTy0HMs6AvXhsw-nUc";

            String range = "Test Data!A2:E";
            //SpreadsheetsResource.ValuesResource.GetRequest request =
            //        service.Spreadsheets.Values.Get(spreadsheetId, range);

            //var list1 = new List<object> { "Hi" };
            //var list2 = new List<object> { "How are you?" };

            //IList<IList<object>> list = new List<IList<object>> { list1, list2 };

            var rng = string.Format("{0}!A1:A{1}", "Test Data", list.Count);
            var vRange = new ValueRange
            {
                Range = rng,
                Values = list,
                MajorDimension = "ROWS"
            };

            var rqst = service.Spreadsheets.Values.Append(vRange, spreadsheetId, rng);
            rqst.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            rqst.Execute();

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            //ValueRange response = request.Execute();
            //IList<IList<Object>> values = response.Values;
            //if (values != null && values.Count > 0)
            //{
            //    Console.WriteLine("Name, Major");
            //    foreach (var row in values)
            //    {
            //        // Print columns A and E, which correspond to indices 0 and 4.
            //        Console.WriteLine("{0}, {1}", row[0], row[4]);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No data found.");
            //}
            //Console.Read();
        }
    }
}
