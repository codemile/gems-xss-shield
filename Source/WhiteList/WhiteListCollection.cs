using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace XssShield.WhiteList
{
    /// <summary>
    /// A collection of white list elements.
    /// </summary>
    public class WhiteListCollection : List<iWhiteList>, iWhiteList
    {
        /// <summary>
        /// Checks the list to see if any are rejected.
        /// </summary>
        /// <param name="pNode">The node to check.</param>
        /// <returns>The result</returns>
        public BlackListed isBlackListed(HtmlNode pNode)
        {
            return this.Select(pWhite=>pWhite.isBlackListed(pNode))
                .FirstOrDefault(pBlack=>pBlack != null);
        }
    }
}