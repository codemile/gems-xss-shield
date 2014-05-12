namespace XssShield.Inspectors
{
    /// <summary>
    /// Inspector classes can use this base class when they need to work on attributes. This class
    /// will maintain the list of element-to-attribute list.
    /// </summary>
    public abstract class AttributeInspector
    {
        /// <summary>
        /// List of attributes the inspector can operate on.
        /// </summary>
        public readonly AttributeList Attributes;

        /// <summary>
        /// Constructor
        /// </summary>
        protected AttributeInspector()
        {
            Attributes = new AttributeList();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected AttributeInspector(AttributeList pWhiteList)
            : this()
        {
            Attributes.Add(pWhiteList);
        }
    }
}