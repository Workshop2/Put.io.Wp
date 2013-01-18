using System.Windows.Controls;
using Put.io.Core.Models;
using Put.io.Core.ProgressTracking;
using Put.io.Core.ViewModels;
using Put.io.Wp.UserControls.Popups;

namespace Put.io.Wp.UserControls
{
    public partial class VideoFilePopup : IPopupClient
    {
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

        public event CloseHandler OnClose;
        public UserControl UserControl { get { return this; } }
    }
}
