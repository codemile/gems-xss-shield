using System.Collections.Generic;
using System.Text;

namespace XssShield
{
    /// <summary>
    /// Holds the result of sanitizing a HTML document.
    /// </summary>
    public class Sanitized
    {
        /// <summary>
        /// The pure text content without HTML.
        /// </summary>
        public readonly StringBuilder Clean;

        /// <summary>
        /// A list of XSS risks discovered in the original document. If
        /// this contains any items the document should not be trusted.
        /// </summary>
        public readonly List<RiskDiscovery> Risks;

        /// <summary>
        /// The sanitized HTML.
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Sanitized()
        {
            Risks = new List<RiskDiscovery>();
            Clean = new StringBuilder();
        }

        /// <summary>
        /// Records that a risk has been discovered.
        /// </summary>
        /// <param name="pRisk">The risk description.</param>
        public void Add(RiskDiscovery pRisk)
        {
            Risks.Add(pRisk);
        }
    }
}