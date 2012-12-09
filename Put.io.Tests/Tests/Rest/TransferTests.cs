using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Put.io.Tests.Tests.Rest
{
    [TestClass]
    public class TransferTests : WorkItemTest
    {

        [Asynchronous]
        [TestMethod]
        public void ListTransfers()
        {
            var rester = new Api.Rest.Transfers("PUTIO_KEY");

            rester.ListTransfers(response =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Data);
                Assert.IsNotNull(response.Data.Transfers);

                Assert.IsTrue(response.Data.SafeToContinue());
                EnqueueTestComplete();
            });
        }
    }
}