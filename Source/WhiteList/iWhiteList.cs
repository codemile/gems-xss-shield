using HtmlAgilityPack;

namespace XssShield.WhiteList
{
    /// <summary>
    /// Objects that test for rejection should implement this interface.
    /// </summary>
    public interface iWhiteList
    {
        /// <summary>
        /// If a node should be reject. Create a BlackListed object and return it.
        /// </summary>
        /// <param name="pNode">The node to inspect.</param>
        /// <returns>A new instance of BlackListed, or Null.</returns>
        BlackListed isBlackListed(HtmlNode pNode);
    }
}