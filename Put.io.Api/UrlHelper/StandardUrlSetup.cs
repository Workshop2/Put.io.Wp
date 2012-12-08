using System.Text;

namespace Put.io.Api.UrlHelper
{
    public class StandardUrlSetup : IUrlHelper
    {
        private string Secret { get; set; }
        public int ClientID { get; private set; }
        public string CallbackUrl { get; private set; }
        public string ApiUrl { get; set; }

        public StandardUrlSetup()
        {
            //TODO: Encryption?
            Secret = "8xe6glmr6df9ylyg8ik2";
            ClientID = 239;
            CallbackUrl = "http://x-volt.com";

            ApiUrl = "https://api.put.io/v2";
        }

        public string AuthenticateUrl()
        {
            var sb = new StringBuilder();

            sb.Append(ApiUrl);
            sb.Append("/oauth2/authenticate?client_id=");
            sb.Append(ClientID);
            sb.Append("&response_type=token");
            sb.Append("&redirect_uri=");
            sb.Append(CallbackUrl);

            return sb.ToString();
        }

        public string ListFiles()
        {
            return "/files/list";
        }

        public string GetFile()
        {
            return "/files/{id}";
        }

        public string ListTransfers()
        {
            return "/transfers/list";
        }
        public string AccountInfo()
        {
            return "/account/info";
        }
    }
}