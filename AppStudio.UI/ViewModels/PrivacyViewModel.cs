using AppStudio.Data;
using System;
using Windows.ApplicationModel;

namespace AppStudio
{
    public class PrivacyViewModel 
    {
        public Uri Url
        {
            get
            {
                return new Uri(UrlText, UriKind.RelativeOrAbsolute);
            }
        }
        public string UrlText
        {
            get
            {

                return "http://www.frisle.com/";
            }
        }
    }
}

