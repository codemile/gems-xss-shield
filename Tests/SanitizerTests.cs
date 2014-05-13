using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield;
using XssShieldTests.Mock;

namespace XssShieldTests
{
    [TestClass]
    public class SanitizerTests
    {
        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Clean_1()
        {
            Sanitizer.Clean(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Clean_2()
        {
            MockInspector mock = new MockInspector();
            Sanitized sanitized = Sanitizer.Clean(mock, null);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Clean_3()
        {
            Sanitized sanitized = Sanitizer.Clean(null, "");
        }

        [TestMethod]
        public void Clean_4()
        {
            MockInspector mock = new MockInspector();
            Sanitized sanitized = Sanitizer.Clean(mock, "<p>Hello world.</p>");
            Assert.IsNotNull(sanitized);
            Assert.AreEqual(1,mock.Count);
            Assert.AreEqual("<p>Hello world.</p>",sanitized.Document);
            Assert.AreEqual("Hello world.",sanitized.Clean.ToString());
        }
    }
}