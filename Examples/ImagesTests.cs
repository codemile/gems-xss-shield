using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield;
using XssShield.Inspectors;

namespace Examples
{
    [TestClass]
    public class ImagesTests
    {
        /// <summary>
        /// This example shows how to number all the images in HTML with a unique ID tag.
        /// </summary>
        [TestMethod]
        public void Number_Images()
        {
            string html = Common.getResource("Images.html");

            InspectorCollection inspectors = Sanitizer.ParinoidInspectors("http://www.thinkingmedia.ca");
            inspectors.Add(new AttributeCounter("img-","img","id",1));

            Sanitized sanitized = Sanitizer.Clean(inspectors, html);
            Common.WriteResults(sanitized, html);
        }
    }
}