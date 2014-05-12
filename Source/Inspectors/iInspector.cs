using HtmlAgilityPack;

namespace XssShield.Inspectors
{
    /// <summary>
    /// Objects that test for rejection should implement this interface.
    /// </summary>
    public interface iInspector
    {
        /// <summary>
        /// If a node should be reject. Create a BlackListed object and return it.
        /// </summary>
        /// <param name="pNode">The node to inspect.</param>
        /// <returns>A new instance of BlackListed, or Null.</returns>
        Rejection Inspect(HtmlNode pNode);
    }
}