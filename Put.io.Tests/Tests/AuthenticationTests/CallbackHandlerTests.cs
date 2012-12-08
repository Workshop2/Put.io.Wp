using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Put.io.Api.Authentication;

namespace Put.io.Tests.Tests.AuthenticationTests
{
    [Tag("AuthenticationTests")]
    public class CallbackHandlerTests
    {
        public static class given
        {
            public abstract class a_standard_setup : SpecsFor<CallbackHandler>
            {
                protected CallbackResult Result { get; set; }

                public override CallbackHandler InitializeClassUnderTest()
                {
                    return new CallbackHandler();
                }

                protected override void When()
                {
                    Result = SUT.ParseAccessToken(InputUrl);
                }

                protected virtual string InputUrl { get { return "http://x-volt.com/#access_token=" + Token; } }
                protected virtual string Token { get { return "abc1234"; } }
            }

            public abstract class an_incorrect_setup : a_standard_setup
            {
                protected override string InputUrl
                {
                    get { return "http://google.com/"; }
                }
            }
        }

        [TestClass]
        public class when_parsing_a_correct_response_url : given.a_standard_setup
        {
            [TestMethod]
            public void then_should_return_token()
            {
                Assert.AreEqual(Result.Token, Token);
            }

            [TestMethod]
            public void then_should_return_success()
            {
                Assert.AreEqual(Result.Status, CallbackStatus.Success);
            }
        }

        [TestClass]
        public class when_parsing_an_incorrect_response_url : given.an_incorrect_setup
        {
            [TestMethod]
            public void then_should_return_token()
            {
                Assert.IsTrue(string.IsNullOrEmpty(Result.Token));
            }

            [TestMethod]
            public void then_should_return_success()
            {
                Assert.AreEqual(Result.Status, CallbackStatus.Failed);
            }
        }
    }
}