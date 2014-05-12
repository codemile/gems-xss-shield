using System.Collections.Generic;

namespace gems_html
{
    /// <summary>
    /// Holds the result of sanitizing a HTML document.
    /// </summary>
    public class Sanitized
    {
        /// <summary>
        /// The original HTML.
        /// </summary>
        private readonly string _original;

        /// <summary>
        /// The sanitized HTML.
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// The pure text content without HTML.
        /// </summary>
        public string Clean { get; set; }

        /// <summary>
        /// True if anything was changed to sanitize the output.
        /// </summary>
        public bool Modified { get; set; }

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
        /// Constructor
        /// </summary>
        /// <param name="pOriginal">The original HTML</param>
        public Sanitized(string pOriginal)
        {
            _original = pOriginal;
            Risks = new List<RiskDiscovery>();
            isDangerous = false;
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