using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace XssShield.Inspectors
{
    /// <summary>
    /// Filters the attributes of nodes to only allow the ones in the white list. This
    /// inspector never rejects any nodes.
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
            if (InspectNode(pNode))
            {
                FilterAttributes(pNode);
            }
            return null;
        }

        /// <summary>
        /// Filters the attributes of a node.
        /// </summary>
        public HtmlNode FilterAttributes(HtmlNode pNode)
        {
            string name = pNode.Name.ToLower().Trim();

            List<HtmlAttribute> copy = pNode.Attributes.ToList();
            pNode.Attributes.RemoveAll();

            if (!Attributes.ContainsKey(name))
            {
                return pNode;
            }

            List<HtmlAttribute> allowedAttr = copy
                .Where(pAttr=>Attributes[name].Contains(pAttr.Name.ToLower().Trim()))
                .GroupBy(pAttr=>pAttr.Name)
                .Select(pGroup=>pGroup.First())
                .ToList();
            allowedAttr.ForEach(pAttr => pNode.Attributes.Add(pAttr));

            return pNode;
        }

        /// <summary>
        /// Should the node be inspected.
        /// </summary>
        /// <param name="pNode">The node to check</param>
        /// <returns>True to inspect the attributes.</returns>
        public bool InspectNode(HtmlNode pNode)
        {
            if (pNode.NodeType != HtmlNodeType.Element
                || !pNode.HasAttributes)
            {
                return false;
            }

            string name = pNode.Name.ToLower().Trim();
            if (Attributes.ContainsKey(name))
            {
                return true;
            }

            pNode.Attributes.RemoveAll();
            return false;
        }
    }
}