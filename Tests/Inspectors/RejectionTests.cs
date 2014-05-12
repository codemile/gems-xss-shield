using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield;
using XssShield.Inspectors;

namespace XssShieldTests.Inspectors
{
    [TestClass]
    public class RejectionTests
    {
        [TestMethod]
        public void Constructo_1()
        {
            Rejection reject = new Rejection(true);
            Assert.IsTrue(reject.RemoveChildren);
            Assert.IsNull(reject.Reason);
        }

        [TestMethod]
        public void Constructo_2()
        {
            Rejection reject = new Rejection(false);
            Assert.IsFalse(reject.RemoveChildren);
            Assert.IsNull(reject.Reason);
        }

        [TestMethod]
        public void Constructo_3()
        {
            Rejection reject = new Rejection(true, new RiskDiscovery(0, 0, "test"));
            Assert.IsTrue(reject.RemoveChildren);
            Assert.IsNotNull(reject.Reason);
        }
    }
}