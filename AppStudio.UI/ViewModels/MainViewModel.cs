using System.Threading.Tasks;
using System.Windows.Input;

using AppStudio.Data;
using AppStudio.Services;

namespace AppStudio
{
    public class MainViewModels : BindableBase
    {
       private TopNewsViewModel _topNewsModel;
       private NewsViewModel _newsModel;
       private TechSolutionViewModel _techSolutionModel;
       private NumberGameViewModel _numberGameModel;
       private FacebookViewModel _facebookModel;

        private ViewModelBase _selectedItem = null;
        private PrivacyViewModel _privacyModel;

        public MainViewModels()
        {
            _selectedItem = TopNewsModel;
            _privacyModel = new PrivacyViewModel();
        }
 
        public TopNewsViewModel TopNewsModel
        {
            get { return _topNewsModel ?? (_topNewsModel = new TopNewsViewModel()); }
        }
 
        public NewsViewModel NewsModel
        {
            get { return _newsModel ?? (_newsModel = new NewsViewModel()); }
        }
 
        public TechSolutionViewModel TechSolutionModel
        {
            get { return _techSolutionModel ?? (_techSolutionModel = new TechSolutionViewModel()); }
        }
 
        public NumberGameViewModel NumberGameModel
        {
            get { return _numberGameModel ?? (_numberGameModel = new NumberGameViewModel()); }
        }
 
        public FacebookViewModel FacebookModel
        {
            get { return _facebookModel ?? (_facebookModel = new FacebookViewModel()); }
        }

        public void SetViewType(ViewTypes viewType)
        {
            TopNewsModel.ViewType = viewType;
            NewsModel.ViewType = viewType;
            TechSolutionModel.ViewType = viewType;
            NumberGameModel.ViewType = viewType;
            FacebookModel.ViewType = viewType;
        }

        public ViewModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                UpdateAppBar();
            }
        }

        public bool IsAppBarVisible
        {
            get
            {
                if (SelectedItem == null || SelectedItem == TopNewsModel)
                {
                    return true;
                }
                return SelectedItem != null ? SelectedItem.IsAppBarVisible : false;
            }
        }

        public bool IsLockScreenVisible
        {
            get { return SelectedItem == null || SelectedItem == TopNewsModel; }
        }

        public bool IsAboutVisible
        {
            get { return SelectedItem == null || SelectedItem == TopNewsModel; }
        }

        public bool IsPrivacyVisible
        {
            get { return SelectedItem == null || SelectedItem == TopNewsModel; }
        }


        public void UpdateAppBar()
        {
            OnPropertyChanged("IsAppBarVisible");
            OnPropertyChanged("IsLockScreenVisible");
            OnPropertyChanged("IsAboutVisible");
        }

        /// <summary>
        /// Load ViewModel items asynchronous
        /// </summary>
        public async Task LoadData(bool isNetworkAvailable)
        {
            var loadTasks = new Task[]
            { 
                TopNewsModel.LoadItems(isNetworkAvailable),
                NewsModel.LoadItems(isNetworkAvailable),
                TechSolutionModel.LoadItems(isNetworkAvailable),
                NumberGameModel.LoadItems(isNetworkAvailable),
                FacebookModel.LoadItems(isNetworkAvailable),
            };
            await Task.WhenAll(loadTasks);
        }

        //
        //  ViewModel command implementation
        //
        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateToPage("AboutThisAppPage");
                });
            }
        }

        public ICommand PrivacyCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateTo(_privacyModel.Url);
                });
            }
        }

        public ICommand LockScreenCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    LockScreenServices.SetLockScreen("/Assets/LockScreenImage.png");
                });
            }
        }
    }
}
