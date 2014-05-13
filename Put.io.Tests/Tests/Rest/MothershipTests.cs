using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Put.io.Api.Rest;

namespace Put.io.Tests.Tests.Rest
{
    [TestClass]
    [Tag("mother")]
    public class MothershipTests : WorkItemTest
    {
        [Asynchronous]
        [TestMethod]
        public void RunTest()
        {
            var rester = new Api.Rest.Mothership();
            var data = new Mothership.Data { DeviceUniqueId = "lalala" };

            rester.Fire(data);
        }
    }
}