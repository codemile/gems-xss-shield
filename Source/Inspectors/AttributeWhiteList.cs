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
            AttributeList Minimum =
                new AttributeList
                {
                    {"p", new List<string> {"align"}},
                    {"div", new List<string> {"align"}},
                    {"font", new List<string> {"color", "face"}},
                    {"a", new List<string> {"href", "title"}},
                    {
                        "img", new List<string> {"src", "height", "width", "alt", "title"}
                    },
                    {"q", new List<string> {"cite"}}
                };

        public static readonly
            AttributeList Tables =
                new AttributeList
                {
                    {"th", new List<string> {"colspan", "rowspace", "scope"}},
                    {"td", new List<string> {"colspan", "rowspace"}},
                    {"colgroup", new List<string> {"span"}}
                };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pWhiteList"></param>
        public AttributeWhiteList(AttributeList pWhiteList)
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