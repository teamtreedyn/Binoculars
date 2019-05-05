using System;
using System.Globalization;

namespace Binoculars.Utilities
{
    
    // Just a little something to make Title Case text. 
    public class Format
    {
        public static string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
    }

    // Just a little something to make Universal and Local Dates. 
    /*public class Date
 
    {
        public static void UniversalDate()
        { 
            string = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK");
        }
        
        public static void LocalDate()
        {
            var localDate = new DateTime(2009, 6, 15, 13, 45, 30, 
                DateTimeKind.Local);
            
        }
    }*/
}
