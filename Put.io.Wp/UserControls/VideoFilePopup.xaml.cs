using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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

        public VideoFilePopup(FileViewModel context, ProgressTracker tracker)
        {
            InitializeComponent();

            DataContext = context;
            CurrentFile = context;
            ProgressTracker = tracker;

            GetMp4Status(context);
        }

        private void GetMp4Status(FileViewModel context)
        {
            App.ViewModel.FileCollection.GetMp4Status(context, (Mp4Status mp4Available) =>
            {
                switch (mp4Available)
                {
                    case Mp4Status.Completed:
                        DownloadMp4.IsEnabled = true;
                        StreamMp4.IsEnabled = true;
                        break;
                    case Mp4Status.NotAvailable:
                        ConvertMp4.IsEnabled = true;
                        break;
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
            Redirect("/Views/StreamVideo.xaml");
        }
        public void Close()
        {
            if (App.ViewModel.FileCollection.SelectedFile == CurrentFile)
                App.ViewModel.FileCollection.NavigateUp();
        }
    }
}
