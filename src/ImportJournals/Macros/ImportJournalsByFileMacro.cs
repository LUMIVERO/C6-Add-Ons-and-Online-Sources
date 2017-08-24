using ImportJournalsAddon.Properties;
using SwissAcademic.Citavi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ImportJournalsAddon
{
    static class ImportJournalsByFileMacro
    {
        public static void Run(Form form, Project project)
        {
            string fileName = null;

            var journalCollection = new List<Periodical>();

            try
            {
                // Dialog zum Auswählen des gewünschten Ordners einblenden
                using (var openFileDialog = new OpenFileDialog
                {
                    Filter = ImportJournalsStrings.FileMacroOpenFileDialogFilters,
                    Title = ImportJournalsStrings.FileMacroOpenFileDialogSubject
                })
                {
                    if (openFileDialog.ShowDialog(form) != DialogResult.OK) return;

                    fileName = openFileDialog.FileName;
                }



                //Hourglass or other wait cursor
                Cursor.Current = Cursors.WaitCursor;

                string journalList;
                var enc = GetFileEncoding(fileName);
                using (var streamReader = new StreamReader(fileName, enc))
                {
                    journalList = streamReader.ReadToEnd();
                    streamReader.Close();
                }



                //We check for certain non-printable chars to ensure we are dealing with a "text file"
                var testRegex = new Regex("[\x00-\x1f-[\t\n\r]]", RegexOptions.CultureInvariant | RegexOptions.Compiled);
                if (testRegex.IsMatch(journalList))
                {   //this is most likely not a textfile
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(form, ImportJournalsStrings.FileMacroNotSupportedCharactersMessage, "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                var matchRegex = new Regex(
                    @"^(?<FullName>[^#=;|\t\n]+?)(?: *[#=;|\t] *(?<Abbreviation1>[^#=;|\t\n]*))?(?: *[#=;|\t] *(?<Abbreviation2>[^#=;|\t\n]*))?(?: *[#=;|\t] *(?<Abbreviation3>[^#=;|\t\n]*))?(?: *[#=;|\t] *(?<ISSN>[^#=;|\t\n]*))??$",
                    RegexOptions.CultureInvariant
                    | RegexOptions.Compiled
                    | RegexOptions.Multiline    //IMPORTANT!
                );




                //// Capture all matches in the journalList
                MatchCollection ms = matchRegex.Matches(journalList);
                //MessageBox.Show("Insgesamt sind " + ms.Count.ToString() + " Treffer gefunden worden.");

                foreach (Match m in ms)
                {

                    if (string.IsNullOrEmpty(m.Groups["FullName"].Value)) continue;

                    Periodical journal = new Periodical(project, m.Groups["FullName"].Value);
                    if (!string.IsNullOrEmpty(m.Groups["Abbreviation1"].Value)) journal.StandardAbbreviation = m.Groups["Abbreviation1"].Value;
                    if (!string.IsNullOrEmpty(m.Groups["Abbreviation2"].Value)) journal.UserAbbreviation1 = m.Groups["Abbreviation2"].Value;
                    if (!string.IsNullOrEmpty(m.Groups["Abbreviation3"].Value)) journal.UserAbbreviation2 = m.Groups["Abbreviation3"].Value;

                    string sISSN = "";
                    if (!string.IsNullOrEmpty(m.Groups["ISSN"].Value))
                    {
                        sISSN = m.Groups["ISSN"].Value;
                        if (IssnValidator.IsValid(sISSN)) journal.Issn = sISSN;
                    }



                    journalCollection.Add(journal);
                }

                project.Periodicals.AddRange(journalCollection);
            }

            catch (Exception exception)
            {
                Cursor.Current = Cursors.Default;

                MessageBox.Show(form, exception.ToString(), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                Cursor.Current = Cursors.Default;

                if (journalCollection != null)
                {
                    MessageBox.Show(form, ImportJournalsStrings.FileMacroResultMessage.FormatString(journalCollection.Count), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    journalCollection = null;
                }


            }

        }


        static Encoding GetFileEncoding(string srcFile)
        {
            //http://www.west-wind.com/Weblog/posts/197245.aspx
            //http://de.wikipedia.org/wiki/Byte_Order_Mark

            // *** Use Default of Encoding.Default (Ansi CodePage)
            var encoding = Encoding.Default;

            // *** Detect byte order mark if any - otherwise assume default
            var buffer = new byte[5];
            using (var file = new FileStream(srcFile, FileMode.Open))
            {
                file.Read(buffer, 0, 5);
                file.Close();
            }

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                encoding = Encoding.UTF8;

            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                encoding = Encoding.Unicode;

            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                encoding = Encoding.UTF32;

            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                encoding = Encoding.UTF7;

            return encoding;
        }
    }
}
