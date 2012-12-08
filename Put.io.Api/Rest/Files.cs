using System;
using System.Collections.Generic;
using System.Globalization;
using Put.io.Api.ResponseObjects.Files;
using RestSharp;

namespace Put.io.Api.Rest
{
    public class Files : RestBase
    {
        public Files(string authKey)
        {
            if(string.IsNullOrEmpty(authKey))
                throw new KeyNotFoundException("Auth key not defined");

            AuthKey = authKey;
        }

        /// <summary>
        /// Retrieves a list of files given a parent and authkey. This method is async
        /// </summary>
        /// <param name="parentID">(optional) the parent folder to browse</param>
        /// <param name="callback">The callback method to use once completed</param>
        public void ListFiles(int? parentID, Action<IRestResponse<FileList>> callback)
        {
            var client = GetRestClient();

            var request = new RestRequest(UrlHelper.ListFiles(), Method.GET);

            AddAuthToken(request);

            if (parentID.HasValue && parentID.Value > 0)
                request.AddParameter("parent_id", parentID.Value);

            client.ExecuteAsync(request, callback);
        }

        public void GetFile(int fileID, Action<IRestResponse<GetFileResponse>> callback)
        {
            var client = GetRestClient();

            var request = new RestRequest(UrlHelper.GetFile(), Method.GET);

            AddAuthToken(request);

            request.AddUrlSegment("id", fileID.ToString(CultureInfo.InvariantCulture));

            client.ExecuteAsync(request, callback);
        }

        //Methods to implement:
        // GET /files/<id>          (gets a given file by id)
        // POST /files/<id>/mp4     (begins MP4 conversion)
        // GET /files/<id>/mp4      (gets status of mp4 conversion)
        // GET /files/<id>/download (returns URL in header to the file to download)

        //Non-critical api calls
        // GET /files/search/<query>/page/<page_no>
        // POST /files/upload
        // POST /files/create-folder
        // POST /files/delete
        // POST /files/rename
        // POST /files/move
    }
}