using System;
using HtmlAgilityPack;

namespace XssShield.Inspectors
{
    /// <summary>
    /// Holds the result of rejecting a node.
    /// </summary>
    public class Rejection
    {
        /// <summary>
        /// The node that was rejected.
        /// </summary>
        public readonly HtmlNode Node;

        /// <summary>
        /// If the removal is because of a risk, then this
        /// holds the description.
        /// </summary>
        public readonly RiskDiscovery Reason;

        /// <summary>
        /// True to remove this node, and it's children.
        /// </summary>
        public readonly bool RemoveChildren;

        /// <summary>
        /// Creates a rejection that is because of a XSS risk.
        /// </summary>
        /// <param name="pRemoveChildren">Remove the children</param>
        /// <param name="pNode">The node that was rejected.</param>
        /// <param name="pReason">The reason for the risk.</param>
        public Rejection(bool pRemoveChildren, HtmlNode pNode, RiskDiscovery pReason = null)
        {
            if (pNode == null)
            {
                throw new NullReferenceException("pNode");
            }

            RemoveChildren = pRemoveChildren;
            Node = pNode;
            Reason = pReason;
        }
    }
}