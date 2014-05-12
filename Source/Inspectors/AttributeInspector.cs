using System.Collections.Generic;
using System.Linq;

namespace XssShield.Inspectors
{
    /// <summary>
    /// Used by inspectors that require a list of attributes grouped by element name.
    /// </summary>
    public abstract class AttributeInspector
    {
        /// <summary>
        /// The list of attributes.
        /// </summary>
        protected Dictionary<string, List<string>> Attributes;

        /// <summary>
        /// Adds an element name and it's allowed attributes.
        /// </summary>
        /// <param name="pTag">The element name</param>
        /// <param name="pAttributes">The list of allowed attributes.</param>
        private void Add(string pTag, IEnumerable<string> pAttributes)
        {
            string tag = pTag.ToLower().Trim();
            if (!Attributes.ContainsKey(tag))
            {
                Attributes.Add(tag, new List<string>());
            }
            Attributes[tag].AddRange(from a in pAttributes select a.ToLower().Trim());
        }

        /// <summary>
        /// Removes duplicate entries and makes all values lower case.
        /// </summary>
        private void Clean()
        {
            Attributes = (from pair in Attributes
                          let key = pair.Key.ToLower().Trim()
                          let values =
                              pair.Value.Select(pValue=>pValue.ToLower().Trim())
                                  .Where(pValue=>pValue.Length > 0).Distinct()
                                  .ToList()
                          select new {Key = key, Value = values})
                .ToDictionary(pKey=>pKey.Key, pValue=>pValue.Value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pWhiteList"></param>
        public AttributeInspector(IEnumerable<KeyValuePair<string, IEnumerable<string>>> pWhiteList)
        {
            Attributes = new Dictionary<string, List<string>>();
            Add(pWhiteList);
        }

        /// <summary>
        /// Adds a list of allowed elements and their attributes.
        /// </summary>
        public void Add(IEnumerable<KeyValuePair<string, IEnumerable<string>>> pWhiteList)
        {
            foreach (KeyValuePair<string, IEnumerable<string>> pair in pWhiteList)
            {
                Add(pair.Key, pair.Value);
            }
            Clean();
        }
    }
}