using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield.Inspectors;

namespace XssShieldTests.Inspectors
{
    [TestClass]
    public class AttributeListTests
    {
        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Add_1()
        {
            AttributeList list = new AttributeList();
            list.Add(null);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Add_2()
        {
            AttributeList list = new AttributeList();
            list.Add(null, null);
        }

        [TestMethod]
        public void Add_3()
        {
            AttributeList list = new AttributeList();
            list.Add("p", new List<string>());
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Add_4()
        {
            AttributeList list = new AttributeList();
            list.Add("P", new List<string> {"CLASS", "STYLE"});
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.ContainsKey("p"));
            CollectionAssert.AreEqual(list["p"], new List<string> {"class", "style"});
        }

        [TestMethod]
        public void Add_5()
        {
            AttributeList list1 = new AttributeList
                                  {
                                      {"img", new List<string> {"src", "height"}},
                                      {"p", new List<string> {"align"}}
                                  };

            AttributeList list2 = new AttributeList
                                  {
                                      {"div", new List<string> {"style", "class"}},
                                      {"img", new List<string> {"src", "width"}}
                                  };

            list1.Add(list2);

            Assert.AreEqual(3, list1.Count);
            CollectionAssert.AreEqual(list1["img"], new List<string> {"src", "height", "width"});
            CollectionAssert.AreEqual(list1["p"], new List<string> {"align"});
            CollectionAssert.AreEqual(list1["div"], new List<string> {"style", "class"});
        }

        [TestMethod]
        public void Clean_1()
        {
            AttributeList list = new AttributeList();
            list.Add("p", new List<string> {"style", "style", "style"});
            list.Add("p", new List<string> {"style", "style", "style"});
            list.Add("p", new List<string> {"style", "style", "style"});
            list.Add("p", new List<string> {"class"});
            AttributeList.Clean(list);

            Assert.AreEqual(1, list.Count);
            CollectionAssert.AreEqual(list["p"], new List<string> {"style", "class"});
        }
    }
}