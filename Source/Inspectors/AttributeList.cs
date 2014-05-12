using System;
using System.Collections.Generic;
using System.Linq;

namespace XssShield.Inspectors
{
    /// <summary>
    /// Collection of attributes grouped by element name.
    /// </summary>
    public class AttributeList : Dictionary<string, List<string>>
    {
        /// <summary>
        /// Removes duplicate entries and makes all values lower case.
        /// </summary>
        public static void Clean(AttributeList pList)
        {
            if (pList == null)
            {
                throw new NullReferenceException("pValues");
            }

            List<KeyValuePair<string, List<string>>> copy = pList.ToList();
            pList.Clear();

            (from pair in copy
             let key = pair.Key.ToLower().Trim()
             let values = pair.Value
                 .Select(pValue=>pValue.ToLower().Trim())
                 .Where(pValue=>pValue.Length > 0)
                 .Distinct()
                 .ToList()
             select new {Key = key, Value = values})
                .ToList()
                .ForEach(pItem=>pList.Add(pItem.Key, pItem.Value));
        }

        /// <summary>
        /// Adds an element name and it's allowed attributes.
        /// </summary>
        /// <param name="pTag">The element name</param>
        /// <param name="pAttributes">The list of allowed attributes.</param>
        public new void Add(string pTag, List<string> pAttributes)
        {
            if (pTag == null)
            {
                throw new NullReferenceException("pTag");
            }
            if (pAttributes == null)
            {
                throw new NullReferenceException("pAttributes");
            }

            if (pAttributes.Count == 0)
            {
                return;
            }

            string tag = pTag.ToLower().Trim();
            if (!ContainsKey(tag))
            {
                base.Add(tag, new List<string>());
            }

            this[tag].AddRange(from a in pAttributes select a.ToLower().Trim());
        }

        /// <summary>
        /// Adds a list of allowed elements and their attributes.
        /// </summary>
        public void Add(AttributeList pWhiteList)
        {
            if (pWhiteList == null)
            {
                throw new NullReferenceException("pWhiteList");
            }

            foreach (KeyValuePair<string, List<string>> pair in pWhiteList)
            {
                Add(pair.Key, pair.Value.ToList());
            }

            Clean(this);
        }
    }
}