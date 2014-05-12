using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield.Inspectors;

namespace XssShieldTests.Inspectors
{
    [TestClass]
    public class AttributeWhiteListTests
    {
        private static void AssertHas(HtmlNode pNode, string pName, string pValue)
        {
            Assert.IsNotNull(pNode);
            Assert.IsNotNull(pName);
            Assert.IsNotNull(pValue);
            Assert.IsTrue(pNode.Attributes.Contains(pName));
            Assert.AreEqual(pValue, pNode.Attributes[pName].Value);
            Assert.AreEqual(1, pNode.Attributes.Count(pAttr=>pAttr.Name == pName));
        }

        private static void AssertHasNot(HtmlNode pNode, string pName)
        {
            Assert.IsNotNull(pNode);
            Assert.IsNotNull(pNode);
            Assert.IsFalse(pNode.Attributes.Contains(pName));
        }

        [TestMethod]
        public void FilterAttributes_1()
        {
            AttributeWhiteList list = new AttributeWhiteList(new AttributeList {{"p", new List<string> {"class"}}});

            HtmlNode node =
                list.FilterAttributes(HtmlNode.CreateNode("<p class='cgTag' onclick='#'>This is my text.</p>"));

            AssertHas(node, "class", "cgTag");
            AssertHasNot(node, "onclick");
        }

        [TestMethod]
        public void FilterAttributes_2()
        {
            AttributeWhiteList list = new AttributeWhiteList(new AttributeList { { "p", new List<string> { "class" } } });

            HtmlNode node =
                list.FilterAttributes(HtmlNode.CreateNode("<p class='cgTag' class='cgOtherThing'>This is my text.</p>"));

            AssertHas(node, "class", "cgTag");
        }

        [TestMethod]
        public void FilterAttributes_3()
        {
            AttributeWhiteList list = new AttributeWhiteList(new AttributeList { { "p", new List<string> { "class" } } });

            HtmlNode node =
                list.FilterAttributes(HtmlNode.CreateNode("<div class='cgTag'><p>This should be the text.</p></div>"));

            AssertHasNot(node, "class");
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void InspectNode_1()
        {
            AttributeWhiteList list = new AttributeWhiteList(AttributeWhiteList.Minimum);
            list.InspectNode(null);
        }

        [TestMethod]
        public void InspectNode_2()
        {
            AttributeWhiteList list = new AttributeWhiteList(AttributeWhiteList.Minimum);

            Assert.IsFalse(list.InspectNode(new HtmlNode(HtmlNodeType.Element, new HtmlDocument(), 0)));
            Assert.IsFalse(list.InspectNode(new HtmlNode(HtmlNodeType.Comment, new HtmlDocument(), 0)));
            Assert.IsFalse(list.InspectNode(new HtmlNode(HtmlNodeType.Document, new HtmlDocument(), 0)));
            Assert.IsFalse(list.InspectNode(new HtmlNode(HtmlNodeType.Text, new HtmlDocument(), 0)));
        }

        [TestMethod]
        public void InspectNode_3()
        {
            AttributeWhiteList list = new AttributeWhiteList(AttributeWhiteList.Minimum);

            Assert.IsTrue(list.InspectNode(HtmlNode.CreateNode("<p class='cgTag'>This is my text.</p>")));
        }

        [TestMethod]
        public void InspectNode_4()
        {
            AttributeWhiteList list = new AttributeWhiteList(AttributeWhiteList.Minimum);

            Assert.IsFalse(list.InspectNode(HtmlNode.CreateNode("<body class='cgTag'><p>This is inner text.</p></body>")));
        }
    }
}