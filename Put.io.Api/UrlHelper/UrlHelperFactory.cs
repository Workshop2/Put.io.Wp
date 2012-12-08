namespace Put.io.Api.UrlHelper
{
    public class UrlHelperFactory
    {
        public IUrlHelper GetUrlDetails()
        {
            return new StandardUrlSetup();
        }
    }
}