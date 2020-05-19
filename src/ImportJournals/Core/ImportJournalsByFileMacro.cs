using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportJournalsAddon
{
    static class ImportJournalsByFileMacro
    {
        public static void Run(PeriodicalList periodicalList)
        {
            var project = periodicalList.Project;
            string fileName = null;

            var journalCollection = new List<Periodical>();

            try
            {
                using (var openFileDialog = new OpenFileDialog
                {
                    Filter = Properties.Resources.FileMacroOpenFileDialogFilters,
                    Title = Properties.Resources.FileMacroOpenFileDialogSubject
                })
                {
                    if (openFileDialog.ShowDialog(periodicalList) != DialogResult.OK) return;

                    fileName = openFileDialog.FileName;
                }

                Cursor.Current = Cursors.WaitCursor;

                string journalList;
                var enc = GetFileEncoding(fileName);
                using (var streamReader = new StreamReader(fileName, enc))
                {
                    journalList = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                var testRegex = new Regex("[\x00-\x1f-[\t\n\r]]", RegexOptions.CultureInvariant | RegexOptions.Compiled);
                if (testRegex.IsMatch(journalList))
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(periodicalList, Properties.Resources.FileMacroNotSupportedCharactersMessage, periodicalList.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                var matchRegex = new Regex(
                    @"^(?<FullName>[^#=;|\t\n]+?)(?: *[#=;|\t] *(?<Abbreviation1>[^#=;|\t\n]*))?(?: *[#=;|\t] *(?<Abbreviation2>[^#=;|\t\n]*))?(?: *[#=;|\t] *(?<Abbreviation3>[^#=;|\t\n]*))?(?: *[#=;|\t] *(?<ISSN>[^#=;|\t\n]*))??$",
                    RegexOptions.CultureInvariant
                    | RegexOptions.Compiled
                    | RegexOptions.Multiline    //IMPORTANT!
                );


                var matchCollection = matchRegex.Matches(journalList);
                string sISSN = string.Empty;

                foreach (Match match in matchCollection)
                {
                    if (string.IsNullOrEmpty(match.Groups["FullName"].Value)) continue;

                    var journal = new Periodical(project, match.Groups["FullName"].Value);
                    if (!string.IsNullOrEmpty(match.Groups["Abbreviation1"].Value)) journal.StandardAbbreviation = match.Groups["Abbreviation1"].Value;
                    if (!string.IsNullOrEmpty(match.Groups["Abbreviation2"].Value)) journal.UserAbbreviation1 = match.Groups["Abbreviation2"].Value;
                    if (!string.IsNullOrEmpty(match.Groups["Abbreviation3"].Value)) journal.UserAbbreviation2 = match.Groups["Abbreviation3"].Value;

                    if (!string.IsNullOrEmpty(match.Groups["ISSN"].Value))
                    {
                        sISSN = match.Groups["ISSN"].Value;
                        if (IssnValidator.IsValid(sISSN)) journal.Issn = sISSN;
                    }

                    journalCollection.Add(journal);
                }

                project.Periodicals.AddRange(journalCollection);
            }

            catch (Exception exception)
            {
                Cursor.Current = Cursors.Default;
                journalCollection = null;
                MessageBox.Show(periodicalList, exception.ToString(), periodicalList.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                Cursor.Current = Cursors.Default;

                if (journalCollection != null)
                {
                    MessageBox.Show(periodicalList, Properties.Resources.FileMacroResultMessage.FormatString(journalCollection.Count), periodicalList.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }

        }

        static Encoding GetFileEncoding(string path)
        {
            //http://www.west-wind.com/Weblog/posts/197245.aspx
            //http://de.wikipedia.org/wiki/Byte_Order_Mark

            // *** Use Default of Encoding.Default (Ansi CodePage)
            var encoding = Encoding.Default;

            // *** Detect byte order mark if any - otherwise assume default
            var buffer = new byte[5];
            using (var file = new FileStream(path, FileMode.Open))
            {
                file.Read(buffer, 0, 5);
                file.Close();
            }

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf) encoding = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff) encoding = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff) encoding = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76) encoding = Encoding.UTF7;

            return encoding;
        }
    }
}
