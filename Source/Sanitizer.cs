using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using XssShield.Inspectors;

namespace XssShield
{
    public class Sanitizer
    {
        /// <summary>
        /// The encoding used internally.
        /// </summary>
        private readonly Encoding _encoding;

        /// <summary>
        /// Defines a list of HTML tags that are allowed.
        /// </summary>
        private readonly iInspector _inspector;

        /// <summary>
        /// Sanitizes the HtmlDocument.
        /// </summary>
        /// <param name="pDocument">The document to modify.</param>
        /// <returns>True if any changes were made, or False if none.</returns>
        private Sanitized Clean(string pDocument)
        {
            HtmlWalker walker = new HtmlWalker(pDocument, _encoding);
            Sanitized result = walker.Execute(Process);
            return result;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pInspector">White list provider.</param>
        /// <param name="pEncoding">The encoding used internally.</param>
        public Sanitizer(iInspector pInspector, Encoding pEncoding)
        {
            _inspector = pInspector;
            _encoding = pEncoding;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Sanitizer(iInspector pInspector)
            : this(pInspector, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Performs sanitization of a HTML document.
        /// </summary>
        /// <param name="pInspector">The white list to use.</param>
        /// <param name="pDocument">The document to inspect.</param>
        /// <returns>The results.</returns>
        public static Sanitized Clean(iInspector pInspector, string pDocument)
        {
            Sanitizer sanitizer = new Sanitizer(pInspector);
            return sanitizer.Clean(pDocument);
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
        /// Called for each node in the document.
        /// </summary>
        /// <param name="pResult">Records the result of sanitizing the document.</param>
        /// <param name="pNode">The node to inspect.</param>
        /// <returns>True if modified</returns>
        public void Process(Sanitized pResult, HtmlNode pNode)
        {
            if (pNode.NodeType == HtmlNodeType.Text)
            {
                pResult.Clean.Append(pNode.InnerText);
            }

            if (pNode.NodeType == HtmlNodeType.Element)
            {
                InspectResult negative = _inspector.Inspect(pNode);
                if (negative == null)
                {
                    return;
                }

                if (negative.Reason != null)
                {
                    pResult.Add(negative.Reason);
                }

                if (!negative.RemoveChildren)
                {
                    pNode.ParentNode.AppendChildren(pNode.ChildNodes);
                }
            }

            // by default, any unhandled nodes are removed.
            pNode.ParentNode.RemoveChild(pNode);
        }
    }
}