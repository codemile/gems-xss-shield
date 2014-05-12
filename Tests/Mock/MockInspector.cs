using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XssShield.Inspectors;

namespace XssShieldTests.Mock
{
    public class MockInspector : iInspector
    {
        private readonly Rejection _reject;

        public int Count;

        public MockInspector()
        {
            Count = 0;
            _reject = null;
        }

        public MockInspector(Rejection pReject)
            : this()
        {
            _reject = pReject;
        }

        public Rejection Inspect(HtmlNode pNode)
        {
            Assert.IsNotNull(pNode);
            Count++;
            return _reject;
        }
    }
}