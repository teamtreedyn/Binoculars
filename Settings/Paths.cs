using System;
using System.IO;
using System.Reflection;

namespace Binoculars.Settings
{
    /// <summary>
    /// This is a little Class that niftily grabs the Paths of the Settings Folder, we are using this as our primary
    /// means of connecting with all User Configurations and Credentials.
    /// </summary>
    
    public static class Paths
    {
        // todo -> lets see if we can get rid of this class altogether, and find a more succinct way a of finding paths.
        public static string SettingsDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }
        
        public static string GoogleCredentialsPath
        {
            get
            {
                var assembly = SettingsDirectory;
                var file = "credentials.json";
                return System.IO.Path.GetFullPath(System.IO.Path.Combine(assembly, @"..\", file));
            }
        }
        
        public static string GoogleCredentialsToken
        {
            get
            {
                var assembly = SettingsDirectory;
                var file = "credentials.json";
                var path =  System.IO.Path.GetFullPath(System.IO.Path.Combine(assembly, @"..\", file));
                var tokenPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(assembly, @"..\", "token.json"));
                return tokenPath;
            }
        }
    }
}