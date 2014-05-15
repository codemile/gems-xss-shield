using System;
using HtmlAgilityPack;

namespace XssShield.Inspectors
{
    /// <summary>
    /// Creates an attribute on matching elements that increments.
    /// </summary>
    public class AttributeCounter : iInspector
    {
        /// <summary>
        /// The DOM element to count.
        /// </summary>
        private readonly string _attributeName;

        /// <summary>
        /// The DOM element to count.
        /// </summary>
        private readonly string _elementName;

        /// <summary>
        /// The prefix for the attribute value.
        /// </summary>
        private readonly string _prefix;

        /// <summary>
        /// The value to start counting from.
        /// </summary>
        private readonly int _startValue;

        /// <summary>
        /// The counter value
        /// </summary>
        private int _counter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pPrefix">The prefix for the attribute value</param>
        /// <param name="pElementName">The DOM element to count</param>
        /// <param name="pAttributeName">The attribute to store the value</param>
        /// <param name="pStartValue">The value to start counting from.</param>
        public AttributeCounter(string pPrefix, string pElementName = "img", string pAttributeName = "id",
                                int pStartValue = 0)
        {
            if (pPrefix == null)
            {
                throw new NullReferenceException("pPrefix");
            }
            if (pElementName == null)
            {
                throw new NullReferenceException("pElementName");
            }
            if (pAttributeName == null)
            {
                throw new NullReferenceException("pAttributeName");
            }

            _elementName = pElementName;
            _attributeName = pAttributeName;
            _prefix = pPrefix;
            _startValue = pStartValue;
            _counter = 0;
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

            if (pNode.NodeType == HtmlNodeType.Element
                && pNode.Name == _elementName)
            {
                pNode.SetAttributeValue(_attributeName, _prefix + (_startValue + _counter++));
            }

            return null;
        }
    }
}