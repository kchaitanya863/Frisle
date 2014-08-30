using System;
using System.Xml.Linq;
using System.Windows.Input;

using AppStudio.Services;

namespace AppStudio.Data
{
    public class AboutThisAppViewModel
    {
        static private AboutThisAppViewModel _current = null;
        static private ShareServices _shareService = new ShareServices();

        static public AboutThisAppViewModel Current
        {
            get { return _current ?? (_current = new AboutThisAppViewModel()); }
        }

        public string DeveloperName
        {
            get
            {
                return XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Author").Value;
            }
        }

        public string AppVersion
        {
            get
            {
                return XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
            }
        }

        public string AboutText
        {
            get
            {
                return "Here\'s a website that brings you everything you need to know, want to know or wan" +
    "t to learn about technology. It delivers you News, Solutions, Tricks, Reviews on" +
    " your desired products all at just one shot \n@ www.frisle.com";
            }
        }

        public ICommand ShareAppCommand
        {
            get
            {
                return new RelayCommand<string>((src) =>
                {
                    string appUri = String.Format("http://xap.winappstudio.com/Job/GetXap?ticket={0}", "464139.vqbjgl");
                    if (_shareService.AppExistInMarketPlace())
                    {
                        appUri = Windows.ApplicationModel.Store.CurrentApp.LinkUri.AbsoluteUri;
                    }
                    _shareService.Share("app", "message", appUri, string.Empty);
                });
            }
        }
    }
}
