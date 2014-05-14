using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;
using Put.io.Core.ViewModels;
using Put.io.Wp.UserControls.Popups;

namespace Put.io.Wp.UserControls
{
    public partial class AboutPopup : IPopupClient
    {
        public event CloseHandler OnClose;
        public event RedirectHandler OnRedirect;
        public UserControl UserControl { get { return this; } }
        private AboutViewModel Data { get { return DataContext as AboutViewModel; } }

        public AboutPopup()
        {
            InitializeComponent();
            DataContext = new AboutViewModel();
        }

        private void LeaveReview_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new MarketplaceReviewTask();
            task.Show();
        }

        private void VisitHomepage_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new WebBrowserTask { Uri = new Uri(Data.HomeUrl, UriKind.Absolute) };
            task.Show();
        }

        private void ViewSource_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new WebBrowserTask { Uri = new Uri(Data.SourceUrl, UriKind.Absolute) };
            task.Show();
        }

        private void EmailAuthor_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new EmailComposeTask { To = Data.AuthorEmail, Subject = Data.EmailSubject, Body = Data.EmailBody };
            task.Show();
        }

        public void Close()
        {

        }
    }
}
