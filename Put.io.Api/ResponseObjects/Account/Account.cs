namespace Put.io.Api.ResponseObjects.Account
{
    public class Account : BaseObject
    {
        public string username { get; set; }
        public string mail { get; set; }
        public Disk disk { get; set; }
    }
}