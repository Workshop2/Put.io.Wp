using System.Collections.Generic;

namespace Put.io.Api.ResponseObjects.Transfers
{
    public class TransferList : BaseObject
    {
        public List<Transfer> Transfers { get; set; }
    }
}