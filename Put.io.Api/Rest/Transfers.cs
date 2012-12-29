using System;
using System.Collections.Generic;
using System.Globalization;
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
            var request = NewRequest(UrlHelper.ListTransfers(), Method.GET);

            RestClient.ExecuteAsync(request, callback);
        }

        public void GetTransfer(int transferID, Action<IRestResponse<GetTransferResponse>> callback)
        {
            var request = NewRequest(UrlHelper.GetTransfer(), Method.GET);

            request.AddUrlSegment("id", transferID.ToString(CultureInfo.InvariantCulture));

            RestClient.ExecuteAsync(request, callback);
        }
    }
}