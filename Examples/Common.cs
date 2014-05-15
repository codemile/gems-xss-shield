using Common.Utils;

namespace Examples
{
    /// <summary>
    /// A utility class for examples.
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Reads an assembly resource as a string.
        /// </summary>
        /// <param name="pFileName">The name of the file to read.</param>
        /// <returns>The contents as a string.</returns>
        public static string getResource(string pFileName)
        {
            Resources reader = new Resources(typeof (Common), "Files");
            return reader.ReadAsString(pFileName, true, true);
        }
    }
}