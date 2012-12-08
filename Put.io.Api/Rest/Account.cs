using System;
using System.Collections.Generic;
using RestSharp;

namespace Put.io.Api.Rest
{
    public class Account : RestBase
    {
        public Account(string authKey)
        {
            if(string.IsNullOrEmpty(authKey))
                throw new KeyNotFoundException("Auth key not defined");

            AuthKey = authKey;
        }

        public void GetAccountInfo(Action<IRestResponse<ResponseObjects.Account.AccountInfo>> callback)
        {
            var client = GetRestClient();

            var request = new RestRequest(UrlHelper.AccountInfo(), Method.GET);

            AddAuthToken(request);

            client.ExecuteAsync(request, callback);
        }
    }
}