using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield;

namespace Examples
{
    [TestClass]
    public class BasicUsageTests
    {
        /// <summary>
        /// This example shows how to clean the HTML
        /// in a string using the Paranoid default
        /// settings.
        /// </summary>
        [TestMethod]
        public void Clean_Html_String()
        {
            string html = Common.getResource("Sample.html");
            Sanitized sanitized = Sanitizer.Parinoid("http://www.thinkingmedia.ca", html);
            Common.WriteResults(sanitized,html);
        }
    }
}
