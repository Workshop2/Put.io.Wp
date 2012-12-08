using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Put.io.Tests.Tests.Rest
{
    [TestClass]
    public class ListFilesTests
    {
        [TestMethod]
        public void adsad()
        {
            var rester = new Api.Rest.Files("PUTIO_KEY");

            rester.ListFiles(null, response =>
            {
                Console.WriteLine("test");
            });
        }

        [TestMethod]
        public void dddddd()
        {
            var rester = new Api.Rest.Transfers("PUTIO_KEY");

            rester.ListTransfers(response =>
            {
                Console.WriteLine("test");
            });
        }

        [TestMethod]
        public void asdsdasdasd()
        {
            var rester = new Api.Rest.Account("PUTIO_KEY");

            rester.GetAccountInfo(response =>
            {
                Console.WriteLine("test");
            });
        }

        [TestMethod]
        public void ssdsssd()
        {
            var rester = new Api.Rest.Files("PUTIO_KEY");

            rester.GetFile(31088044, response =>
            {
                Console.WriteLine("test");
            });
        }
    }
}