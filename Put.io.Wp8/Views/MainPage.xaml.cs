using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Put.io.Core.ViewModels;
using Put.io.Wp8.UserControls;
using Put.io.Wp8.UserControls.Popups;

namespace Put.io.Wp8.Views
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

            Pivot_OnSelectionChanged(Pivot, null);
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            App.ViewModel.OnWorkingStatusChanged += ViewModel_OnWorkingStatusChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.ViewModel.OnWorkingStatusChanged -= ViewModel_OnWorkingStatusChanged;
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

            App.ViewModel.SelectFile(selected);

            //Clear selection to avoid problems down the road
            selector.SelectedItem = null;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (Popup != null && Popup.IsOpen)
            {
                Popup.Close();
                e.Cancel = true;
                return;
            }

            if (Pivot.SelectedItem == FilesPivot)
            {
                if (App.ViewModel.FileCollection.NavigateUp())
                {
                    Files.SelectedItem = null;
                    e.Cancel = true;
                    return;
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

        #region ProgressBar

        private ProgressIndicator ProgressBar
        {
            get
            {
                return SystemTray.ProgressIndicator;
            }
        }

        private void ViewModel_OnWorkingStatusChanged(bool isWorking)
        {
            ProgressBar.IsVisible = isWorking;
        }

        #endregion

        private PopupWrapper Popup { get; set; }
        private void LoginClicked(object sender, EventArgs e)
        {
            const int vertOffset = 48;
            const int horizOffset = 40;

            var appHost = Application.Current.Host.Content;
            var pageSize = new Size(appHost.ActualWidth, appHost.ActualHeight);
            var spacing = new RectangleSpacing(vertOffset, horizOffset, pageSize);
            var apiKeyFetcher = new ApiKeyFetcher();
            apiKeyFetcher.OnKeyFound += App.ViewModel.ChangeKey;

            Popup = new PopupWrapper(apiKeyFetcher, spacing, ApplicationBar);
            Popup.OnClose += Popup_OnClose;
            Popup.Open();
        }

        private void Popup_OnClose()
        {
            Popup.OnClose -= Popup_OnClose;
            Popup = null;
        }

        private void RefreshClicked(object sender, EventArgs e)
        {
            if (Pivot.SelectedItem == FilesPivot)
            {
                App.ViewModel.FileCollection.Refresh();
                return;
            }

            if (Pivot.SelectedItem == TransfersPivot)
            {
                App.ViewModel.TransferCollection.Refresh();
                return;
            }

        }

        private void TransferSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = sender as LongListSelector;

            // If selected item is null (no selection) do nothing
            if (selector == null || selector.SelectedItem == null)
                return;

            var selected = selector.SelectedItem as TransferViewModel;

            if (selected == null)
                return;

            App.ViewModel.SelectTransfer(selected);

            //Clear selection to avoid problems down the road
            selector.SelectedItem = null;
        }

        private void Pivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pivot = sender as Pivot;

            if (pivot == null)
                return;

            var clearButtonFound = FindClearButton();

            if (pivot.SelectedItem == FilesPivot)
            {
                if (clearButtonFound)
                    ApplicationBar.Buttons.Remove(ClearButton);
            }

            if (pivot.SelectedItem == TransfersPivot)
            {
                if (!clearButtonFound)
                {
                    ApplicationBar.Buttons.Add(ClearButton);
                }
            }
        }

        private void ClearupClick(object sender, EventArgs e)
        {
            App.ViewModel.TransferCollection.Clearup();
        }

        private ApplicationBarIconButton ClearButton { get; set; }
        private bool FindClearButton()
        {
            var matching = ApplicationBar.Buttons.Cast<ApplicationBarIconButton>()
                              .Where(button => button.Text.Equals("Cleanup", StringComparison.InvariantCultureIgnoreCase));

            var applicationBarIconButtons = matching as IList<ApplicationBarIconButton> ?? matching.ToList();
            var clearButton = applicationBarIconButtons.FirstOrDefault();

            if (clearButton != null)
                ClearButton = clearButton;

            return clearButton != null;
        }
    }
}