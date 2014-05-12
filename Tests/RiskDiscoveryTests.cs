using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield;

namespace XssShieldTests
{
    [TestClass]
    public class RiskDiscoveryTests
    {
        [TestMethod]
        public void Constructor_1()
        {
            RiskDiscovery risk = new RiskDiscovery("test");
            Assert.AreEqual("test",risk.Message);
            Assert.AreEqual(-1, risk.Line);
            Assert.AreEqual(-1, risk.Column);
        }

        [TestMethod]
        public void Constructor_2()
        {
            RiskDiscovery risk = new RiskDiscovery(10,100,"test");
            Assert.AreEqual("test", risk.Message);
            Assert.AreEqual(10, risk.Line);
            Assert.AreEqual(100, risk.Column);
        }

        [TestMethod]
        public void Create_1()
        {
            RiskDiscovery risk = RiskDiscovery.Create(HtmlNode.CreateNode("<p>This <span>is a</span> test.</p>").ChildNodes.FindFirst("span"), "test");
            Assert.IsNotNull(risk);
            Assert.AreEqual("test",risk.Message);
            Assert.AreEqual(1, risk.Line);
            Assert.AreEqual(9, risk.Column);
        }
    }
}