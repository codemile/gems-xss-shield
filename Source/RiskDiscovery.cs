using System.Diagnostics;
using HtmlAgilityPack;

namespace XssShield
{
    /// <summary>
    /// Represents the discovery of something risky in the HTML.
    /// </summary>
    [DebuggerDisplay("{Line} {Column}:{Message}")]
    public class RiskDiscovery
    {
        /// <summary>
        /// The column of the discovery, or -1 if not applicable.
        /// </summary>
        public readonly int Column;

        /// <summary>
        /// The line of the discovery, or -1 if not applicable.
        /// </summary>
        public readonly int Line;

        /// <summary>
        /// The description of the risk.
        /// </summary>
        public readonly string Message;

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

        /// <summary>
        /// Creates an object based upon a HtmlNode object.
        /// </summary>
        /// <param name="pNode">The node object.</param>
        /// <param name="pMessage">The message.</param>
        /// <returns>The new instance.</returns>
        public static RiskDiscovery Create(HtmlNode pNode, string pMessage)
        {
            return new RiskDiscovery(pNode.Line, pNode.LinePosition, pMessage);
        }
    }
}