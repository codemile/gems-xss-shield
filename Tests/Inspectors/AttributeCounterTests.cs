using System;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XssShield.Inspectors.Tests
{
    [TestClass]
    public class AttributeCounterTests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Construct_1()
        {
            AttributeCounter counter = new AttributeCounter(null);
        }

        [TestMethod]
        public void Inspect_1()
        {
            AttributeCounter counter = new AttributeCounter("test-", "div", "id", 1);

            HtmlNode node = HtmlNode.CreateNode("<div></div>");
            Rejection rejection = counter.Inspect(node);
            Assert.IsNull(rejection);
            Assert.AreEqual("test-1", node.Id);
        }
    }
}