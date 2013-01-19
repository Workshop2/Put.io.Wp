using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Put.io.Wp.Views
{
    public partial class StreamVideo : PhoneApplicationPage
    {
        public StreamVideo()
        {
            InitializeComponent();

            //TODO: Implement connection detection, implement controls, maybe detect size of download


            PreviousState = MediaElementState.Stopped;
            Player.CurrentStateChanged += PlayerOnCurrentStateChanged;
            Player.MediaFailed += PlayerOnMediaFailed;
            
            App.ViewModel.FileCollection.GetMp4Url(App.ViewModel.FileCollection.SelectedFile, (Uri uri) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    Player.Source = uri;
                    Player.Play();
                });
            });
        }

        private void PlayerOnMediaFailed(object sender, ExceptionRoutedEventArgs exceptionRoutedEventArgs)
        {
            throw new NotImplementedException();
        }

        private MediaElementState PreviousState { get; set; }
        private void PlayerOnCurrentStateChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Player.CurrentState == PreviousState)
                return;

            PreviousState = Player.CurrentState;

            Dispatcher.BeginInvoke(() =>
            {
                switch (Player.CurrentState)
                {
                    case MediaElementState.Buffering:
                    case MediaElementState.Opening:
                        Progress.Visibility = Visibility.Visible;
                        break;
                    default:
                        Progress.Visibility = Visibility.Collapsed;
                        break;
                }
            });
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            Player.Stop();

            //Change the selected file to its parent
            App.ViewModel.FileCollection.NavigateUp();
        }
    }
}