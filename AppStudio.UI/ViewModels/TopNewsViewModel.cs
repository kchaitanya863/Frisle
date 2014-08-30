using System;
using System.Windows;
using System.Windows.Input;

using AppStudio.Services;

namespace AppStudio.Data
{
    public class TopNewsViewModel : ViewModelBase<TopNewsSchema>
    {
        override protected string CacheKey
        {
            get { return "TopNewsDataSource"; }
        }

        override protected IDataSource<TopNewsSchema> CreateDataSource()
        {
            return new TopNewsDataSource(); // CollectionDataSource
        }

        override public bool IsPinToStartVisible
        {
            get { return ViewType == ViewTypes.Detail; }
        }

        override public void PinToStart()
        {
            base.PinToStart("TopNewsDetail", "{DefaultTitle}", "{DefaultSummary}", "{DefaultImageUrl}");
        }

        override public bool IsShareItemVisible
        {
            get { return ViewType == ViewTypes.Detail; }
        }
        
        override public void ShareItem()
        {
            base.ShareItem("{DefaultTitle}", "{DefaultSummary}", "{DefaultImageUrl}", "{DefaultImageUrl}");
        }

        override public bool IsSpeakTextVisible
        {
            get { return ViewType == ViewTypes.Detail; }
        }
        
        override public void SpeakText()
        {
            base.SpeakText("Description");
        }

        override public bool IsRefreshVisible
        {
            get { return ViewType == ViewTypes.List; }
        }

        override protected void NavigateToSelectedItem()
        {
            NavigationServices.NavigateToPage("TopNewsDetail");
        }
    }
}
