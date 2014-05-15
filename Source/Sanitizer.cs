using System;
using XssShield.Inspectors;
using XssShield.Processing;

namespace XssShield
{
    /// <summary>
    /// The main factory class for sanitizing HTML.
    /// </summary>
    public static class Sanitizer
    {
        /// <summary>
        /// Performs sanitization of a HTML document.
        /// </summary>
        /// <param name="pInspector">The white list to use.</param>
        /// <param name="pDocument">The document to inspect.</param>
        /// <returns>The results.</returns>
        public static Sanitized Clean(iInspector pInspector, string pDocument)
        {
            if (pInspector == null)
            {
                throw new NullReferenceException("pInspector");
            }
            if (pDocument == null)
            {
                throw new NullReferenceException("pDocument");
            }

            XssProcessor xssProcessor = new XssProcessor(pInspector);
            return xssProcessor.Clean(pDocument);
        }

        /// <summary>
        /// Performs sanitization of an HTML document twice. Assumes that any differences between the
        /// first pass and second pass are indicators of filter evasion.
        /// </summary>
        /// <param name="pInspector">The white list to use.</param>
        /// <param name="pDocument">The document to inspect.</param>
        /// <returns>The results.</returns>
        public static Sanitized DoublePass(iInspector pInspector, string pDocument)
        {
            if (pInspector == null)
            {
                throw new NullReferenceException("pInspector");
            }
            if (pDocument == null)
            {
                throw new NullReferenceException("pDocument");
            }

            // perform a two pass sanitization test
            Sanitized pass1 = Clean(pInspector, pDocument);
            Sanitized pass2 = Clean(pInspector, pass1.Document);

            if (pass1.Document.Equals(pass2.Document) == false)
            {
                pass1.Add(new RiskDiscovery("Second pass failure. HTML should not change when sanitizing a second time."));
            }

            return pass1;
        }

        /// <summary>
        /// Performs sanitization of a HTML document using a default
        /// set of inspectors where strict XSS safety is required.
        /// </summary>
        /// <param name="pRelative">The base path for URLs</param>
        /// <param name="pDocument">A string containing the HTML.</param>
        /// <returns>The results.</returns>
        public static Sanitized Parinoid(string pRelative, string pDocument)
        {
            return Clean(ParinoidInspectors(pRelative), pDocument);
        }

        /// <summary>
        /// Creates a list of inspectors for strict XSS safety.
        /// </summary>
        public static InspectorCollection ParinoidInspectors(string pRelative)
        {
            return new InspectorCollection
                   {
                       new WhiteList(WhiteList.Html5(), WhiteList.ChildFriendlyTags),
                       new AttributeWhiteList(AttributeWhiteList.Minimum),
                       new UrlRewriter(pRelative, UrlRewriter.Basic, false)
                   };
        }
    }
}