using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Tracker
{
    /// <summary>
    /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
    /// </summary>
    public class TrackerViewExtension : IViewExtension
    {
        public string UniqueId => "4DB6C8D9-7D8E-42A8-8995-E14ACFA037CF";
        public string Name => "Binoculars View Extension";


        private string UserName = Environment.UserName;

        private MenuItem _extensionMenu;
        private ViewLoadedParams _viewLoadedParams;
        private DynamoViewModel _dynamoViewModel => _viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;
        
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
            // hold a reference to the Dynamo params to be used later
            _viewLoadedParams = vlp;
            
            // we can register our own events that will be triggered when specific things happen in Dynamo
            // a reference to the ReadyParams is needed to do this, so we pass it on
            TrackerEvents.RegisterRunEventHandlers((vlp.DynamoWindow.DataContext as DynamoViewModel).Model);

            // we can now add custom menu items to Dynamo's UI
            TrackerMenuItems();
        }

        /// <summary>
        /// Adds custom menu items to the Dynamo menu
        /// </summary>
        
        public string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public void TrackerMenuItems()
        {
            // let's now create a completely top-level new menu item
            _extensionMenu = new MenuItem {Header = "Binoculars 🔍" };

            // and now we add a new sub-menu item that says hello when clicked
            var sayHelloMenuItem = new MenuItem {Header = " ❓ About"};
            sayHelloMenuItem.Click += (sender, args) =>
            {
                // Display a MessageBox to the user
                // @todo Use a view framework to improve the UI/UX
                MessageBox.Show("Hello " + ToTitleCase(UserName) + "👋🏻\n\nWe at Binoculars just want to let you know that collecting user data is common practice in modern websites and applications as a way of providing creators with more information to make decisions and create better experiences. \n\nAmong other benefits, data can be used to help tailor content, drive product direction, and provide insight into problems in current implementations. Collecting relevant information and using it wisely can give organizations an edge over competitors and increase the impact of limited resources. \n\nKind Regards,\n\nAll the Team @ Binoculars.");
            };


            // now make a hackathon worthy menu item
            var hackMenuItem = new MenuItem {Header = "☁ Data" };
            hackMenuItem.Click += (sender, args) => { Process.Start("https://datastudio.google.com/s/jfnD88Nn6mA"); };

            // add all menu items to menu
            
            _extensionMenu.Items.Add(hackMenuItem);
            _extensionMenu.Items.Add(sayHelloMenuItem);

            // finally, we need to add our menu to Dynamo
            _viewLoadedParams.dynamoMenu.Items.Add(_extensionMenu);
        }

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown()
        {
        }

        public void Dispose()
        {
        }
    }
}