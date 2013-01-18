using System.Windows.Controls;

namespace Put.io.Wp.UserControls.Popups
{
    public delegate void CloseHandler();

    public interface IPopupClient
    {
        event CloseHandler OnClose;
        UserControl UserControl { get; }
    }
}