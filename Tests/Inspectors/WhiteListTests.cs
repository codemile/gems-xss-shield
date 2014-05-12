using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield.Inspectors;

namespace XssShieldTests.Inspectors
{
    [TestClass]
    public class WhiteListTests
    {
        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void CleanList_1()
        {
            WhiteList.CleanList(null);
        }

        [TestMethod]
        public void CleanList_2()
        {
            CollectionAssert.AreEquivalent(new[] {"div"}, WhiteList.CleanList(new[] {"DIV"}));
        }

        [TestMethod]
        public void CleanList_3()
        {
            CollectionAssert.AreEquivalent(new[] {"div"}, WhiteList.CleanList(new[] {"DIV", "div"}));
        }

        [TestMethod]
        public void CleanList_4()
        {
            CollectionAssert.AreEquivalent(new[] {"div"}, WhiteList.CleanList(new[] {"DIV", "", "div"}));
        }

        [TestMethod]
        public void CleanList_5()
        {
            CollectionAssert.AreEquivalent(new[] {"p", "img", "div"},
                WhiteList.CleanList(new[] {"P", "img ", "DIV", "", "div"}));
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Construct_1()
        {
            WhiteList list = new WhiteList(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Construct_2()
        {
            WhiteList list = new WhiteList(null, new string[0]);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Construct_3()
        {
            WhiteList list = new WhiteList(new string[0], null);
        }

        [TestMethod]
        public void Construct_4()
        {
            WhiteList list = new WhiteList(new[] {"p", "div", "span"}, new[] {"div"});
            Assert.IsNotNull(list.List);
            Assert.IsNotNull(list.ChildFriendly);
            CollectionAssert.AreEquivalent(new List<string> {"p", "div", "span"}, list.List);
            CollectionAssert.AreEquivalent(new List<string> {"div"}, list.ChildFriendly);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Inspect_1()
        {
            WhiteList list = new WhiteList(new[] { "p", "div", "span" }, new[] { "div" });
            list.Inspect(null);
        }

        [TestMethod]
        public void Inspect_2()
        {
            WhiteList list = new WhiteList(new[] { "p" }, new[] { "div" });

            HtmlNode node = HtmlNode.CreateNode("<div><p>This is a <span>test</span>.</p></div>");

            HtmlNode div = node;
            Assert.IsNotNull(div);
            Assert.AreEqual("div", div.Name);

            HtmlNode p = div.ChildNodes[0];
            Assert.IsNotNull(p);
            Assert.AreEqual(HtmlNodeType.Element,p.NodeType);
            Assert.AreEqual("p",p.Name);

            HtmlNode span = p.ChildNodes[1];
            Assert.IsNotNull(span);
            Assert.AreEqual(HtmlNodeType.Element, span.NodeType);
            Assert.AreEqual("span", span.Name);

            Rejection rejected = list.Inspect(div);
            Assert.IsNotNull(rejected);
            Assert.IsFalse(rejected.RemoveChildren);
            Assert.IsNotNull(rejected.Reason);

            Assert.IsNull(list.Inspect(p));

            rejected = list.Inspect(span);
            Assert.IsNotNull(rejected);
            Assert.IsTrue(rejected.RemoveChildren);
            Assert.IsNotNull(rejected.Reason);
        }
    }
}