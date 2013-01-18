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
        string StreamFile();
        string DownloadMp4();
        string StreamMp4();

        //Transfer API
        string ListTransfers();
        string GetTransfer();
        string CancelTransfers();

        //Account Info API
        string AccountInfo();
    }
}