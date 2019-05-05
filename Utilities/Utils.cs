using System;

namespace Binoculars.Utilities
{
    /// <summary>
    /// If run from Revit, returns Revit version etc.
    /// </summary>

    public static class Utils
    {
        internal static string GetRevitData()
        {
            // todo -> local variable 'doc' is never used?
            Autodesk.Revit.DB.Document doc = RevitServices.Persistence.DocumentManager.Instance.CurrentDBDocument;
            Autodesk.Revit.UI.UIApplication uiapp = RevitServices.Persistence.DocumentManager.Instance.CurrentUIApplication;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;

            return String.Format("Revit Build: {0}", app.SubVersionNumber);
        }
    }
}
