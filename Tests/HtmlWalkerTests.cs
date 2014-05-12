using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield;

namespace XssShieldTests
{
    [TestClass]
    public class HtmlWalkerTests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Constructor_1()
        {
            HtmlWalker walk = new HtmlWalker(null,Encoding.UTF8);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Constructor_2()
        {
            HtmlWalker walk = new HtmlWalker("", null);
        }

        [TestMethod]
        public void Constructor_3()
        {
            const string str = "<p>This is my html</p>";
            HtmlWalker walk = new HtmlWalker(str, Encoding.UTF8);

            Assert.IsNotNull(walk.Orginial);
            Assert.AreEqual(str,walk.Orginial);
            Assert.IsNotNull(walk.Document);
            Assert.IsNotNull(walk.Document.DocumentNode);
            Assert.AreEqual(1,walk.Document.DocumentNode.ChildNodes.Count);
        }

        [TestMethod]
        public void Constructor_4()
        {
            const string str = "This is not html";
            HtmlWalker walk = new HtmlWalker(str, Encoding.UTF8);

            Assert.IsNotNull(walk.Orginial);
            Assert.AreEqual(str, walk.Orginial);
            Assert.IsNotNull(walk.Document);
            Assert.IsNotNull(walk.Document.DocumentNode);
            Assert.AreEqual(1, walk.Document.DocumentNode.ChildNodes.Count);
        }

        [TestMethod]
        public void Execute_1()
        {
            const string str = "<p>This is my html</p><p>Another paragraph <!-- with a comment --> inside it.</p><p>Again another</p>";
            HtmlWalker walk = new HtmlWalker(str, Encoding.UTF8);

            Dictionary<HtmlNodeType,int> count = new Dictionary<HtmlNodeType, int>
                                                 {
                                                     {HtmlNodeType.Comment,0},
                                                     {HtmlNodeType.Element,0},
                                                     {HtmlNodeType.Document,0},
                                                     {HtmlNodeType.Text,0}
                                                 };

            walk.Execute((pResult, pNode) =>
                         {
                             Assert.IsNotNull(pResult);
                             Assert.IsNotNull(pNode);
                             count[pNode.NodeType]++;
                         });

            Assert.AreEqual(1, count[HtmlNodeType.Comment]);
            Assert.AreEqual(3, count[HtmlNodeType.Element]);
            Assert.AreEqual(1, count[HtmlNodeType.Document]);
            Assert.AreEqual(4, count[HtmlNodeType.Text]);
        }

        [TestMethod]
        public void Execute_2()
        {
            const string str = "<html><head></head><body><div><p>Hello</p><p>World</p></div></body></html>";
            HtmlWalker walk = new HtmlWalker(str, Encoding.UTF8);

            Dictionary<string, int> childCount = new Dictionary<string, int>
                                                {
                                                    {"#document",1},
                                                    {"#text",0},
                                                    {"html",2},
                                                    {"head",0},
                                                    {"body",1},
                                                    {"div",2},
                                                    {"p",1}
                                                };

            int count = 0;
            walk.Execute((pResult, pNode) =>
            {
                Assert.IsNotNull(pResult);
                Assert.IsNotNull(pNode);
                Assert.IsTrue(childCount.ContainsKey(pNode.Name), pNode.Name);
                Assert.AreEqual(childCount[pNode.Name],pNode.ChildNodes.Count,pNode.Name);

                count++;
            });

            Assert.AreEqual(9, count);
        }

    }
}