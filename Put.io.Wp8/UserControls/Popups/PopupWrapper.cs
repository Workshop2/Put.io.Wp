using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Put.io.Wp8.UserControls.Popups
{
    public class PopupWrapper
    {
        public event CloseHandler OnClose;
        private IPopupClient Client { get; set; }
        private Popup PopupDisplay { get; set; }

        public PopupWrapper(IPopupClient client, RectangleSpacing spacing)
        {
            Client = client;
            Client.OnClose += Close;

            if (spacing.AdjustedWidth.HasValue)
                Client.UserControl.Width = spacing.AdjustedWidth.Value;

            if (spacing.AdjustedHeight.HasValue)
                Client.UserControl.Height = spacing.AdjustedHeight.Value;
            

            PopupDisplay = new Popup
            {
                Child = Client.UiElement,
                VerticalOffset = spacing.Top,
                HorizontalOffset = spacing.Left
            };
        }

        public void Close()
        {
            PopupDisplay.IsOpen = false;

            if (OnClose != null)
                OnClose();
        }

        public void Open()
        {
            PopupDisplay.IsOpen = true;
        }

        public bool IsOpen { get { return PopupDisplay.IsOpen; } }
    }
}