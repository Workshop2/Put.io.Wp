using System.Windows.Controls;

namespace Put.io.Wp.UserControls.Popups
{
    public delegate void CloseHandler();
    public delegate void RedirectHandler(string uri);

    public interface IPopupClient
    {
        event CloseHandler OnClose;
        event RedirectHandler OnRedirect;
        UserControl UserControl { get; }
        void Close();
    }
}