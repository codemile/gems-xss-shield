using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield;

namespace XssShieldTests
{
    [TestClass]
    public class SanitizedTests
    {
        [TestMethod]
        public void Constructor_1()
        {
            Sanitized san = new Sanitized();
            Assert.IsNotNull(san.Clean);
            Assert.IsNotNull(san.Risks);
            Assert.AreEqual(0,san.Risks.Count);
            Assert.IsFalse(san.Dangerous);
            Assert.IsNull(san.Document);
        }

        [TestMethod]
        public void Add_1()
        {
            Sanitized san = new Sanitized();
            san.Add(new RiskDiscovery(0,0,"test"));
            Assert.AreEqual(1,san.Risks.Count);
            Assert.IsTrue(san.Dangerous);
        }
    }
}