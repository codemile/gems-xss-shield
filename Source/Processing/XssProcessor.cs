using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using XssShield.Inspectors;

namespace XssShield.Processing
{
    public class XssProcessor
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
        public Sanitized Clean(string pDocument)
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

            // remove extra whitespace
            string clean = result.Clean.ToString();
            result.Clean.Clear();
            result.Clean.Append(CleanWhiteSpace(clean));
            result.Document = CleanWhiteSpace(result.Document);

            return result;
        }

        /// <summary>
        /// Reduces white space in the string.
        /// </summary>
        /// <param name="pStr">The string to clean</param>
        /// <returns>The modified string.</returns>
        private static string CleanWhiteSpace(string pStr)
        {
            pStr = pStr.Replace("\t", " ").Replace("\r", "");
            pStr = Regex.Replace(pStr, @"[ ]+", " ");
            pStr = Regex.Replace(pStr, @"[\n]+", "\n");
            pStr = pStr.Replace(" \n", "");
            pStr = string.Join("\n",pStr.Split('\n').Select(pLine=>pLine.Trim()));
            return pStr;
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
        public XssProcessor(iInspector pInspector, Encoding pEncoding)
        {
            if (pInspector == null)
            {
                throw new NullReferenceException("pInspector");
            }

            _inspector = pInspector;
            _encoding = pEncoding;
            _rejections = new List<Rejection>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public XssProcessor(iInspector pInspector)
            : this(pInspector, Encoding.UTF8)
        {
        }
    }
}