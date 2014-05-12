namespace XssShield
{
    /// <summary>
    /// Represents the discovery of something risky in the HTML.
    /// </summary>
    public class RiskDiscovery
    {
        /// <summary>
        /// The description of the risk.
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// The line of the discovery, or -1 if not applicable.
        /// </summary>
        public readonly int Line;

        /// <summary>
        /// The column of the discovery, or -1 if not applicable.
        /// </summary>
        public readonly int Column;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pColumn">The column of the discovery</param>
        /// <param name="pLine">The line of the discovery</param>
        /// <param name="pMessage">The message</param>
        public RiskDiscovery(int pLine, int pColumn, string pMessage)
        {
            Line = pLine;
            Column = pColumn;
            Message = pMessage;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pMessage">The message</param>
        public RiskDiscovery(string pMessage)
            : this(-1, -1, pMessage)
        {
            
        }
    }
}