using System.Collections.Generic;
using HtmlAgilityPack;

namespace XssShield.Inspectors
{
    public interface iInspector
    {
        List<RiskDiscovery> getRicks(HtmlNode pNode);
    }
}