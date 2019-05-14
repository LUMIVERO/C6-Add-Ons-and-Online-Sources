using SwissAcademic.Addons.NormalizeAllCapitalAuthorNames.Properties;
using SwissAcademic.Citavi.Shell;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.NormalizeAllCapitalAuthorNames
{
    internal static class Macro
    {
        public static void Run(PersonList personList)
        {
            var prefixSuffixFirstCapitalLetter = false;
            var normalizeCapitalLastname = true;
            var counter = 0;
            var project = personList.Project;

            var authors = project.Persons.ToList();
            if (!authors.Any()) return;

            foreach (var author in authors)
            {
                var originalAuthorFullName = author.FullName.ToString();
                var originalAuthorLastName = author.LastName.ToString();

                if ((!string.IsNullOrEmpty(originalAuthorFullName) && originalAuthorFullName.Equals(originalAuthorFullName.ToUpper(), StringComparison.Ordinal)) || (!string.IsNullOrEmpty(originalAuthorLastName) && originalAuthorLastName.Equals(originalAuthorLastName.ToUpper(), StringComparison.Ordinal) && normalizeCapitalLastname))
                {
                    counter++;

                    var authorFirstName = author.FirstName.ToString();
                    var authorMiddleName = author.MiddleName.ToString();
                    var authorLastName = author.LastName.ToString();

                    if (!string.IsNullOrEmpty(authorFirstName)) author.FirstName = authorFirstName.ToLower().ToInitialUpper();
                    if (!string.IsNullOrEmpty(authorMiddleName)) author.MiddleName = authorMiddleName.ToLower().ToInitialUpper();
                    if (!string.IsNullOrEmpty(authorLastName)) author.LastName = authorLastName.ToLower().ToInitialUpper();

                    var authorPrefix = author.Prefix.ToString();
                    var authorSuffix = author.Suffix.ToString();

                    if (prefixSuffixFirstCapitalLetter == true)
                    {
                        if (!string.IsNullOrEmpty(authorPrefix)) author.Prefix = authorPrefix.ToLower().ToInitialUpper();
                        if (!string.IsNullOrEmpty(authorSuffix)) author.Suffix = authorSuffix.ToLower().ToInitialUpper();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(authorPrefix)) author.Prefix = authorPrefix.ToLower();
                        if (!string.IsNullOrEmpty(authorSuffix)) author.Suffix = authorSuffix.ToLower();
                    }

                }
            }
            MessageBox.Show(personList, NormalizeAllCapitalAuthorNamesResources.ResultMessage.FormatString(counter), personList.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}