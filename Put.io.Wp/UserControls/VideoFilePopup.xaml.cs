using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;
using Put.io.Core.Models;
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
        private bool Visible { get; set; }
        private const int Mp4StatusInterval = 5;

        public VideoFilePopup(FileViewModel context)
        {
            InitializeComponent();

            DataContext = context;
            CurrentFile = context;
            Visible = true;

            //Put.io assumes that if the file format is of MP4 then we can use it
            if (FileNativeMp4())
            {
                ConvertMp4.IsEnabled = false;
                DownloadMp4.IsEnabled = true;
                StreamMp4.IsEnabled = true;
                return;
            }

            App.ViewModel.FileCollection.GetMp4Status(context, (mp4Available, percentDone) =>
            {
                if (mp4Available == Mp4Status.InQueue || mp4Available == Mp4Status.Converting)
                    Task.Factory.StartNew(ConversionInProgress);

                UpdateUi(() => SetupButtons(mp4Available, percentDone));
            });
        }

        private bool FileNativeMp4()
        {
            return CurrentFile.File.Name.EndsWith("mp4", StringComparison.InvariantCultureIgnoreCase);
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
                    Thread.Sleep(TimeSpan.FromSeconds(Mp4StatusInterval));
                    Task.Factory.StartNew(ConversionInProgress);
                }
            });

        }

        private void StreamMp4_OnClick(object sender, RoutedEventArgs e)
        {
            StreamMp4.IsEnabled = false;
            DownloadMp4.IsEnabled = false;

            if(FileNativeMp4())
            {
                //TODO: Check URI
                App.ViewModel.FileCollection.GetStreamUrl(CurrentFile, LaunchVideo);
                return;
            }

            //TODO: Check URI
            App.ViewModel.FileCollection.GetMp4Url(CurrentFile, LaunchVideo);
        }

        private void LaunchVideo(Uri uri)
        {
            var task = new WebBrowserTask {Uri = uri};
            task.Show();

            Dispatcher.BeginInvoke(() =>
            {
                StreamMp4.IsEnabled = true;
                DownloadMp4.IsEnabled = true;
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
