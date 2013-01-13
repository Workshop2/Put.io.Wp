using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Put.io.Core.Authentication;
using Put.io.Wp8.UserControls.Popups;

namespace Put.io.Wp8.UserControls
{
    public delegate void ApiKeyFoundHandler(string apiKey);

    public partial class ApiKeyFetcher : IPopupClient
    {
        private readonly Uri _landingPage = App.ViewModel.AuthenticateUrl;

        public ApiKeyFetcher()
        {
            InitializeComponent();

            SetupBrowser();
            SetLandingPage();
        }

        private void SetupBrowser()
        {
            var clear = new[]
            {
                Browser.ClearInternetCacheAsync(),
                Browser.ClearCookiesAsync()
            };

            Task.WaitAll(clear, (int) new TimeSpan(0, 0, 10).TotalMilliseconds);

            var cookies = new CookieContainer().GetCookies(new Uri("https://api.put.io/"));
            foreach (Cookie cookie in cookies)
            {
                cookie.Discard = true;
                cookie.Expired = true;
            }

            Browser.Navigating += BrowserNavigating;
            Browser.Navigated += BrowserNavigated;
            Browser.NavigationFailed += BrowserNavigationFailed;
        }

        private void SetLandingPage()
        {
            Browser.Navigate(_landingPage);
        }

        private void BrowserNavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            e.Handled = true;

            if (e.Exception != null)
                DisplayError();
        }

        private void BrowserNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Progress.Visibility = Visibility.Collapsed;
            BrowserGrid.Visibility = Visibility.Visible;
        }

        private void BrowserNavigating(object sender, NavigatingEventArgs e)
        {
            BrowserGrid.Visibility = Visibility.Collapsed;
            Progress.Visibility = Visibility.Visible;

            var url = e.Uri.ToString();
            var callbackHandler = new CallbackHandler();
            var result = callbackHandler.ParseAccessToken(url);

            if (result.Status != CallbackStatus.Success)
            {
                //If we are being navigated away from the API domain, lets error :)
                if (!UrlWithinKnownWaters(e))
                {
                    DisplayError();
                }

                return;
            }

            e.Cancel = true;

            if (OnClose != null)
            {
                OnClose();
            }

            if (OnKeyFound != null)
            {
                OnKeyFound(result.Token);
            }
        }

        //TODO: Make this dynamic - maybe download from x-volt.com?
        private static bool UrlWithinKnownWaters(NavigatingEventArgs e)
        {
            if (e.Uri.AbsoluteUri.Equals("https://api.put.io/login", StringComparison.InvariantCultureIgnoreCase))
                return true;

            return e.Uri.Segments.Any(x => x.Equals(@"oauth2/", StringComparison.InvariantCultureIgnoreCase));
        }

        private void DisplayError()
        {
            MessageBox.Show("Unable to detect login, please try again");
            SetLandingPage();
        }

        public event CloseHandler OnClose;
        public event ApiKeyFoundHandler OnKeyFound;
        public UserControl UserControl { get { return this; } }
    }
}
