using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Put.io.Api.Authentication;
using Put.io.Core.Models;
using Put.io.Wp8.Resources;
using Put.io.Wp8.ViewModels;

namespace Put.io.Wp8
{
    public partial class MainPage : PhoneApplicationPage
    {
        public File TestFile { get; set; }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            TestFile = new File() {FileID = 234};

            Browser.Navigating += Browser_Navigating;
            var urlHelper = new Api.UrlHelper.UrlHelperFactory().GetUrlDetails();
            Browser.Navigate(new Uri(urlHelper.AuthenticateUrl(), UriKind.Absolute));
            MainLongListSelector.ItemsSource = new List<File>
                                                   {
                                                       TestFile
                                                   };
        }

        private void Browser_Navigating(object sender, NavigatingEventArgs e)
        {
            var url = e.Uri.ToString();
            var callbackHandler = new CallbackHandler();
            var result = callbackHandler.ParseAccessToken(url);

            if (result.Status == CallbackStatus.Success)
            {
                Browser.Visibility = Visibility.Collapsed;
                
                MessageBox.Show(result.Token);
                e.Cancel = true;

                TestFile.FileID = 22222;
            }
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        // Handle selection changed on LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            //if (MainLongListSelector.SelectedItem == null)
            //    return;

            //// Navigate to the new page
            //NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as ItemViewModel).ID, UriKind.Relative));

            //// Reset selected item to null (no selection)
            //MainLongListSelector.SelectedItem = null;
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}