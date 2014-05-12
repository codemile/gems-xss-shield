namespace XssShield.WhiteList
{
    /// <summary>
    /// Holds the result of a white list check.
    /// </summary>
    public class BlackListed
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
        public BlackListed(bool pRemoveChildren, RiskDiscovery pReason)
        {
            Reason = pReason;
            RemoveChildren = pRemoveChildren;
        }

        /// <summary>
        /// Creates a rejection that isn't because of a XSS risk.
        /// </summary>
        /// <param name="pRemoveChildren">True to remove the children</param>
        public BlackListed(bool pRemoveChildren)
            : this(pRemoveChildren, null)
        {
        }
    }
}