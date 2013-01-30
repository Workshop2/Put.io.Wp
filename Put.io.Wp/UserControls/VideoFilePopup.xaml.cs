using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Put.io.Core.Models;
using Put.io.Core.ProgressTracking;
using Put.io.Core.ViewModels;
using Put.io.Wp.UserControls.Popups;

namespace Put.io.Wp.UserControls
{
    public partial class VideoFilePopup : IPopupClient
    {
        public event CloseHandler OnClose;
        public event RedirectHandler OnRedirect;
        public UserControl UserControl { get { return this; } }

        private FileViewModel CurrentFile { get; set; }
        private ProgressTracker ProgressTracker { get; set; }
        private bool Visible { get; set; }

        public VideoFilePopup(FileViewModel context, ProgressTracker tracker)
        {
            InitializeComponent();

            DataContext = context;
            CurrentFile = context;
            ProgressTracker = tracker;
            Visible = true;

            //Put.io assumes that if the file format is of MP4 then we can use it
            if (CurrentFile.File.Name.EndsWith("mp4", StringComparison.InvariantCultureIgnoreCase))
            {
                //TODO: Question put.io on this crap
                ConvertMp4.Content = "Mp4 unavailable";
                ConvertMp4.IsEnabled = false;
                DownloadMp4.IsEnabled = false;
                StreamMp4.IsEnabled = false;
                return;
            }

            App.ViewModel.FileCollection.GetMp4Status(context, (mp4Available, percentDone) =>
            {
                if (mp4Available == Mp4Status.InQueue || mp4Available == Mp4Status.Converting)
                    Task.Factory.StartNew(ConversionInProgress);

                UpdateUi(() => SetupButtons(mp4Available, percentDone));
            });
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

        private void SetupButtons(Mp4Status mp4Available, int percentDone)
        {
            switch (mp4Available)
            {
                case Mp4Status.Completed:
                    ConvertMp4.IsEnabled = false;
                    DownloadMp4.IsEnabled = true;
                    StreamMp4.IsEnabled = true;
                    break;
                case Mp4Status.NotAvailable:
                    ConvertMp4.IsEnabled = true;
                    DownloadMp4.IsEnabled = false;
                    StreamMp4.IsEnabled = false;
                    break;
                case Mp4Status.Converting:
                case Mp4Status.InQueue:
                    ConvertMp4.IsEnabled = false;
                    DownloadMp4.IsEnabled = false;
                    StreamMp4.IsEnabled = false;
                    break;
            }

            switch (mp4Available)
            {
                case Mp4Status.Converting:
                    ConvertMp4.Content = string.Format("Converted {0}{1}", percentDone, CultureInfo.CurrentCulture.NumberFormat.PercentSymbol);
                    break;
                case Mp4Status.InQueue:
                    ConvertMp4.Content = "In queue...";
                    break;
                default:
                    ConvertMp4.Content = "Convert to MP4";
                    break;
            }
        }

        private void ConversionInProgress()
        {
            if (!Visible)
                return;

            App.ViewModel.FileCollection.GetMp4Status(CurrentFile, (mp4Available, percentDone) =>
            {
                UpdateUi(() => SetupButtons(mp4Available, percentDone));

                if (mp4Available == Mp4Status.Converting || mp4Available == Mp4Status.InQueue)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    Task.Factory.StartNew(ConversionInProgress);
                }
            });

        }

        private void Redirect(string uri)
        {
            if (OnRedirect != null)
            {
                OnRedirect(uri);
            }
        }

        private void StreamMp4_OnClick(object sender, RoutedEventArgs e)
        {
            StreamMp4.IsEnabled = false;
            DownloadMp4.IsEnabled = false;

            //TODO: Check URI
            App.ViewModel.FileCollection.GetMp4Url(CurrentFile, uri =>
            {
                App.ViewModel.StreamUrls.Push(uri.AbsoluteUri);
                Redirect("/Views/StreamVideo.xaml");
                Dispatcher.BeginInvoke(() =>
                {
                    StreamMp4.IsEnabled = true;
                    DownloadMp4.IsEnabled = true;
                });
            });
        }

        public void Close()
        {
            if (App.ViewModel.FileCollection.SelectedFile == CurrentFile)
                App.ViewModel.FileCollection.NavigateUp();

            Visible = false;
        }

        private void ConvertMp4_OnClick(object sender, RoutedEventArgs e)
        {
            App.ViewModel.FileCollection.ConvertToMp4(CurrentFile, () => Task.Factory.StartNew(ConversionInProgress));
        }
    }
}
