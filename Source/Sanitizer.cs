using System.Text;
using HtmlAgilityPack;
using XssShield.Inspectors;
using XssShield.WhiteList;

namespace XssShield
{
    public class Sanitizer
    {
        /// <summary>
        /// The encoding used internally.
        /// </summary>
        private readonly Encoding _encoding;

        /// <summary>
        /// Handles node inspections.
        /// </summary>
        private readonly iInspector _inspector;

        /// <summary>
        /// Defines a list of HTML tags that are allowed.
        /// </summary>
        private readonly iWhiteList _whiteList;

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
        /// <param name="pWhiteList">White list provider.</param>
        /// <param name="pInspector">Handles inspecting a node for risk signatures.</param>
        /// <param name="pEncoding">The encoding used internally.</param>
        public Sanitizer(iWhiteList pWhiteList, iInspector pInspector, Encoding pEncoding)
        {
            _whiteList = pWhiteList;
            _inspector = pInspector;
            _encoding = pEncoding;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Sanitizer(iWhiteList pWhiteList, iInspector pInspector)
            : this(pWhiteList, pInspector, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Performs sanitization of a HTML document.
        /// </summary>
        /// <param name="pWhiteList">The white list to use.</param>
        /// <param name="pInspector">The node inspector to use.</param>
        /// <param name="pDocument">The document to inspect.</param>
        /// <returns>The results.</returns>
        public static Sanitized Clean(iWhiteList pWhiteList, iInspector pInspector, string pDocument)
        {
            Sanitizer sanitizer = new Sanitizer(pWhiteList, pInspector);
            return sanitizer.Clean(pDocument);
        }

        /// <summary>
        /// Performs sanitization of an HTML document twice. Assumes that any differences between the
        /// first pass and second pass are indicators of filter evasion.
        /// </summary>
        /// <param name="pWhiteList">The white list to use.</param>
        /// <param name="pInspector">The node inspector to use.</param>
        /// <param name="pDocument">The document to inspect.</param>
        /// <returns>The results.</returns>
        public static Sanitized DoublePass(iWhiteList pWhiteList, iInspector pInspector, string pDocument)
        {
            // perform a two pass sanitization test
            Sanitized pass1 = Clean(pWhiteList, pInspector, pDocument);
            Sanitized pass2 = Clean(pWhiteList, pInspector, pass1.Document);

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
            _inspector.getRicks(pNode).ForEach(pResult.Add);

            if (pNode.NodeType == HtmlNodeType.Text)
            {
            }

            if (pNode.NodeType == HtmlNodeType.Element)
            {
                BlackListed black = _whiteList.isBlackListed(pNode);
                if (black == null)
                {
                    return;
                }

                if (black.Reason != null)
                {
                    pResult.Add(black.Reason);
                }

                if (!black.RemoveChildren)
                {
                    pNode.ParentNode.AppendChildren(pNode.ChildNodes);
                }
            }

            // by default, any unhandled nodes are removed.
            pNode.ParentNode.RemoveChild(pNode);
        }
    }
}