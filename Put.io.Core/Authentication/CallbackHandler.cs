using System;
using System.Linq;

namespace Put.io.Core.Authentication
{
    public class CallbackHandler
    {
        private readonly string[] _accessToken = { "/#access_token=", "/#token=" };

         public CallbackResult ParseAccessToken(string url)
         {
             var callbackUrl = new Api.UrlHelper.UrlHelperFactory().GetUrlDetails().CallbackUrl;

             if (url.StartsWith(callbackUrl, StringComparison.InvariantCultureIgnoreCase))
             {
                 var trailing = url.Substring(url.IndexOf(callbackUrl, StringComparison.InvariantCultureIgnoreCase) + callbackUrl.Length);
                 var toFilter = ContainsAccessToken(trailing);

                 if (!string.IsNullOrEmpty(toFilter))
                 {
                     var token = trailing.Substring(trailing.IndexOf(toFilter, StringComparison.InvariantCultureIgnoreCase) + toFilter.Length);

                     return new CallbackResult {Status = CallbackStatus.Success, Token = token};
                 }
             }

             return new CallbackResult {Status = CallbackStatus.Failed}; 
         }

        private string ContainsAccessToken(string trailing)
        {
            return _accessToken.FirstOrDefault(x => trailing.StartsWith(x, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}