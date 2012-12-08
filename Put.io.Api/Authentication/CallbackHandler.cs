using System;

namespace Put.io.Api.Authentication
{
    public class CallbackHandler
    {
        private const string AccessToken = "/#access_token=";

         public CallbackResult ParseAccessToken(string url)
         {
             var callbackUrl = new UrlHelper.UrlHelperFactory().GetUrlDetails().CallbackUrl;

             if (url.StartsWith(callbackUrl, StringComparison.InvariantCultureIgnoreCase))
             {
                 var trailing = url.Substring(url.IndexOf(callbackUrl, StringComparison.InvariantCultureIgnoreCase) + callbackUrl.Length);

                 if (trailing.StartsWith(AccessToken, StringComparison.InvariantCultureIgnoreCase))
                 {
                     var token = trailing.Substring(trailing.IndexOf(AccessToken, StringComparison.InvariantCultureIgnoreCase) + AccessToken.Length);

                     return new CallbackResult {Status = CallbackStatus.Success, Token = token};
                 }
             }

             return new CallbackResult {Status = CallbackStatus.Failed}; 
         }
    }
}