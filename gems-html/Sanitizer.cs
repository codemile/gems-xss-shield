using System.Text;
using HtmlAgilityPack;

namespace gems_html
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
        private readonly iWhiteList _whiteList;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pWhiteList">White list provider.</param>
        /// <param name="pNodeInspector">Handles inspecting a node for risk signatures.</param>
        /// <param name="pEncoding">The encoding used internally.</param>
        public Sanitizer(iWhiteList pWhiteList, iNodeInspector pNodeInspector, Encoding pEncoding)
        {
            _whiteList = pWhiteList;
            _encoding = pEncoding;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Sanitizer(iWhiteList pWhiteList, iNodeInspector pNodeInspector)
            : this(pWhiteList, pNodeInspector, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Sanitizes the HtmlDocument.
        /// </summary>
        /// <param name="pDocument">The document to modify.</param>
        /// <returns>True if any changes were made, or False if none.</returns>
        public Sanitized Clean(string pDocument)
        {
            Sanitized result = new Sanitized(pDocument);

            // first pass
            result.Document = Sanatize(pDocument);
            // second pass
            string pass2 = Sanatize(result.Document);

            if (result.Document.Equals(pass2) == false)
            {
                result.Add(new RiskDiscovery("Second pass failure. HTML should not change after sanitizing twice."));
            }

            return result;
        }

        public string Sanatize(string pDocument)
        {
            // NOTE: XSS attacks often rely upon readers that perform automatic closing of tags. If
            // you change any of these HtmlDocument settings. All unit tests must be verified again.
            HtmlDocument html = new HtmlDocument
            {
                OptionFixNestedTags = true,
                OptionAutoCloseOnEnd = true,
                OptionDefaultStreamEncoding = _encoding,
                OptionUseIdAttribute = true,
                OptionWriteEmptyNodes = true
            };

            Process(html.DocumentNode);
        }

        public bool Process(HtmlNode pNode)
        {
            if (pNode.NodeType == HtmlNodeType.Text)
            {
            }

            if (pNode.NodeType == HtmlNodeType.Element)
            {
            }

            // by default, any unhandled nodes are removed.
            pNode.Remove();
            return true;
        }
    }
}