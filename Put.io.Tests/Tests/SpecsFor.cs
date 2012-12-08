using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Put.io.Tests.Tests
{
    public abstract class SpecsFor<T>
    {
        protected T SUT;

        [TestInitialize]
        public void Setup()
        {
            SUT = InitializeClassUnderTest();
            Given();
            When();
        }

        public abstract T InitializeClassUnderTest();

        protected virtual void Given()
        {
            
        }

        protected virtual void When()
        {

        }
    }
}