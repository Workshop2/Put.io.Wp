using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Put.io.Core.Models;
using Put.io.Core.ProgressTracking;
using Put.io.Core.ViewModels;
using Put.io.Wp.UserControls;
using Put.io.Wp.UserControls.Popups;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Put.io.Wp.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        private PopupWrapper Popup { get; set; }

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
            App.ViewModel.OnOpenFilePopup += ViewModel_OnOpenFilePopup;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //App.ViewModel.OnWorkingStatusChanged -= ViewModel_OnWorkingStatusChanged;
            App.ViewModel.OnOpenFilePopup -= ViewModel_OnOpenFilePopup;
        }

        private void UpdateUi(Action action)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(action);
                return;
            }

            action();
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
            UpdateUi(() => { ProgressBar.IsVisible = isWorking; });
        }

        #endregion

        #region Popups

        private void SetupPopup(IPopupClient apiKeyFetcher)
        {
            const int vertOffset = 48;
            const int horizOffset = 40;
            var appHost = Application.Current.Host.Content;
            var pageSize = new Size(appHost.ActualWidth, appHost.ActualHeight);
            var spacing = new RectangleSpacing(vertOffset, horizOffset, pageSize);

            Popup = new PopupWrapper(apiKeyFetcher, spacing, ApplicationBar);
            Popup.OnClose += PopupOnClose;
            Popup.OnRedirect += PopupOnRedirect;
            Popup.Open();
        }

        private void ViewModel_OnOpenFilePopup(FileViewModel file, ProgressTracker tracker)
        {
            var videoFilePopup = new VideoFilePopup(file, tracker);
            SetupPopup(videoFilePopup);
        }

        private void LoginClicked(object sender, EventArgs e)
        {
            var apiKeyFetcher = new ApiKeyFetcher();
            apiKeyFetcher.OnKeyFound += App.ViewModel.ChangeKey;

            SetupPopup(apiKeyFetcher);
        }

        private void PopupOnClose()
        {
            Popup.OnClose -= PopupOnClose;
            Popup.OnRedirect -= PopupOnRedirect;
            Popup = null;
        }

        private void PopupOnRedirect(string uri)
        {
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        #endregion

        #region ApplicationBar

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

        #endregion

        private void TransferSelectionChanged(object sender, GestureEventArgs e)
        {
            var obj = sender as FrameworkElement;
            if (obj == null)
                return;

            var currentObject = obj.DataContext as TransferViewModel;
            if (currentObject == null)
                return;

            App.ViewModel.SelectTransfer(currentObject);

            e.Handled = true;
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

        private void CancelTransfer(object sender, RoutedEventArgs e)
        {
            var senderConverted = sender as MenuItem;

            if (senderConverted == null)
                return;

            var selectedItem = senderConverted.CommandParameter as Transfer;

            if (selectedItem == null)
                return;

            App.ViewModel.TransferCollection.CancelTransfer(selectedItem);
        }
    }
}