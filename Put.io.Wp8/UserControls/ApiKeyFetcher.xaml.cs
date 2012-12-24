using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Put.io.Core.Authentication;

namespace Put.io.Wp8.UserControls
{
    public partial class ApiKeyFetcher : UserControl
    {
        public ApiKeyFetcher()
        {
            InitializeComponent();

            Browser.Navigating += BrowserNavigating;
            Browser.Navigate(new Uri(App.ViewModel.AuthenticateUrl(), UriKind.Absolute));
        }

        private void BrowserNavigating(object sender, NavigatingEventArgs e)
        {
            var url = e.Uri.ToString();
            var callbackHandler = new CallbackHandler();
            var result = callbackHandler.ParseAccessToken(url);

            if (result.Status != CallbackStatus.Success) return;

            Browser.Visibility = Visibility.Collapsed;

            MessageBox.Show(result.Token);
            e.Cancel = true;
        }
    }
}
