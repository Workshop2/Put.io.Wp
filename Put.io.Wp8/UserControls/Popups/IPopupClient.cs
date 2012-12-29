using System.Windows;
using System.Windows.Controls;

namespace Put.io.Wp8.UserControls.Popups
{
    public delegate void CloseHandler();

    public interface IPopupClient
    {
        event CloseHandler OnClose;
        UserControl UserControl { get; }
    }
}