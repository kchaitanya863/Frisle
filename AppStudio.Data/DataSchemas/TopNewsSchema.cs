using System;

namespace AppStudio.Data
{
    /// <summary>
    /// Implementation of the TopNewsSchema class.
    /// </summary>
    public class TopNewsSchema : BindableSchemaBase
    {
        private string _title;
        private string _subtitle;
        private string _imageUrl;
        private string _description;
         
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
 
        public string Subtitle
        {
            get { return _subtitle; }
            set { SetProperty(ref _subtitle, value); }
        }
 
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { SetProperty(ref _imageUrl, value); }
        }
 
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public override string DefaultTitle
        {
            get { return Title; }
        }

        public override string DefaultSummary
        {
            get { return Subtitle; }
        }

        public override string DefaultImageUrl
        {
            get { return ImageUrl; }
        }

        override public string GetValue(string fieldName)
        {
            if (!String.IsNullOrEmpty(fieldName))
            {
                switch (fieldName.ToLowerInvariant())
                {
                    case "title":
                        return String.Format("{0}", Title); 
                    case "subtitle":
                        return String.Format("{0}", Subtitle); 
                    case "imageurl":
                        return String.Format("{0}", ImageUrl); 
                    case "description":
                        return String.Format("{0}", Description); 
                    case "defaulttitle":
                        return DefaultTitle;
                    case "defaultsummary":
                        return DefaultSummary;
                    case "defaultimageurl":
                        return DefaultImageUrl;
                    default:
                        break;
                }
            }
            return String.Empty;
        }
    }
}
