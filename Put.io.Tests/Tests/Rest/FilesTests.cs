using System.Linq;
using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Put.io.Tests.Tests.Rest
{
    [TestClass]
    public class FilesTests : WorkItemTest 
    {
        [Asynchronous]
        [TestMethod]
        public void ListFiles()
        {
            var rester = new Api.Rest.Files("**Put.io API Key**");

            rester.ListFiles(null, response =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Data);
                Assert.IsNotNull(response.Data.parent);

                Assert.IsTrue(response.Data.files.Any());
                Assert.IsTrue(response.Data.SafeToContinue());

                EnqueueTestComplete();
            });
        }

        [Asynchronous]
        [TestMethod]
        public void GetFile()
        {
            var rester = new Api.Rest.Files("**Put.io API Key**");

            rester.GetFile(31088044, response =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Data);
                Assert.IsNotNull(response.Data.file);

                Assert.IsTrue(response.Data.SafeToContinue());
                
                EnqueueTestComplete();
            });
        }

        [Asynchronous]
        [TestMethod]
        public void Mp4Status()
        {
            var rester = new Api.Rest.Files("**Put.io API Key**");

            rester.Mp4Status(11908901, response =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Data);
                Assert.IsNotNull(response.Data.mp4);

                Assert.IsTrue(response.Data.SafeToContinue());

                EnqueueTestComplete();
            });
        }

        [Asynchronous]
        [TestMethod]
        public void FileToMp4()
        {
            var rester = new Api.Rest.Files("**Put.io API Key**");

            rester.FileToMp4(11908901, response =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Data);

                Assert.IsTrue(response.Data.SafeToContinue());

                EnqueueTestComplete();
            });
        }

        [Asynchronous]
        [TestMethod]
        public void DownloadFile()
        {
            var rester = new Api.Rest.Files("**Put.io API Key**");

            rester.DownloadFile(47022459, response =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.ResponseUri);

                EnqueueTestComplete();
            });
        }
    }
}
