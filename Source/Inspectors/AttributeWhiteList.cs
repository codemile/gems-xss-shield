using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace XssShield.Inspectors
{
    /// <summary>
    /// Filters the attributes of nodes to only allow the ones in the white list.
    /// </summary>
    public class AttributeWhiteList : AttributeInspector, iInspector
    {
        public static readonly
            Dictionary<string, IEnumerable<string>> Minimum =
                new Dictionary<string, IEnumerable<string>>
                {
                    {"p", new[] {"align"}},
                    {"div", new[] {"align"}},
                    {"font", new[] {"color", "face"}},
                    {"a", new[] {"href", "title"}},
                    {
                        "img", new[] {"src", "height", "width", "alt", "title"}
                    },
                    {"q", new[] {"cite"}}
                };

        public static readonly
            Dictionary<string, IEnumerable<string>> Tables =
                new Dictionary<string, IEnumerable<string>>
                {
                    {"th", new[] {"colspan", "rowspace", "scope"}},
                    {"td", new[] {"colspan", "rowspace"}},
                    {"colgroup", new[] {"span"}}
                };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pWhiteList"></param>
        public AttributeWhiteList(IEnumerable<KeyValuePair<string, IEnumerable<string>>> pWhiteList)
            : base(pWhiteList)
        {
        }

        /// <summary>
        /// Removes any attributes for the node that are not on the white list.
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
                pNode.Attributes.RemoveAll();
                return null;
            }

            List<HtmlAttribute> allowedAttr =
                pNode.Attributes
                    .Where(pAttr=>Attributes[name].Contains(pAttr.Name.ToLower()))
                    .GroupBy(pAttr=>pAttr.Name)
                    .Select(pGroup=>pGroup.First())
                    .ToList();

            pNode.Attributes.RemoveAll();
            allowedAttr.ForEach(pAttr=>pNode.Attributes.Add(pAttr));

            return null;
        }

    }
}