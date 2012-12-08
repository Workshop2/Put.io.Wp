using System;
using System.Collections.Generic;
using Put.io.Api.ResponseObjects.Transfers;
using RestSharp;

namespace Put.io.Api.Rest
{
    public class Transfers : RestBase
    {
        public Transfers(string authKey)
        {
            if(string.IsNullOrEmpty(authKey))
                throw new KeyNotFoundException("Auth key not defined");

            AuthKey = authKey;
        }

        public void ListTransfers(Action<IRestResponse<TransferList>> callback)
        {
            var client = GetRestClient();

            var request = new RestRequest(UrlHelper.ListTransfers(), Method.GET);

            AddAuthToken(request);

            client.ExecuteAsync(request, callback);
        }
    }
}