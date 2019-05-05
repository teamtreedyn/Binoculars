using Dynamo.Models;
using System.Windows;
using Binoculars.Export;
using Binoculars.Utilities;

namespace Binoculars.Extension
{
    public static class Events
    {
        public static DynamoModel Model;

        /// <summary>
        /// When the graph is evaluated .. ?
        /// </summary>


        private static void OnEvaulationCompleted(object sender, EvaluationCompletedEventArgs e)
        {
            // Record Run Data
            Data.Record(Model.CurrentWorkspace);

            // If running in DEBUG mode display a MessageBox
            #if DEBUG
                string[] message = new string[] {
                    $"Hello {Data.User},",
                    $"You're currently running {Data.Filename} on {Data.ComputerName} from {Data.City}, {Data.Country} using Dynamo {Data.DynamoVersion}."
                };
                const string title = "DEBUG Binoculars 🔍";
                MessageBox.Show(string.Join("\n\n", message), title);
            #endif

            // When graph evaluation is complete
            // Get all the data we want to store and then store it
            var dataToExport = Data.Export();
            GoogleSheets.Execute(dataToExport);

        }

        internal static void Register(DynamoModel model)
        {
            // Subscribe our event handler methods to Dynamo
            model.EvaluationCompleted += OnEvaulationCompleted;

            // Set the dynamo model to Model so we can access it whenever required
            Model = model;
        }

        /// <summary>
        /// Removes our custom event handler methods from Dynamo.
        /// </summary>
        public static void Unregister()
        {
            // Unsubscribe our event handler methods
            Model.EvaluationCompleted -= OnEvaulationCompleted;
        }
    }
}