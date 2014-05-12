using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace XssShield.Inspectors
{
    /// <summary>
    /// A collection of white list elements.
    /// </summary>
    public class InspectorCollection : List<iInspector>, iInspector
    {
        /// <summary>
        /// Checks the list to see if any are rejected.
        /// </summary>
        /// <param name="pNode">The node to check.</param>
        /// <returns>The result</returns>
        public InspectResult Inspect(HtmlNode pNode)
        {
            return this.Select(pWhite=>pWhite.Inspect(pNode))
                .FirstOrDefault(pBlack=>pBlack != null);
        }
    }
}