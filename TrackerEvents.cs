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
        private static string fileName;
        public static string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                if(fileName != value)
                {
                    fileName = value;
                }
            }
        }

        internal static Version DynamoVersion;

        /// <summary>
        /// Registers custom events to be triggered when something happens in Dynamo.
        /// </summary>
        /// <param name="dynamoReadyParams">Reference to the Dynamo extension ready parameters.</param>
        public static void RegisterEventHandlers(ReadyParams dynamoReadyParams)
        {
            dynamoReadyParams.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            
            // keep a reference to the parameters supplied at startup
            // so we can un-register our event handlers later

            DynamoReadyParams = dynamoReadyParams;
        }

        /// <summary>
        /// Removes our custom events from Dynamo.
        /// </summary>
        public static void UnregisterEventHandlers()
        {
            DynamoReadyParams.CurrentWorkspaceChanged -= OnCurrentWorkspaceChanged;
            Model.RunCompleted -= OnGraphRun;
        }

        /// <summary>
        /// Event triggered whenever a Dynamo Workspace (file) is changed.
        /// </summary>
        /// <param name="obj">The current Dynamo workspace</param>
        private static void OnCurrentWorkspaceChanged(Dynamo.Graph.Workspaces.IWorkspaceModel obj)
        {
            FileName = DynamoReadyParams.CurrentWorkspaceModel.FileName;

            MessageBox.Show($"Congratulations on opening the {obj.Name} workspace!");
            
            string DynamoVersion = string.Empty;
            MessageBox.Show($"The current Graph name is {FileName}");

            var dataToExport = ExportData.Export(FileName, DynamoVersion);
            ExportSheets.Execute(dataToExport);


        }

        internal static void RegisterRunEventHandlers(DynamoModel model)
        {
            model.RunCompleted += OnGraphRun;

            // Set the Model - priceless!
            Model = model;
        }

        private static void OnGraphRun(object sender, bool success)
        {
            // WE HOOK UP HERE 
            if (success)
            {
                string DynamoVersion = string.Empty;
                MessageBox.Show($"The current Graph name is {FileName}");

                var dataToExport = ExportData.Export(FileName, DynamoVersion);
                ExportSheets.Execute(dataToExport);

            }
        }
    }
}