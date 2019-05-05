using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

namespace Binoculars.Export
{
    /// <summary>
    /// Here we are adding the custom Google Sheets connection and exporter
    /// </summary>
    
    //todo -> create a nice clean method of sending our Parameters to Google Sheets
    
    public static class GoogleSheets
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        public static void Execute(IList<IList<object>> list)
        {
            UserCredential credential;
            
            using (var stream = new FileStream(Settings.Paths.GoogleCredentialsPath, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "users",
                    CancellationToken.None,
                    new FileDataStore(Settings.Paths.GoogleCredentialsToken, true)).Result;
                Console.WriteLine("Credential file saved to: " + Settings.Paths.GoogleCredentialsToken);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            // todo -> fetch these from a config.json file or environment variables?
            
            // Define request parameters.
            var spreadsheetId = "1Jh3lTqwARCjsz_6Hn5ByS39791LYBxXd6ooTO8HsTqA";
            var spreadsheetTab = "Sheet1";

            // Here we define the sheet range
            var rng = string.Format("{0}!A1:A{1}", spreadsheetTab, list.Count);
            var vRange = new ValueRange
            {
                Range = rng,
                Values = list,
                MajorDimension = "ROWS"
            };

            // Send the request to the Google Sheets API
            var request = service.Spreadsheets.Values.Append(vRange, spreadsheetId, rng);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            request.Execute();
            
            // todo -> we need to check the request was actually sent successfully and gracefully deal with cases where the API is unavailable
        }
    }
}


