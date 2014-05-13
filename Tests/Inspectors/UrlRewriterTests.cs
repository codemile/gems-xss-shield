using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield.Inspectors;

namespace XssShieldTests.Inspectors
{
    [TestClass]
    public class UrlRewriterTests
    {
        private void CleanList(Dictionary<string, string> pList)
        {
            foreach (KeyValuePair<string, string> pair in pList)
            {
                string url = UrlRewriter.CleanURL(pair.Key, false);
                Assert.IsNotNull(url, pair.Key);
                Assert.AreEqual(pair.Value, url);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Clean_1()
        {
            UrlRewriter.CleanURL(null, false);
        }

        [TestMethod]
        public void Clean_2()
        {
            Dictionary<string, string> compare = new Dictionary<string, string>
                                                 {
                                                     {"http://www.thinkingmedia.ca", "http://www.thinkingmedia.ca/"},
                                                     {
                                                         "http://www.THINKINGMEDIA.ca/about/things",
                                                         "http://www.thinkingmedia.ca/about/things"
                                                     }
                                                 };
            CleanList(compare);
        }

        [TestMethod]
        public void Clean_3()
        {
            Dictionary<string, string> compare = new Dictionary<string, string>
                                                 {
                                                     {
                                                         "http://www.THINKINGMEDIA.ca/about us",
                                                         "http://www.thinkingmedia.ca/about%20us"
                                                     },
                                                     {
                                                         "http://www.thinking.ca:8080/index.php?find=some thing&that thing",
                                                         "http://www.thinking.ca:8080/index.php?find=some%20thing&that%20thing"
                                                     }
                                                 };
            CleanList(compare);
        }

        [TestMethod]
        public void Clean_4()
        {
            Dictionary<string, string> compare = new Dictionary<string, string>
                                                 {
                                                     {
                                                         "https://mathew:password@www.thinking.ca",
                                                         "https://www.thinking.ca/"
                                                     },
                                                     {
                                                         "http://mathew%40yaoo.com:password@www.thinking.ca/#thing",
                                                         "http://www.thinking.ca/#thing"
                                                     },
                                                 };
            CleanList(compare);
        }

        [TestMethod]
        public void Clean_5()
        {
            List<string> xssUrls = new List<string>
                                   {
                                       "javascript:alert('hello');",
                                       "JaVaScRiPt:alert('XSS')",
                                       "javascript:alert(\"RSnake says, 'XSS'\")",
                                       "#",
                                       "&#106;&#97;&#118;&#97;&#115;&#99;&#114;&#105;&#112;&#116;&#58;&#97;&#108;&#101;&#114;&#116;&#40;&#39;&#88;&#83;&#83;&#39;&#41;",
                                       "&#0000106&#0000097&#0000118&#0000097&#0000115&#0000099&#0000114&#0000105&#0000112&#0000116&#0000058&#0000097&#0000108&#0000101&#0000114&#0000116&#0000040&#0000039&#0000088&#0000083&#0000083&#0000039&#0000041",
                                       "&#x6A&#x61&#x76&#x61&#x73&#x63&#x72&#x69&#x70&#x74&#x3A&#x61&#x6C&#x65&#x72&#x74&#x28&#x27&#x58&#x53&#x53&#x27&#x29",
                                       "jav\tascript:alert('XSS');",
                                       "jav&#x0A;ascript:alert('XSS');",
                                       "jav&#x0D;ascript:alert('XSS');",
                                       " &#14;  javascript:alert('XSS');",
                                       "vbscript:msgbox(\"XSS\")",
                                       "livescript:[code]",
                                       "&{alert('XSS')}",
                                       "xss:expression(alert('XSS'))",
                                       "behavior: url(xss.htc);",
                                       "¼script¾alert(¢XSS¢)¼/script¾"
                                   };

            foreach (string bad in xssUrls)
            {
                Assert.IsNull(UrlRewriter.CleanURL(bad, false));
            }
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Construct_1()
        {
            UrlRewriter url = new UrlRewriter(null, false);
        }

        [TestMethod]
        public void Construct_2()
        {
            UrlRewriter url = new UrlRewriter(UrlRewriter.Basic, false);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void InspectNode_1()
        {
            UrlRewriter url = new UrlRewriter(UrlRewriter.Basic, false);
            url.InspectNode(null);
        }

        [TestMethod]
        public void InspectNode_2()
        {
            UrlRewriter url = new UrlRewriter(UrlRewriter.Basic, false);
            Assert.IsTrue(url.InspectNode(HtmlNode.CreateNode("<img src=''/>")));
            Assert.IsTrue(url.InspectNode(HtmlNode.CreateNode("<a href='#'>This is a test</a>")));
            Assert.IsFalse(url.InspectNode(HtmlNode.CreateNode("<span>")));
            Assert.IsFalse(url.InspectNode(HtmlNode.CreateNode("<span src=''></span>")));
        }
    }
}