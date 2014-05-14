using Put.io.Core.Common;

namespace Put.io.Core.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public string Title { get { return "Put.io for Windows Phone"; } }
        public string Description
        {
            get
            {
                return @"Thanks for using this overly simple application – I hope you enjoy it. This application is completely free and open source – feel free to contribute or branch it.

If you like this application, please leave a review or even send me an email – I love to hear feedback.

I dedicate this application to my Fiancé - Devika";
            }
        }
        public string HomeUrl { get { return "http://x-volt.com"; } }
        public string SourceUrl { get { return "https://github.com/Workshop2/Put.io.Wp"; } }
        public string AuthorName { get { return "Simon Colmer"; } }
        public string AuthorEmail { get { return "simon_colmer@hotmail.com"; } }
        public string EmailSubject { get { return "Put.io for Windows Phone Feedback"; } }
        public string EmailBody { get { return "This application is AWESOME....only kidding."; } }
    }
}