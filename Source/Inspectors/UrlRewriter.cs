using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace XssShield.Inspectors
{
    /// <summary>
    /// Validates and rewrites the URL for attributes that can only accept a valid URL.
    /// NOTE: This rewriter strips username and passwords from URLs.
    /// </summary>
    public class UrlRewriter : AttributeInspector, iInspector
    {
        /// <summary>
        /// </summary>
        public static readonly
            AttributeList Basic =
                new AttributeList
                {
                    {"a", new List<string> {"href"}},
                    {"img", new List<string> {"src"}},
                    {"q", new List<string> {"cite"}}
                };

        /// <summary>
        /// The base path for rewriting relative URLs.
        /// </summary>
        private readonly string _relative;

        /// <summary>
        /// Reject URLs that use an IP address.
        /// </summary>
        private readonly bool _allowIPAddresses;

        /// <summary>
        /// Constructor
        /// </summary>
        public UrlRewriter(string pRelative, AttributeList pWhiteList, bool pAllowIPAddresses)
            : base(pWhiteList)
        {
            _relative = pRelative;
            if (_relative != null && string.IsNullOrWhiteSpace(_relative))
            {
                throw new ArgumentOutOfRangeException("pRelative","Value can not be empty string.");
            }

            _allowIPAddresses = pAllowIPAddresses;
        }

        /// <summary>
        /// If a node should be reject. Create a BlackListed object and return it.
        /// </summary>
        /// <param name="pNode">The node to inspect.</param>
        /// <returns>A new instance of BlackListed, or Null.</returns>
        public Rejection Inspect(HtmlNode pNode)
        {
            if (pNode == null)
            {
                throw new NullReferenceException("pNode");
            }

            if (!InspectNode(pNode))
            {
                return null;
            }

            string name = pNode.Name.ToLower().Trim();
            foreach (string attrName in Attributes[name])
            {
                string url = CleanURL(_relative, pNode.Attributes[attrName].Value, _allowIPAddresses);
                if (url == null)
                {
                    return new Rejection(true, pNode, new RiskDiscovery(pNode.Line, pNode.LinePosition, "UrlRewriter: Malformed URL"));
                }
                pNode.Attributes[attrName].Value = url;
            }

            return null;
        }

        /// <summary>
        /// Sanitizes and rewrites a URL
        /// </summary>
        /// <param name="pRelative">The relative URL for paths, or Null to reject relative.</param>
        /// <param name="pUrl">The input URL</param>
        /// <param name="pAllowIP">True to allow IP addresses.</param>
        /// <returns>A new URL, or Null if the input URL was invalid</returns>
        public static string CleanURL(string pRelative, string pUrl, bool pAllowIP)
        {
            if (pUrl == null)
            {
                throw new NullReferenceException("pUrl");
            }

            try
            {
                // Assume XSS encoding attack if a "&" character comes before a "?" or "#" character.
                if (pUrl.IndexOf('&') >= 0 &&
                    ((pUrl.IndexOf('?') >= 0 && pUrl.IndexOf('?') > pUrl.IndexOf('&'))
                    || (pUrl.IndexOf('#') >= 0 && pUrl.IndexOf('#') > pUrl.IndexOf('&'))))
                {
                    return null;
                }

                // don't allow "&" by itself
                if (pUrl.IndexOf('&') >= 0 
                    && pUrl.IndexOf('?') == -1
                    && pUrl.IndexOf('#') == -1)
                {
                    return null;
                }

                // remove whitespace from the URL. Some XSS attacks add
                // whitespace to trick filters.
                pUrl = Regex.Replace(pUrl, @"\s", "");

                // decode US-ASCII tags
                pUrl = pUrl.Replace("¼", "<").Replace("¾", ">");

                // see if the decoded version contains any JavaScript
                string str = HttpUtility.UrlDecode(pUrl,Encoding.UTF8);
                if (str != null)
                {
                    str = Regex.Replace(str.ToLower().Trim(), @"\s", "");
                    if (str.IndexOf("javascript:", StringComparison.Ordinal) != -1
                        || str.IndexOf("<script", StringComparison.Ordinal) != -1)
                    {
                        return null;
                    }
                }

                // by rebuilding the URL most XSS attacks
                // will be broken apart and rebuilt causing
                // the attack to fail.
                Uri uri = pRelative == null
                    ? new Uri(pUrl, UriKind.Absolute)
                    : new Uri(new Uri(pRelative), pUrl);

                // only allow HTTP schemes
                if (uri.IsAbsoluteUri
                    && uri.Scheme != Uri.UriSchemeHttp
                    && uri.Scheme != Uri.UriSchemeHttps)
                {
                    return null;
                }

                // only allow DNS and IP hosts
                if (uri.HostNameType == UriHostNameType.Unknown
                    || uri.HostNameType == UriHostNameType.Basic)
                {
                    return null;
                }

                // optionally reject IP addresses
                if (!pAllowIP &&
                    (uri.HostNameType == UriHostNameType.IPv4 || uri.HostNameType == UriHostNameType.IPv6))
                {
                    return null;
                }

                // rebuild the URL by decoding and encoding it's components.
                StringBuilder sb = new StringBuilder();
                sb.Append(uri.Scheme);
                sb.Append("://");

                // TODO: Validate Host name using DNS restrictions
                sb.Append(uri.Host.ToLower());

                if (!uri.IsDefaultPort)
                {
                    sb.Append(":");
                    sb.Append(uri.Port.ToString(CultureInfo.InvariantCulture));
                }

                sb.Append("/");

                if (uri.Segments.Length > 0)
                {
                    string path = string.Join("/",
                        uri.Segments.Select(pSeg => pSeg.Replace("/", "")).Where(pSeg => pSeg != ""));
                    sb.Append(path);
                }

                if (!string.IsNullOrWhiteSpace(uri.Query))
                {
                    sb.Append(uri.Query);
                }

                if (!string.IsNullOrWhiteSpace(uri.Fragment))
                {
                    sb.Append("#");
                    sb.Append(uri.Fragment.Substring(1));
                }

                return sb.ToString();
            }
            catch (UriFormatException)
            {
            }

            return null;
        }

        /// <summary>
        /// Checks if this node should be inspected.
        /// </summary>
        /// <param name="pNode">The node to inspect</param>
        /// <returns>True to continue processing it, or False to skip it.</returns>
        public bool InspectNode(HtmlNode pNode)
        {
            if (pNode == null)
            {
                throw new NullReferenceException("pNode");
            }

            if (pNode.NodeType != HtmlNodeType.Element
                || !pNode.HasAttributes)
            {
                return false;
            }

            return Attributes.ContainsKey(pNode.Name.ToLower());
        }
    }
}