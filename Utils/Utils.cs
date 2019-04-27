using System;
using System.IO;
using System.Reflection;

namespace Binoculars
{
    public static class Utils
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
        /// <summary>
        /// If run from Revit, returns Revit version
        /// </summary>
        /// <returns></returns>
        internal static string GetRevitData()
        {
            Autodesk.Revit.DB.Document doc = RevitServices.Persistence.DocumentManager.Instance.CurrentDBDocument;
            Autodesk.Revit.UI.UIApplication uiapp = RevitServices.Persistence.DocumentManager.Instance.CurrentUIApplication;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;

            return String.Format("Revit Build: {0}", app.SubVersionNumber);
        }
    }
}
