using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Put.io.Core.Models;
using Put.io.Core.ViewModels;
using Put.io.Wp8.Resources;

namespace Put.io.Wp8
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {

            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            //Browser.Navigating += Browser_Navigating;
            //var urlHelper = new Api.UrlHelper.UrlHelperFactory().GetUrlDetails();
            //Browser.Navigate(new Uri(urlHelper.AuthenticateUrl(), UriKind.Absolute));
        }

        //private void Browser_Navigating(object sender, NavigatingEventArgs e)
        //{
        //    var url = e.Uri.ToString();
        //    var callbackHandler = new CallbackHandler();
        //    var result = callbackHandler.ParseAccessToken(url);

        //    if (result.Status == CallbackStatus.Success)
        //    {
        //        Browser.Visibility = Visibility.Collapsed;

        //        MessageBox.Show(result.Token);
        //        e.Cancel = true;
        //    }
        //}

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        // Handle selection changed on LongListSelector
        private void FileSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = sender as LongListSelector;

            // If selected item is null (no selection) do nothing
            if (selector == null || selector.SelectedItem == null)
                return;

            var selected = selector.SelectedItem as FileViewModel;

            if (selected == null)
                return;

            App.ViewModel.FileCollection.SelectedFile = selected;
            App.ViewModel.FileCollection.ExpandFile(selected);

            //// Navigate to the new page
            //NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as ItemViewModel).ID, UriKind.Relative));

            //// Reset selected item to null (no selection)
            //MainLongListSelector.SelectedItem = null;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (Pivot.SelectedItem == FilesPivot)
            {
                if (App.ViewModel.FileCollection.NavigateUp())
                {
                    Files.SelectedItem = null;
                    e.Cancel = true;
                }
            }
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