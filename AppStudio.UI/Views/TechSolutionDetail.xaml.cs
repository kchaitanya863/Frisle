using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

using Microsoft.Phone.Controls;

using MyToolkit.Paging;

using AppStudio.Data;
using AppStudio.Services;

namespace AppStudio
{
    public partial class TechSolutionDetail
    {
        private bool _isDeepLink = false;

        public TechSolutionDetail()
        {
            InitializeComponent();
        }

        public TechSolutionViewModel TechSolutionModel { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TechSolutionModel = NavigationServices.CurrentViewModel as TechSolutionViewModel;
            if (e.NavigationMode == NavigationMode.New && NavigationContext.QueryString.ContainsKey("id"))
            {
                string id = NavigationContext.QueryString["id"];
                if (!String.IsNullOrEmpty(id))
                {
                    _isDeepLink = true;
                    TechSolutionModel = new TechSolutionViewModel();
                    NavigationServices.CurrentViewModel = TechSolutionModel;
                    TechSolutionModel.LoadItem(id);
                }
            }
            if (TechSolutionModel != null)
            {
                TechSolutionModel.ViewType = ViewTypes.Detail;
            }
            DataContext = TechSolutionModel;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                NavigationServices.CurrentViewModel = null;
            }
            SpeechServices.Stop();
            base.OnNavigatedFrom(e);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (_isDeepLink)
            {
                _isDeepLink = false;
                NavigationServices.NavigateToPage("MainPage");
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SpeechServices.Stop();
        }
    }
}
