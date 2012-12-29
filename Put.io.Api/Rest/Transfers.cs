using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Put.io.Api.ResponseObjects.Transfers;
using RestSharp;

namespace Put.io.Api.Rest
{
    public class Transfers : RestBase
    {
        public Transfers(string authKey)
        {
            if (string.IsNullOrEmpty(authKey))
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

        public void DeleteTransfers(List<int> transferIDs, Action<IRestResponse<GetTransferResponse>> callback)
        {
            var request = NewRequest(UrlHelper.CancelTransfers(), Method.POST);
            
            var toDelete = transferIDs.Aggregate(string.Empty, (current, transfer) => current + transfer + ",");
            toDelete = toDelete.Substring(0, toDelete.Length - 1);

            request.AddParameter("transfer_ids", toDelete, ParameterType.GetOrPost);

            RestClient.ExecuteAsync(request, callback);
        }
    }
}