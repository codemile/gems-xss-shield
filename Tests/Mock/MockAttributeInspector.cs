using XssShield.Inspectors;

namespace XssShieldTests.Mock
{
    public class MockAttributeInspector : AttributeInspector
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MockAttributeInspector()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MockAttributeInspector(AttributeList pWhiteList)
            : base(pWhiteList)
        {
        }
    }
}