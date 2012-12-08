using Put.io.Api.UrlHelper;
using RestSharp;

namespace Put.io.Api.Rest
{
    public abstract class RestBase
    {
        protected IUrlHelper UrlHelper { get; set; }
        protected string AuthKey { get; set; }

        protected RestBase()
        {
            UrlHelper = new UrlHelperFactory().GetUrlDetails();
        }

        protected RestClient GetRestClient()
        {
            return new RestClient(UrlHelper.ApiUrl);
        }

        protected void AddAuthToken(RestRequest request)
        {
            request.AddParameter("oauth_token", AuthKey);
        }
    }
}