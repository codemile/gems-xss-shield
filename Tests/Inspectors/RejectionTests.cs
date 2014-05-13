using System;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield;
using XssShield.Inspectors;

namespace XssShieldTests.Inspectors
{
    [TestClass]
    public class RejectionTests
    {
        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Construct_1()
        {
            Rejection reject = new Rejection(true, null);
        }

        [TestMethod]
        public void Construct_2()
        {
            Rejection reject = new Rejection(true, HtmlNode.CreateNode("<br>"));
            Assert.IsTrue(reject.RemoveChildren);
            Assert.IsNull(reject.Reason);
        }

        [TestMethod]
        public void Construct_3()
        {
            Rejection reject = new Rejection(false, HtmlNode.CreateNode("<br>"));
            Assert.IsFalse(reject.RemoveChildren);
            Assert.IsNull(reject.Reason);
        }

        [TestMethod]
        public void Construct_4()
        {
            Rejection reject = new Rejection(true, HtmlNode.CreateNode("<br>"), new RiskDiscovery(0, 0, "test"));
            Assert.IsTrue(reject.RemoveChildren);
            Assert.IsNotNull(reject.Reason);
        }
    }
}