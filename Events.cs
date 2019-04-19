using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.Models;
using System;
using System.Windows;

namespace Binoculars
{
    public static class Events
    {
        private static DynamoModel Model;

        /// <summary>
        /// When the graph is evaluated .. ?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnEvaulationCompleted(object sender, EvaluationCompletedEventArgs e)
        {
            // Record Run Data
            Data.Record(Model.CurrentWorkspace);

            // If running in DEBUG mode display a MessageBox
            #if DEBUG
                string[] message = new string[] {
                    $"Hi {Data.user},",
                    $"You're currently running {Data.filename} on {Data.computerName} from {Data.city}, {Data.country} using Dynamo {Data.dynamoVersion}."
                };
                string title = "Binoculars";
                MessageBox.Show(string.Join("\n\n", message), title);
            #endif

            // When graph evaluation is complete
            // Get all the data we want to store and then store it
            var dataToExport = Data.Export();
            ExportSheets.Execute(dataToExport);

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