namespace TechFlurry.SparkLedger.Shared.Extentions
{
    public static class StringExtentions
    {
        /// <summary>
        /// Converts a string to first word as uppercase other small
        /// </summary>
        /// <param name="s">string to convert</param>
        /// <returns></returns>
        public static string ToInitialCapital(this string s)
        {
            string firstLetterCapital = s.Substring(0, 1).ToUpper();
            string restWordSmall = s.Substring(1).ToLower();
            return firstLetterCapital + restWordSmall;
        }
    }
}
