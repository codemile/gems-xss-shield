using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace XssShield.Inspectors
{
    /// <summary>
    /// Rejects any HTML elements that are not in the white list.
    /// </summary>
    public class WhiteList : iInspector
    {
        public static readonly string[] Articles =
        {
            "article", "section", "nav", "header", "footer", "aside", "address",
            "main"
        };

        public static readonly string[] BlockStyles = {"pre", "code", "blockquote"};

        /// <summary>
        /// A list of tags whose children can be kept if they are
        /// queued to be removed.
        /// </summary>
        public static readonly string[] ChildFriendlyTags =
        {
            "body", "html", "div"
        };

        public static readonly string[] Figures = {"figure", "figcaption"};
        public static readonly string[] Headings = {"h1", "h2", "h3", "h4", "h5", "h6"};
        public static readonly string[] Links = {"a", "img"};
        public static readonly string[] Lists = {"ul", "li", "ol", "dl", "dt", "dd"};
        public static readonly string[] WebPage = {"html", "head", "title", "body"};
        public static readonly string[] References = {"q", "cite", "abbr", "acronym", "del", "ins"};
        public static readonly string[] Semantics = {"time", "mark"};
        public static readonly string[] Structure = {"p", "div", "span", "br", "hr", "label"};
        public static readonly string[] Styles = {"font", "strong", "b", "em", "i", "u", "strike", "sub", "sup"};

        public static readonly string[] Videos = {"iframe", "object", "embed"};

        public static readonly string[] Tables =
        {
            "table", "thead", "tbody", "tfoot", "th", "tr", "td", "caption", "col",
            "colgroup"
        };

        /// <summary>
        /// A list of elements that are child friendly.
        /// </summary>
        public readonly List<string> ChildFriendly;

        /// <summary>
        /// The list to check
        /// </summary>
        public readonly List<string> List;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pList">A white list of elements.</param>
        /// <param name="pChildFriendly">A white list of elements, when removed their children are kept.</param>
        public WhiteList(IEnumerable<string> pList, IEnumerable<string> pChildFriendly)
        {
            if (pList == null)
            {
                throw new NullReferenceException("pList");
            }

            if (pChildFriendly == null)
            {
                throw new NullReferenceException("pChildFriendly");
            }

            List = CleanList(pList);
            ChildFriendly = CleanList(pChildFriendly);
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

            if (pNode.NodeType != HtmlNodeType.Element)
            {
                return null;
            }

            string name = pNode.Name.ToLower().Trim();
            return List.Contains(name)
                ? null
                : new Rejection(!ChildFriendly.Contains(name),
                    pNode,
                    new RiskDiscovery(pNode.Line, pNode.LinePosition,
                        string.Format("WhiteList: <{0}> is not in white list.", pNode.Name)));
        }

        /// <summary>
        /// Removes empty and duplicated entries in the list.
        /// </summary>
        /// <param name="pList">The list to clean.</param>
        /// <returns>A new list.</returns>
        public static List<string> CleanList(IEnumerable<string> pList)
        {
            if (pList == null)
            {
                throw new NullReferenceException("pList");
            }

            return (from item in pList
                    let str = item.ToLower().Trim()
                    where str.Length > 0
                    select str)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Generates a list of commonly used tags for HTML4.
        /// </summary>
        public static IEnumerable<string> Html4()
        {
            List<string> all = new List<string>();
            all.AddRange(WebPage);
            all.AddRange(Structure);
            all.AddRange(Headings);
            all.AddRange(Styles);
            all.AddRange(Lists);
            all.AddRange(BlockStyles);
            all.AddRange(Links);
            all.AddRange(Tables);
            all.AddRange(References);
            return all;
        }

        /// <summary>
        /// Generates a list of commonly used tags in HTML5.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> Html5()
        {
            List<string> all = Html4().ToList();
            all.AddRange(Articles);
            all.AddRange(Figures);
            all.AddRange(Semantics);
            return all;
        }
    }
}