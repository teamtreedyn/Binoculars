using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Binoculars.Utilities;

namespace Binoculars.Extension
{
    /// <summary>
    /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
    /// </summary>
    
    
    public class ViewExtension : IViewExtension
    {

        public string UniqueId => "4DB6C8D9-7D8E-42A8-8995-E14ACFA037CF";
        public string Name => "Binoculars View Extension";
        readonly string _userName = Environment.UserName;
        private MenuItem _extensionMenu;
        private ViewLoadedParams _viewLoadedParams;
        private DynamoViewModel DynamoViewModel => _viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;

        
        
        /// <summary>
        /// Method that is called when Dynamo starts, but is not yet ready to be used.
        /// </summary>
        /// <param name="vsp">Parameters that provide references to Dynamo settings, version and extension manager.</param>
        
        public void Startup(ViewStartupParams vsp)
        {
        }

        /// <summary>
        /// Method that is called when Dynamo has finished loading and the UI is ready to be interacted with.
        /// </summary>
        /// <param name="vlp">
        /// Parameters that provide references to Dynamo commands, settings, events and
        /// Dynamo UI items like menus or the background preview. This object is supplied by Dynamo itself.
        /// </param>
        
        public void Loaded(ViewLoadedParams vlp)
        {
            // Collect the environment variables
            Data.Collect(vlp);

            // hold a reference to the Dynamo params to be used later
            _viewLoadedParams = vlp;
            var dynamoViewModel = vlp.DynamoWindow.DataContext as DynamoViewModel;
            
            
            // we can register our own events that will be triggered when specific things happen in Dynamo
            // a reference to the ReadyParams is needed to do this, so we pass it on
            Events.Register(dynamoViewModel.Model);

            // Add Revit data, if run from inside Revit
            // 10x Brendan Cassidy https://knowledge.autodesk.com/community/screencast/2f26aab4-bbdb-4935-84e1-bdd0e012a1dc
            if (dynamoViewModel.HostName.ToLower().Contains("revit"))
            {
                Data.RevitBuild = Utils.GetRevitData();
            }

            // we can now add custom menu items to Dynamo's UI
            BinocularsMenuItems();
        }

        /// <summary>
        /// Here we are adding the custom extension menu items to the Dynamo UI
        /// </summary>
        

        private void BinocularsMenuItems()
        {
            // extension top-level new menu item:
            _extensionMenu = new MenuItem {Header = "Binoculars 🔍"};

            // sub-menu item 'About': 
            var aboutMenuItem = new MenuItem {Header = " ❓ About"};
            aboutMenuItem.Click += (sender, args) =>

            {
                // in this we generate an 'about' message box for the user to read:
                MessageBox.Show(
                    "Hello " + Format.ToTitleCase(_userName) + "👋🏻\n\nWe at Binoculars just " +
                                "want to let you know that collecting user data is common practice in modern websites and applications as a way " +
                                "of providing creators with more information to make decisions and create better experiences. \n\nAmong other benefits, " +
                                "data can be used to help tailor content, drive product direction, and provide insight into problems in current " +
                                "implementations. Collecting relevant information and using it wisely can give organizations an edge " +
                                "over competitors and increase the impact of limited resources. \n\nKind Regards,\n\n" +
                                "All the Team @ Binoculars.");
            };
            
            // sub-menu item 'Data':
            var dataMenuItem = new MenuItem {Header = "☁ Data" };
            dataMenuItem.Click += (sender, args) => 
                { Process.Start("https://datastudio.google.com/s/jfnD88Nn6mA"); };

            // sub-menu item 'Contribute':
            var contributeMenuItem = new MenuItem {Header = "💡 Contribute" };
            contributeMenuItem.Click += (sender, args) => 
                { Process.Start("https://github.com/teamtreedyn/Binoculars"); };
            
            // add all Binoculars menu items to menu: 
            _extensionMenu.Items.Add(dataMenuItem);
            _extensionMenu.Items.Add(contributeMenuItem);
            _extensionMenu.Items.Add(aboutMenuItem);
            
            
            // finally, we need to add the extension menu to Dynamo
            _viewLoadedParams.dynamoMenu.Items.Add(_extensionMenu);
        }

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown()
        {
            Events.Unregister();
        }

        public void Dispose()
        {
        }
    }
}