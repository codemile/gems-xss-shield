using System;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield;
using XssShield.Inspectors;
using XssShieldTests.Mock;

namespace XssShieldTests.Inspectors
{
    [TestClass]
    public class InspectorCollectionTests
    {
        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Inspect_1()
        {
            InspectorCollection list = new InspectorCollection();
            list.Inspect(null);
        }

        [TestMethod]
        public void Inspect_2()
        {
            MockInspector mock = new MockInspector();
            InspectorCollection list = new InspectorCollection {mock};

            Rejection reject = list.Inspect(HtmlNode.CreateNode("<br>"));
            Assert.IsNull(reject);
            Assert.AreEqual(1,mock.Count);
        }

        [TestMethod]
        public void Inspect_3()
        {
            MockInspector mock = new MockInspector(new Rejection(true, HtmlNode.CreateNode("<span>"), new RiskDiscovery("test")));
            InspectorCollection list = new InspectorCollection { mock };

            Rejection reject = list.Inspect(HtmlNode.CreateNode("<br>"));
            Assert.IsNotNull(reject);
            Assert.AreEqual(1, mock.Count);
            Assert.AreEqual("test",reject.Reason.Message);
            Assert.AreEqual("br",reject.Node.Name);
        }

        [TestMethod]
        public void Inspect_4()
        {
            MockInspector mock1 = new MockInspector();
            MockInspector mock2 = new MockInspector(new Rejection(true, HtmlNode.CreateNode("<span>"), new RiskDiscovery("test")));

            InspectorCollection list = new InspectorCollection { mock1, mock2 };

            Rejection reject = list.Inspect(HtmlNode.CreateNode("<br>"));
            Assert.IsNotNull(reject);
            Assert.AreEqual(1, mock1.Count);
            Assert.AreEqual(1, mock2.Count);
            Assert.AreEqual("test", reject.Reason.Message);
            Assert.AreEqual("br", reject.Node.Name);
        }

        [TestMethod]
        public void Inspect_5()
        {
            MockInspector mock1 = new MockInspector();
            MockInspector mock2 = new MockInspector(new Rejection(true, HtmlNode.CreateNode("<span>"), new RiskDiscovery("test")));

            InspectorCollection list = new InspectorCollection { mock2, mock1 };

            Rejection reject = list.Inspect(HtmlNode.CreateNode("<br>"));
            Assert.IsNotNull(reject);
            Assert.AreEqual(1, mock2.Count);
            Assert.AreEqual(0, mock1.Count);
            Assert.AreEqual("test", reject.Reason.Message);
            Assert.AreEqual("br", reject.Node.Name);
        }

        [TestMethod]
        public void Inspect_6()
        {
            MockInspector mock1 = new MockInspector();
            MockInspector mock2 = new MockInspector();
            MockInspector mock3 = new MockInspector();
            MockInspector mock4 = new MockInspector();
            InspectorCollection list = new InspectorCollection { mock1, mock2, mock3, mock4 };

            Rejection reject = list.Inspect(HtmlNode.CreateNode("<br>"));
            Assert.IsNull(reject);
            Assert.AreEqual(1, mock1.Count);
            Assert.AreEqual(1, mock2.Count);
            Assert.AreEqual(1, mock3.Count);
            Assert.AreEqual(1, mock4.Count);
        }

        [TestMethod]
        public void Inspect_7()
        {
            MockInspector mock1 = new MockInspector();
            MockInspector mock2 = new MockInspector();
            MockInspector mock3 = new MockInspector(new Rejection(true, HtmlNode.CreateNode("<span>"), new RiskDiscovery("test")));
            MockInspector mock4 = new MockInspector();
            InspectorCollection list = new InspectorCollection { mock1, mock2, mock3, mock4 };

            Rejection reject = list.Inspect(HtmlNode.CreateNode("<br>"));
            Assert.IsNotNull(reject);
            Assert.AreEqual(1, mock1.Count);
            Assert.AreEqual(1, mock2.Count);
            Assert.AreEqual(1, mock3.Count);
            Assert.AreEqual(0, mock4.Count);
            Assert.AreEqual("test", reject.Reason.Message);
            Assert.AreEqual("br", reject.Node.Name);
        }

    }
}