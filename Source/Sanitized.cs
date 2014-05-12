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
        /// True if the signature of a XSS attack was found.
        /// </summary>
        public bool isDangerous;

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
            isDangerous = false;
            Clean = new StringBuilder();
        }

        /// <summary>
        /// Records that a risk has been discovered.
        /// </summary>
        /// <param name="pRisk">The risk description.</param>
        public void Add(RiskDiscovery pRisk)
        {
            Risks.Add(pRisk);
            isDangerous = true;
        }
    }
}