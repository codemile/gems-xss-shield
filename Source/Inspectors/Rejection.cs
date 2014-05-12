namespace XssShield.Inspectors
{
    /// <summary>
    /// Holds the result of rejecting a node.
    /// </summary>
    public class Rejection
    {
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
        /// <param name="pReason">The reason for the risk.</param>
        public Rejection(bool pRemoveChildren, RiskDiscovery pReason)
        {
            Reason = pReason;
            RemoveChildren = pRemoveChildren;
        }
    }
}