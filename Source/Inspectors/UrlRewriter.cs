using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace XssShield.Inspectors
{
    /// <summary>
    /// Validates and rewrites the URL for attributes that can only accept a valid URL.
    /// </summary>
    public class UrlRewriter : AttributeInspector, iInspector
    {
        public static readonly
            Dictionary<string, IEnumerable<string>> Basic =
                new Dictionary<string, IEnumerable<string>>
                {
                    {"a", new[] {"href"}},
                    {"img", new[] {"src"}},
                    {"q", new[] {"cite"}}
                };

        /// <summary>
        /// Constructor
        /// </summary>
        public UrlRewriter(AttributeList pWhiteList)
            : base(pWhiteList)
        {
        }

        /// <summary>
        /// If a node should be reject. Create a BlackListed object and return it.
        /// </summary>
        /// <param name="pNode">The node to inspect.</param>
        /// <returns>A new instance of BlackListed, or Null.</returns>
        public Rejection Inspect(HtmlNode pNode)
        {
            string name = pNode.Name.ToLower().Trim();

            if (pNode.NodeType != HtmlNodeType.Element
                || !pNode.HasAttributes)
            {
                return null;
            }

            if (!Attributes.ContainsKey(name))
            {
                return null;
            }

            foreach (string attrName in Attributes[name])
            {
                string url = CleanURL(pNode.Attributes[attrName].Value);
                if (url == null)
                {
                    return new Rejection(true, new RiskDiscovery(pNode.Line, pNode.LinePosition, "Malformed URL"));
                }
                pNode.Attributes[attrName].Value = url;
            }

            return null;
        }

        /// <summary>
        /// Sanitizes and rewrites a URL
        /// </summary>
        /// <param name="pUrl">The input URL</param>
        /// <returns>A new URL output</returns>
        public static string CleanURL(string pUrl)
        {
            Uri uri = new Uri(pUrl);
            return uri.ToString();
        }
    }
}