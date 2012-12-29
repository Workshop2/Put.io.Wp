using System;

namespace Put.io.Core.Themes
{
    public interface IThemeManager
    {
        Uri RefreshIcon { get; }
        Uri SettingsIcon { get; }
    }
}