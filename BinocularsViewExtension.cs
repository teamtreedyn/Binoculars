using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Binoculars
{
    /// <summary>
    /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
    /// </summary>
    public class BinocularsViewExtension : IViewExtension
    {
        public string Name => "Binoculars View Extension";

        private string UserName = Environment.UserName;

        private MenuItem menu;
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

            // Load user/organisation settings
            // If it fails then gracefully stop loading the extension.
            if ( ! Settings.Load()) return;

            // we can now add custom menu items to Dynamo's UI
            BinocularsMenuItems();

            string message;
            string title;
            string[] messages;

            if ( ! (Boolean)Settings.consent["requested"])
            {
                // If consent has NOT been requested then ask for it!
                // Display a MessageBox to the user
                // @todo Use a view framework to improve the UI/UX
                message = "By pressing OK you agreeing to Binoculars 🔍 data collection.";
                title = "Terms of Use Agreement";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            // @todo Add a method to record the user's consent response

            // Check for consent
            if ( ! (Boolean)Settings.consent["given"])
            {
                // If consent has NOT been given

                // If running in DEBUG mode display a MessageBox
                #if DEBUG
                    messages = new string[] {
                            "Hi Anonymous,",
                            "You didn't give us consent to record your details so I guess it's goodbye for now.",
                            "But, perhaps we can still be friends?"
                        };
                    title = "Binoculars";
                    MessageBox.Show(string.Join("\n\n", messages), title);
                #endif

                // The user did not consent so we must cease and desist
                return;

            }

            // Collect the environment variables
            Data.Collect(vlp);

            // Consent must have been given if we've reached this point so let the user know what we know about them.
            #if DEBUG
                messages = new string[] {
                    $"Hi {Data.user},",
                    $"You're currently on {Data.computerName} from {Data.city}, {Data.country} using Dynamo {Data.dynamoVersion}."
                };
                title = "Binoculars";
                MessageBox.Show(string.Join("\n\n", messages), title);
            #endif

            var dynamoViewModel = vlp.DynamoWindow.DataContext as DynamoViewModel;
            // we can register our own events that will be triggered when specific things happen in Dynamo
            // a reference to the ReadyParams is needed to do this, so we pass it on
            Events.Register(dynamoViewModel.Model);
        }

        /// <summary>
        /// Adds custom menu items to the Dynamo menu
        /// </summary>
        public string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public void BinocularsMenuItems()
        {
            // let's now create a completely top-level new menu item
            menu = new MenuItem {Header = "Binoculars 🔍" };

            // and now we add a new sub-menu item that says hello when clicked
            var aboutMenu = new MenuItem {Header = " ❓ About"};
            aboutMenu.Click += (sender, args) =>
            {
                // Display a MessageBox to the user
                // @todo Use a view framework to improve the UI/UX
                MessageBox.Show("Hello " + ToTitleCase(UserName) + "👋🏻\n\nWe at Binoculars just want to let you know that collecting user data is common practice in modern websites and applications as a way of providing creators with more information to make decisions and create better experiences. \n\nAmong other benefits, data can be used to help tailor content, drive product direction, and provide insight into problems in current implementations. Collecting relevant information and using it wisely can give organizations an edge over competitors and increase the impact of limited resources. \n\nKind Regards,\n\nAll the Team @ Binoculars.");
            };
            menu.Items.Add(aboutMenu);

            // Create an item linking to the Google Sheet if we're using it
            if ((String)Settings.export["method"] == "googleSheets")
            {
                var googleSheetMenu = new MenuItem { Header = "🧾 Google Sheet" };
                googleSheetMenu.Click += (sender, args) => {
                    Process.Start("https://docs.google.com/spreadsheets/d/" + (String)Settings.export["googleSheets"]["id"]);
                };
                menu.Items.Add(googleSheetMenu);
            }

            // Create an item linking to the Google Data Studio Dashboard if we're using it
            if ((String)Settings.dashboard["method"] == "googleDataStudio")
            {
                var googleDataStudioMenu = new MenuItem { Header = "📈 Google Data Studio" };
                googleDataStudioMenu.Click += (sender, args) =>
                {
                    Process.Start("https://datastudio.google.com/reporting/" + (String)Settings.dashboard["id"]);
                };
                menu.Items.Add(googleDataStudioMenu);
            }

            // finally, we need to add our menu to Dynamo
            _viewLoadedParams.dynamoMenu.Items.Add(menu);
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

        /// <summary>
        /// Unique GUID
        /// From: https://developer.dynamobim.org/03-Development-Options/3-6-extensions.html
        /// </summary>
        public string UniqueId
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
    }
}
