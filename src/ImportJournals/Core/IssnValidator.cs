using System;
using System.Text.RegularExpressions;

namespace SwissAcademic.Addons.ImportJournalsAddon
{
    internal static class IssnValidator
    {
        public static bool IsValid(string issn)
        {
            //example of a valid ISSN
            //string issn = "0317-8471";

            issn = Regex.Replace(issn, @"[^\dxX]", "");
            if (issn.Length != 8) return false;

            var issnchars = issn.ToCharArray();
            var issnsubstrings = new string[issnchars.Length];
            for (var i = 0; i < issnchars.Length; i++)
            {
                issnsubstrings[i] = issnchars[i].ToString();
            }

            if (issnsubstrings[7].Equals("X", StringComparison.OrdinalIgnoreCase))
            {
                issnsubstrings[7] = "10";
            }

            var sum = 0;
            for (var i = 0; i < issnsubstrings.Length; i++)
            {
                sum += ((8 - i) * Int32.Parse(issnsubstrings[i]));
            };

            return ((sum % 11) == 0);
        }
    }
}
