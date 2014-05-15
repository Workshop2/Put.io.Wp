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
using Put.io.Wp.ApplicationBarHandling;
using Put.io.Wp.UserControls;
using Put.io.Wp.UserControls.Popups;
using ReviewNotifier.Apollo;
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

            ButtonHandler = new ApplicationBarHandler(ApplicationBar);
            ButtonHandler.OnClick += ButtonHandlerOnOnClick;

            Pivot_OnSelectionChanged(Pivot, null);
        }

        #region System

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.ViewModel.OnWorkingStatusChanged += ViewModel_OnWorkingStatusChanged;
            App.ViewModel.OnOpenFilePopup += ViewModel_OnOpenFilePopup;

            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.ViewModel.OnWorkingStatusChanged -= ViewModel_OnWorkingStatusChanged;
            App.ViewModel.OnOpenFilePopup -= ViewModel_OnOpenFilePopup;
        }

        /// <summary>
        /// Ensures the execution of the thread is done on the UI theada
        /// </summary>
        /// <param name="action"></param>
        private void UpdateUi(Action action)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(action);
                return;
            }

            action();
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
                if (Files.IsSelectionEnabled)
                {
                    Files.IsSelectionEnabled = false;
                    e.Cancel = true;
                    return;
                }
                if (App.ViewModel.FileCollection.NavigateUp())
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void Pivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateApplicationBar();
        }

        #endregion

        #region Files

        private void FileSelectionChanged(object sender, GestureEventArgs e)
        {
            var selector = sender as Grid;

            // If selected item is null (no selection) do nothing
            if (selector == null || selector.DataContext == null)
                return;

            var selected = selector.DataContext as FileViewModel;

            if (selected == null)
                return;

            App.ViewModel.SelectFile(selected);
        }

        private void IsSelectingChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateApplicationBar();
        }

        #endregion

        #region Transfers

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

        #endregion

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
            var videoFilePopup = new VideoFilePopup(file);
            SetupPopup(videoFilePopup);
        }

        private void LoginClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException("Test for raygun");
            var apiKeyFetcher = new ApiKeyFetcher();
            apiKeyFetcher.OnKeyFound += App.ViewModel.ChangeKey;

            SetupPopup(apiKeyFetcher);
        }

        private void AboutClicked(object sender, EventArgs e)
        {
            var aboutPopup = new AboutPopup();
            SetupPopup(aboutPopup);
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
        private ApplicationBarHandler ButtonHandler { get; set; }

        private void UpdateApplicationBar()
        {
            if (Pivot.SelectedItem == FilesPivot)
            {
                SetupFilesApplicationBar();
            }

            if (Pivot.SelectedItem == TransfersPivot)
            {
                ButtonHandler.DisplayButtons(new[] { ApplicationBarButtons.Refresh, ApplicationBarButtons.Cleanup });
            }
        }

        private void SetupFilesApplicationBar()
        {
            ButtonHandler.DisplayButtons(Files.IsSelectionEnabled ?
                new[] { ApplicationBarButtons.SelectAll, ApplicationBarButtons.Delete, ApplicationBarButtons.Convert } :
                new[] { ApplicationBarButtons.Refresh, ApplicationBarButtons.Select });
        }

        private void ButtonHandlerOnOnClick(ApplicationBarButtons button)
        {
            switch (button)
            {
                case ApplicationBarButtons.Cleanup:
                    ClearupClick();
                    break;
                case ApplicationBarButtons.Refresh:
                    RefreshClicked();
                    break;
                case ApplicationBarButtons.SelectAll:
                    SelectAllClicked();
                    break;
                case ApplicationBarButtons.Convert:
                    ConvertClicked();
                    break;
                case ApplicationBarButtons.Delete:
                    DeleteClicked();
                    break;
                case ApplicationBarButtons.Select:
                    SelectClicked();
                    break;
            }
        }

        private void SelectClicked()
        {
            Files.IsSelectionEnabled = true;
        }

        private void DeleteClicked()
        {
            var selected = SelectedFiles();
            if (!selected.Any())
                return;

            var messageBox = new CustomMessageBox
            {
                Caption = "Delete?",
                Message = "Are you sure you want to delete these files?",
                LeftButtonContent = "yes",
                RightButtonContent = "no",
                IsFullScreen = false
            };

            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        App.ViewModel.FileCollection.DeleteFiles(selected);
                        break;
                }
            };

            messageBox.Show();
        }

        private void ConvertClicked()
        {
            var selected = SelectedFiles();
            Files.IsSelectionEnabled = false;

            App.ViewModel.FileCollection.ConvertToMp4(selected);
        }

        private List<FileViewModel> SelectedFiles()
        {
            return Files.SelectedItems.Cast<FileViewModel>().Distinct().ToList();
        }

        private void RefreshClicked()
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

        private void ClearupClick()
        {
            App.ViewModel.TransferCollection.Clearup();
        }

        private void SelectAllClicked()
        {
            if (!Files.IsSelectionEnabled)
                return;

            var selected = Files.SelectedItems.Cast<FileViewModel>().Distinct().ToList();
            var currentFiles = App.ViewModel.FileCollection.CurrentFileList;

            if (currentFiles.Count == selected.Count)
            {
                Files.IsSelectionEnabled = false;
                return;
            }

            foreach (var item in Files.ItemsSource)
            {
                var container = Files.ContainerFromItem(item) as LongListMultiSelectorItem;
                if (container != null)
                    container.IsSelected = true;
            }
        }

        #endregion

        private async void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            await ReviewNotification.TriggerAsync("Enjoying this app? Not enjoying this app? Please leave a review :-)", "Time for a review?", 5);
        }
    }
}