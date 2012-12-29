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
            var request = NewRequest(UrlHelper.AccountInfo(), Method.GET);

            RestClient.ExecuteAsync(request, callback);
        }
    }
}