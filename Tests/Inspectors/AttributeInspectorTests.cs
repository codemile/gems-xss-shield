using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield.Inspectors;
using XssShieldTests.Mock;

namespace XssShieldTests.Inspectors
{
    [TestClass]
    public class AttributeInspectorTests
    {
        private static readonly AttributeList _values = new AttributeList
                                                        {
                                                            {"p", new List<string> {"align"}},
                                                            {"img", new List<string> {"src", "width", "height"}}
                                                        };

        [TestMethod]
        public void Constructor_1()
        {
            MockAttributeInspector mock = new MockAttributeInspector(new AttributeList());
            Assert.IsNotNull(mock.Attributes);
            Assert.AreEqual(0, mock.Attributes.Count);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Constructor_2()
        {
            MockAttributeInspector mock = new MockAttributeInspector(null);
        }

        [TestMethod]
        public void Constructor_3()
        {
            MockAttributeInspector mock = new MockAttributeInspector(_values);
            Assert.IsNotNull(mock.Attributes);
            Assert.AreEqual(2, mock.Attributes.Count);
        }
    }
}