namespace Put.io.Api.UrlHelper
{
    public interface IUrlHelper
    {
        int ClientID { get; }
        string CallbackUrl { get; }
        string ApiUrl { get; }

        string AuthenticateUrl();

        //File API
        string ListFiles();
        string GetFile();
        string FileMp4();
        string DownloadFile();

        //Transfer API
        string ListTransfers();
        string GetTransfer();
        string CancelTransfers();

        //Account Info API
        string AccountInfo();
    }
}