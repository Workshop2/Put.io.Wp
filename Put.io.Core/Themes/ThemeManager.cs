using System;
using System.IO;
using System.Windows;

namespace Put.io.Core.Themes
{
    public class ThemeManager : IThemeManager
    {
        private string RootDirectory { get; set; }

        public ThemeManager()
        {
            var lightTheme = (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible;

            SetRootDirectory(lightTheme);
        }

        public ThemeManager(bool lightTheme)
        {
            SetRootDirectory(lightTheme);
        }

        private void SetRootDirectory(bool lightTheme)
        {
            RootDirectory = lightTheme ? "/Assets/Light/" : "/Assets/Dark/";
        }

        private Uri GetUri(string fileName)
        {
            return new Uri(Path.Combine(RootDirectory, fileName), UriKind.Relative);
        }

        private Uri _refreshIcon;
        public Uri RefreshIcon
        {
            get { return _refreshIcon ?? (_refreshIcon = GetUri("Refresh.png")); }
        }

        private Uri _settingsIcon;
        public Uri SettingsIcon
        {
            get { return _settingsIcon ?? (_settingsIcon = GetUri("Settings.png")); }
        }
    }
}