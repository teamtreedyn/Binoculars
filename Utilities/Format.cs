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

        public static string UniversalDate
        {
            get
            {
                var date = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK");
                return date;
            }
        }
    }
}