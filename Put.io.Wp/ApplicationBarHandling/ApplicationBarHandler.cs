using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Shell;
using Put.io.Core.InvokeSynchronising;

namespace Put.io.Wp.ApplicationBarHandling
{
    public delegate void ButtonClickHandler(ApplicationBarButtons button);

    public class ApplicationBarHandler
    {
        private IApplicationBar ApplicationBar { get; set; }
        private IDictionary<ApplicationBarButtons, ApplicationBarIconButton> ButtonCache { get; set; }
        public event ButtonClickHandler OnClick;

        public ApplicationBarHandler(IApplicationBar applicationBar)
        {
            ApplicationBar = applicationBar;
            ButtonCache = new Dictionary<ApplicationBarButtons, ApplicationBarIconButton>();

            ButtonCache.Add(GetButton(@"/Assets/AppBar/refresh.png", "Refresh", ApplicationBarButtons.Refresh));
            ButtonCache.Add(GetButton(@"/Assets/AppBar/settings.png", "Settings", ApplicationBarButtons.Settings));
            ButtonCache.Add(GetButton(@"/Assets/AppBar/delete.png", "Cleanup", ApplicationBarButtons.Cleanup));
            ButtonCache.Add(GetButton(@"/Assets/AppBar/select.png", "Select All", ApplicationBarButtons.SelectAll));
            ButtonCache.Add(GetButton(@"/Assets/AppBar/film.png", "to Mp4", ApplicationBarButtons.Convert));
            ButtonCache.Add(GetButton(@"/Assets/AppBar/select.png", "Select Many", ApplicationBarButtons.Select));
            ButtonCache.Add(GetButton(@"/Assets/AppBar/delete.png", "delete", ApplicationBarButtons.Delete));
        }

        private KeyValuePair<ApplicationBarButtons, ApplicationBarIconButton> GetButton(string icon, string text, ApplicationBarButtons buttonType)
        {
            var button = new ApplicationBarIconButton { IconUri = new Uri(icon, UriKind.Relative), Text = text };
            button.Click += (sender, args) =>
            {
                if (OnClick != null)
                    OnClick(buttonType);
            };

            return new KeyValuePair<ApplicationBarButtons, ApplicationBarIconButton>(buttonType, button);
        }

        public void DisplayButtons(IEnumerable<ApplicationBarButtons> buttons)
        {
            ApplicationBar.Buttons.Clear();

            foreach (var button in buttons)
            {
                var cachedButton = ButtonCache[button];

                if (cachedButton != null)
                    ApplicationBar.Buttons.Add(cachedButton);
            }
        }

    }
}