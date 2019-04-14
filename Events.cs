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

            // Display a MessageBox to the user
            // @todo Remove this as it's for testing purposes only
            string filename = Data.filename;
            MessageBox.Show($"The current Graph name is {filename}");

            // When graph evaluation is complete
            // Get all the data we want to store and then store it
            var dataToExport = Data.Export();
            ExportSheets.Execute(dataToExport);
        }

        private static void OnGraphRun(object sender, bool success)
        {
            // DOES NOT WORK
            if (success)
            {
            }
        }

        internal static void Register(DynamoModel model)
        {
            // Subscribe our event handler methods to Dynamo
            model.RunCompleted += OnGraphRun;
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
            Model.RunCompleted -= OnGraphRun;
            Model.EvaluationCompleted -= OnEvaulationCompleted;
        }
    }
}