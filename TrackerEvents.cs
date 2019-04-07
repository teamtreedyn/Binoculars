using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.Models;
using System;
using System.Windows;

namespace Tracker
{
    public static class TrackerEvents
    {
        private static ReadyParams DynamoReadyParams;
        private static DynamoModel Model;
        
        /// <summary>
        /// Registers custom events to be triggered when something happens in Dynamo.
        /// </summary>
        /// <param name="dynamoReadyParams">Reference to the Dynamo extension ready parameters.</param>
        public static void RegisterEventHandlers(ReadyParams dynamoReadyParams)
        {
            // Subscribe our event handler method to Dynamo
            dynamoReadyParams.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            
            // Keep a reference to the parameters supplied at startup
            // so we can un-register our event handlers later
            DynamoReadyParams = dynamoReadyParams;
        }

        /// <summary>
        /// Removes our custom event handler methods from Dynamo.
        /// </summary>
        public static void UnregisterEventHandlers()
        {
            // Unsubscribe our event handler methods
            DynamoReadyParams.CurrentWorkspaceChanged -= OnCurrentWorkspaceChanged;
            Model.RunCompleted -= OnGraphRun;
            Model.EvaluationCompleted -= OnEvaulationCompleted;
        }

        /// <summary>
        /// Event triggered whenever a Dynamo Workspace (file) is changed.
        /// </summary>
        /// <param name="obj">The current Dynamo workspace</param>
        private static void OnCurrentWorkspaceChanged(Dynamo.Graph.Workspaces.IWorkspaceModel obj)
        {
            // When the Dynamo Workspace is changed, set the filename
            ExportData.filename = obj.Name;

            // Display a MessageBox to the user
            // @todo Remove this as it's for testing purposes only
            string filename = ExportData.filename;
            MessageBox.Show($"Congratulations on opening the {filename} workspace!");
        }

        internal static void RegisterRunEventHandlers(DynamoModel model)
        {
            // Subscribe our event handler methods to Dynamo
            model.RunCompleted += OnGraphRun;
            model.EvaluationCompleted += OnEvaulationCompleted;

            // Set the dynamo model to Model so we can access it whenever required
            Model = model;
        }

        /// <summary>
        /// When the graph is evaluated .. ?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnEvaulationCompleted(object sender, EvaluationCompletedEventArgs e)
        {
            // Display a MessageBox to the user
            // @todo Remove this as it's for testing purposes only
            string filename = ExportData.filename;
            MessageBox.Show($"The current Graph name is {filename}");

            // When graph evaluation is complete
            // Get all the data we want to store and then store it
            var dataToExport = ExportData.Export();
            ExportSheets.Execute(dataToExport);
        }

        private static void OnGraphRun(object sender, bool success)
        {
            // DOES NOT WORK
            if (success)
            {
            }
        }
    }
}