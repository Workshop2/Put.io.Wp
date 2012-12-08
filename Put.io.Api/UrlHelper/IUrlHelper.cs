namespace Put.io.Api.UrlHelper
{
    public interface IUrlHelper
    {
        int ClientID { get; }
        string CallbackUrl { get; }
        string ApiUrl { get; set; }

        string AuthenticateUrl();

        //File API
        string ListFiles();
        string GetFile();

        //Transfer API
        string ListTransfers();

        //Account Info API
        string AccountInfo();
    }
}