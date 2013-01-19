using System;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Shell;

namespace Put.io.Wp.UserControls.Popups
{
    public class PopupWrapper
    {
        public event CloseHandler OnClose;
        private IPopupClient Client { get; set; }
        private Popup PopupDisplay { get; set; }
        private IApplicationBar ApplicationBar { get; set; }
        private bool ApplicationBarPreviousState { get; set; }
        public event RedirectHandler OnRedirect;

        public PopupWrapper(IPopupClient client, RectangleSpacing spacing)
        {
            Client = client;
            Client.OnClose += Close;
            Client.OnRedirect += ClientOnOnRedirect;

            if (spacing.AdjustedWidth.HasValue)
                Client.UserControl.Width = spacing.AdjustedWidth.Value;

            if (spacing.AdjustedHeight.HasValue)
                Client.UserControl.Height = spacing.AdjustedHeight.Value;
            
            PopupDisplay = new Popup
            {
                Child = Client.UserControl,
                VerticalOffset = spacing.Top,
                HorizontalOffset = spacing.Left
            };
        }

        public PopupWrapper(IPopupClient client, RectangleSpacing spacing, IApplicationBar applicationBar)
            :this(client, spacing)
        {
            ApplicationBar = applicationBar;
        }

        public void Close()
        {
            PopupDisplay.IsOpen = false;
            Client.Close();

            if (OnClose != null)
                OnClose();

            if (ApplicationBar != null && ApplicationBarPreviousState)
            {
                ApplicationBar.IsVisible = true;
            }
        }

        public void Open()
        {
            if (ApplicationBar != null)
            {
                ApplicationBarPreviousState = ApplicationBar.IsVisible;
                ApplicationBar.IsVisible = false;
            }

            PopupDisplay.IsOpen = true;
        }

        private void ClientOnOnRedirect(string uri)
        {
            if (OnRedirect != null)
                OnRedirect(uri);

            Close();
        }
        
        public bool IsOpen { get { return PopupDisplay.IsOpen; } }
    }
}