using System;

namespace ImportJournalsAddon
{
    internal static class IssnValidator
    {
        internal static bool IsValid(string issn)
        {
            //example of a valid ISSN
            //string issn = "0317-8471";

            issn = System.Text.RegularExpressions.Regex.Replace(issn, @"[^\dxX]", "");
            if (issn.Length != 8) return false;

            char[] issnchars = issn.ToCharArray();
            string[] issnsubstrings = new string[issnchars.Length];
            for (int i = 0; i < issnchars.Length; i++)
            {
                issnsubstrings[i] = issnchars[i].ToString();
            }

            if (issnsubstrings[7].ToUpper() == "X")
            {
                issnsubstrings[7] = "10";
            }
            int sum = 0;
            for (int i = 0; i < issnsubstrings.Length; i++)
            {
                sum += ((8 - i) * Int32.Parse(issnsubstrings[i]));
            };
            return ((sum % 11) == 0);


        }
    }
}
