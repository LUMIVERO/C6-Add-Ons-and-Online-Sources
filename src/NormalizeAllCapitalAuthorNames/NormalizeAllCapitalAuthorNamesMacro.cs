using NormalizeAllCapitalAuthorNamesAddon.Properties;
using SwissAcademic.Citavi;
using System;
using System.Linq;
using System.Windows.Forms;

namespace NormalizeAllCapitalAuthorNamesAddon
{
    internal static class NormalizeAllCapitalAuthorNamesMacro
    {
        public static void Run(Form form, Project project)
        {
            //****************************************************************************************************************
            // Normalize Capitalization of All Upper Case Author Names
            // 1.1 -- 2016-03-31
            //
            // This macro checks all author names if they are stored in capital letters (SMITH, JOHN A.) and normalises
            // the name to mixed case (Smith, John A.).
            //
            // v1.1 - names with captial last names only are also normalized if option is set

            // EDIT HERE
            // Variables to be changed by user

            bool prefixSuffixFirstCapitalLetter = false; // capitalize the first letter of name prefixes/suffixes
            bool normalizeCapitalLastname = true; // if true macro will work if only last name is in capital letters, e.g. "HUBER, David"

            // DO NOT EDIT BELOW THIS LINE
            // ****************************************************************************************************************


            int counter = 0;

            if (MessageBox.Show(form, NormalizeAllCapitalAuthorNamesStrings.IsBackupAvailableMessage.Replace("\r\n", System.Environment.NewLine), "Citavi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK) return;

            //get names and exit if none are present
            Person[] authors = project.Persons.ToArray();
            if (!authors.Any()) return;

            // RegEx search and replace in every category name
            foreach (Person author in authors)
            {

                // get full name and check if it is all upper case
                var originalAuthorFullName = author.FullName.ToString();
                var originalAuthorLastName = author.LastName.ToString();

                if (originalAuthorFullName == author.FullName.ToUpper() ||
                    (originalAuthorLastName == author.LastName.ToUpper() && normalizeCapitalLastname))
                {
                    counter++;

                    // get name parts as strings
                    var authorPrefix = author.Prefix.ToString();
                    var authorFirstName = author.FirstName.ToString();
                    var authorMiddleName = author.MiddleName.ToString();
                    var authorLastName = author.LastName.ToString();
                    var authorSuffix = author.Suffix.ToString();

                    // normalise the strings to initial upper case
                    var authorFirstNameNormal = authorFirstName.ToLower().ToInitialUpper();
                    var authorMiddleNameNormal = authorMiddleName.ToLower().ToInitialUpper();
                    var authorLastNameNormal = authorLastName.ToLower().ToInitialUpper();

                    // Prefix/Suffix get initial lower case unless prefixSuffixFirstCapitalLetter is true
                    string authorPrefixNormal;
                    string authorSuffixNormal;

                    if (prefixSuffixFirstCapitalLetter == true)
                    {
                        authorPrefixNormal = authorPrefix.ToLower().ToInitialUpper();
                        authorSuffixNormal = authorSuffix.ToLower().ToInitialUpper();
                    }
                    else
                    {
                        authorPrefixNormal = authorPrefix.ToLower();
                        authorSuffixNormal = authorSuffix.ToLower();
                    }

                    // change author
                    author.Prefix = authorPrefixNormal;
                    author.FirstName = authorFirstNameNormal;
                    author.MiddleName = authorMiddleNameNormal;
                    author.LastName = authorLastNameNormal;
                    author.Suffix = authorSuffixNormal;
                }
            }

            MessageBox.Show(form, NormalizeAllCapitalAuthorNamesStrings.ResultMessage.FormatString(counter), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
