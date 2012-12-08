using System;
using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Put.io.Tests.Tests.UrlHelperTests
{
    [Tag("UrlHelperTests")]
    [TestClass]
    public class when_testing_standard_authenticate_url
    {
        [TestMethod]
        public void then_result_should_match_expected_url()
        {
            var target = new Api.UrlHelper.StandardUrlSetup();
            var result = target.AuthenticateUrl();
            const string expected = @"https://api.put.io/v2/oauth2/authenticate?client_id=239&response_type=token&redirect_uri=http://x-volt.com";

            Assert.IsTrue(result.Equals(expected, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}