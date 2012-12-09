using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Put.io.Tests.Tests.Rest
{
    [TestClass]
    public class AccountTests : WorkItemTest
    {
        [Asynchronous]
        [TestMethod]
        public void GetAccountInfo()
        {
            var rester = new Api.Rest.Account("PUTIO_KEY");

            rester.GetAccountInfo(response =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Data);
                Assert.IsNotNull(response.Data.Info);

                Assert.IsTrue(response.Data.SafeToContinue());
                EnqueueTestComplete();
            });
        }
    }
}