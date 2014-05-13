using System;
using System.Collections.Generic;
using System.IO;
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
        /// A list of nodes to remove from the DOM.
        /// </summary>
        private readonly List<Rejection> _rejections;

        /// <summary>
        /// Sanitizes the HtmlDocument.
        /// </summary>
        /// <param name="pDocument">The document to modify.</param>
        /// <returns>True if any changes were made, or False if none.</returns>
        private Sanitized Clean(string pDocument)
        {
            _rejections.Clear();

            HtmlWalker walker = new HtmlWalker(pDocument, _encoding);
            Sanitized result = walker.Execute(Process);

            foreach (Rejection rejected in _rejections)
            {
                HtmlNode node = rejected.Node;
                HtmlNode parent = node.ParentNode;

                // can not remove the root node of the document.
                if (parent == null)
                {
                    continue;
                }

                if (!rejected.RemoveChildren)
                {
                    parent.AppendChildren(node.ChildNodes);
                    node.ChildNodes.Clear();
                }
                parent.RemoveChild(node);
            }

            using (StringWriter writer = new StringWriter())
            {
                walker.Document.Save(writer);
                result.Document = writer.ToString();
            }

            return result;
        }

        /// <summary>
        /// Called for each node in the document.
        /// </summary>
        /// <param name="pResult">Records the result of sanitizing the document.</param>
        /// <param name="pNode">The node to inspect.</param>
        /// <returns>True if modified</returns>
        private void Process(Sanitized pResult, HtmlNode pNode)
        {
            switch (pNode.NodeType)
            {
                case HtmlNodeType.Text:
                    pResult.Clean.Append(pNode.InnerText);
                    break;
                case HtmlNodeType.Element:
                    Rejection result = _inspector.Inspect(pNode);
                    if (result == null)
                    {
                        break;
                    }
                    if (result.Reason != null)
                    {
                        pResult.Add(result.Reason);
                    }
                    _rejections.Add(result);
                    break;
                default:
                    _rejections.Add(new Rejection(true, pNode));
                    break;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pInspector">White list provider.</param>
        /// <param name="pEncoding">The encoding used internally.</param>
        private Sanitizer(iInspector pInspector, Encoding pEncoding)
        {
            _inspector = pInspector;
            _encoding = pEncoding;
            _rejections = new List<Rejection>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private Sanitizer(iInspector pInspector)
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
            if (pInspector == null)
            {
                throw new NullReferenceException("pInspector");
            }
            if (pDocument == null)
            {
                throw new NullReferenceException("pDocument");
            }

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
    }
}