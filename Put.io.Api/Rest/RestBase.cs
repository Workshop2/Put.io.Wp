using System;
using System.Globalization;
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

        private IRestClient _restClient;
        protected IRestClient RestClient
        {
            get
            {
                return _restClient ?? (_restClient = new RestClient(UrlHelper.ApiUrl) { Authenticator = new OAuth2UriQueryParameterAuthenticator(AuthKey) });
            }
        }

        protected RestRequest NewRequest(string path, Method method)
        {
            var request = new RestRequest(path, method);
            request.AddParameter("no-cache", DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture), ParameterType.GetOrPost);

            return request;
        }
    }
}