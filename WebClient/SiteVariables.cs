using System.Collections.Generic;


namespace WebClient
{
    public class SiteVariables
    {
        private static List<WebMessages> wm = new List<WebMessages>();

        public static List<WebMessages> WM
        {
            get { return wm; }
            set { wm = value; }
        }
    }
}