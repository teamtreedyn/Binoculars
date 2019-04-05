using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System;
using System.Diagnostics;
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
        public string Name => "Tracker View Extension";

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
            MessageBox.Show("Hello there, Tracker has loaded!");

            // hold a reference to the Dynamo params to be used later
            _viewLoadedParams = vlp;

            // we can now add custom menu items to Dynamo's UI
            MakeMenuItems();
        }

        /// <summary>
        /// Adds custom menu items to the Dynamo menu
        /// </summary>
        public void MakeMenuItems()
        {
            // let's now create a completely top-level new menu item
            _extensionMenu = new MenuItem {Header = "Tracker"};

            // and now we add a new sub-menu item that says hello when clicked
            var sayHelloMenuItem = new MenuItem {Header = "Freak User"};
            sayHelloMenuItem.Click += (sender, args) =>
            {
                MessageBox.Show("Hello " + Environment.UserName + " you are being watched!");
            };


            // now make a hackathon worthy menu item
            var hackMenuItem = new MenuItem {Header = "Tracker Information"};
            hackMenuItem.Click += (sender, args) => { Process.Start("https://www.google.com/search?biw=1707&bih=801&tbm=isch&sa=1&ei=kXenXJXRMI6HrwShk4vgCw&q=impressive+statistical+stuff&oq=impressive+statistical+stuff&gs_l=img.3...1791.5141..5399...0.0..0.140.648.8j1......1....1..gws-wiz-img.......0i24.aoH3n_7DImU"); };

            // add all menu items to menu
            _extensionMenu.Items.Add(sayHelloMenuItem);
            _extensionMenu.Items.Add(hackMenuItem);

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