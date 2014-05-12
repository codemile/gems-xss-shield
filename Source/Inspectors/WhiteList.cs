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
        public static readonly string[] ChildFriendly =
        {
            "body", "html", "div"
        };

        public static readonly string[] Figures = {"figure", "figcaption"};
        public static readonly string[] Headings = {"h1", "h2", "h3", "h4", "h5", "h6"};
        public static readonly string[] Links = {"a", "img"};
        public static readonly string[] Lists = {"ul", "li", "ol", "dl", "dt", "dd"};
        public static readonly string[] References = {"q", "cite", "abbr", "acronym", "del", "ins"};
        public static readonly string[] Semantics = {"time", "mark"};
        public static readonly string[] Structure = {"p", "div", "span", "br", "hr", "label"};
        public static readonly string[] Styles = {"font", "strong", "b", "em", "i", "u", "strike", "sub", "sup"};

        public static readonly string[] Tables =
        {
            "table", "thead", "tbody", "tfoot", "th", "tr", "td", "caption", "col",
            "colgroup"
        };

        /// <summary>
        /// A list of elements that are child friendly.
        /// </summary>
        private readonly IEnumerable<string> _childFriendly;

        /// <summary>
        /// The list to check
        /// </summary>
        private readonly List<string> _list;

        /// <summary>
        /// Removes empty and duplicated entries in the list.
        /// </summary>
        /// <param name="pList">The list to clean.</param>
        /// <returns>A new list.</returns>
        private static List<string> CleanList(IEnumerable<string> pList)
        {
            return (from item in pList
                    let str = item.ToLower().Trim()
                    where str.Length > 0
                    select str)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pList">A white list of elements.</param>
        /// <param name="pChildFriendly">A white list of elements, when removed their children are kept.</param>
        public WhiteList(IEnumerable<string> pList, IEnumerable<string> pChildFriendly)
        {
            _list = CleanList(pList);
            _childFriendly = CleanList(pChildFriendly);
        }

        /// <summary>
        /// If a node should be reject. Create a BlackListed object and return it.
        /// </summary>
        /// <param name="pNode">The node to inspect.</param>
        /// <returns>A new instance of BlackListed, or Null.</returns>
        public Rejection Inspect(HtmlNode pNode)
        {
            string name = pNode.Name.ToLower().Trim();
            return _list.Contains(name)
                ? null
                : new Rejection(_childFriendly.Contains(name),
                    new RiskDiscovery(pNode.Line, pNode.LinePosition,
                        string.Format("<{0}> is not in white list.", pNode.Name)));
        }

        /// <summary>
        /// Generates a list of commonly used tags for HTML4.
        /// </summary>
        public static IEnumerable<string> Html4()
        {
            List<string> all = new List<string>();
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