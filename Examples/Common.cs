using System;
using Common.Utils;
using XssShield;

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

        /// <summary>
        /// Writes the results to the console.
        /// </summary>
        public static void WriteResults(Sanitized pSanitized, string pOriginal)
        {
            Console.WriteLine(@"*******************************************");
            Console.WriteLine(@"Clean");
            Console.WriteLine(@"*******************************************");
            Console.WriteLine(pSanitized.Clean);
            Console.WriteLine();

            Console.WriteLine(@"*******************************************");
            Console.WriteLine(@"HTML");
            Console.WriteLine(@"*******************************************");
            Console.WriteLine(pSanitized.Document);
            Console.WriteLine();

            Console.WriteLine(@"*******************************************");
            Console.WriteLine(@"Original");
            Console.WriteLine(@"*******************************************");
            Console.WriteLine(pOriginal);
            Console.WriteLine();
        }
    }
}