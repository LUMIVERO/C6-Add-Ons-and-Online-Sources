using SwissAcademic.Addons.ImportJournals.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportJournals
{
    static class ImportJournalsByWoodwardMacro
    {
        public static void Main(ProjectShell shell, Project project)
        {
            string journalUrl = @"http://journal-abbreviations.library.ubc.ca/dump.php";

            var journalCollection = new List<Periodical>();
            string completeList;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                using (var webClient = new WebClient())
                {
                    using (var stream = webClient.OpenRead(journalUrl))
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            completeList = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(shell.ActiveForm, ImportJournalsResources.PubMedMacroReadErrorMessage.FormatString(journalUrl, e.Message), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int refCounter = 0;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                completeList = Regex.Match(completeList, @"(<tbody>.*?<\\/tbody)").ToString();
                completeList = Regex.Replace(completeList, @"\\/", @"/");


                var journals = Regex.Matches(completeList, "(?<=<tr>).*?(?=</tr>)");

                foreach (var journalAndAbbrev in journals)
                {
                    var journalTitle = string.Empty;
                    var abbreviation1 = string.Empty; // this one should have full stops
                    var abbreviation2 = string.Empty; // this one shouldn't
                    var abbreviation3 = string.Empty; // this one should be any all uppercase acronym after a colon in JournalTitl

                    MatchCollection journalData = Regex.Matches(journalAndAbbrev.ToString(), "(?<=<td>).*?(?=</td>)");
                    if (journalData.Count < 2) continue;

                    abbreviation1 = journalData[0].Value;
                    journalTitle = Regex.Replace(journalData[1].Value, @"\bfur\b", "für");

                    // generate abbreviation2 by removing full stops from abbreviation1
                    var abbreviation1Words = abbreviation1.Split(' ');
                    for (int i = 0; i < abbreviation1Words.Length; i++)
                    {
                        abbreviation1Words[i] = abbreviation1Words[i].TrimEnd('.');
                    }
                    abbreviation2 = String.Join(" ", abbreviation1Words);


                    // try to establish Abbreviation3
                    abbreviation3 = Regex.Match(journalTitle, @"(?:: )[A-Z]{2,6}$").ToString();



                    Periodical journal = new Periodical(project, journalTitle);
                    if (!String.IsNullOrEmpty(abbreviation1)) journal.StandardAbbreviation = abbreviation1;
                    if (!String.IsNullOrEmpty(abbreviation2)) journal.UserAbbreviation1 = abbreviation2;
                    if (!String.IsNullOrEmpty(abbreviation3)) journal.UserAbbreviation2 = abbreviation3;

                    journalCollection.Add(journal);

                }

                DialogResult updateReferences = MessageBox.Show(shell.ActiveForm, ImportJournalsResources.WoodwardMacroUpdateMessage, "Citavi", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                 MessageBoxDefaultButton.Button2);

                if (updateReferences == DialogResult.Yes)
                {

                    var references = Program.ActiveProjectShell.PrimaryMainForm.GetFilteredReferences();
                    foreach (var reference in references)
                    {
                        if (reference.Periodical == null) continue;
                        if (journalCollection.Any(item => item.Name == reference.Periodical.Name) && !String.IsNullOrEmpty(reference.Periodical.Name))
                        {
                            reference.Periodical = journalCollection.Where(item => item.Name == reference.Periodical.Name).FirstOrDefault();
                            refCounter++;
                        }
                    }
                    project.Periodicals.AddRange(journalCollection);
                }
                else
                {
                    project.Periodicals.AddRange(journalCollection);
                }



            }
            catch (Exception e)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(shell.ActiveForm, ImportJournalsResources.MacroImportingErrorMessage.FormatString(e.Message), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            finally
            {
                Cursor.Current = Cursors.Default;

                if (journalCollection != null)
                {
                    MessageBox.Show(shell.ActiveForm, ImportJournalsResources.WoodwardMacroResultMessage.FormatString(journalCollection.Count, refCounter), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    journalCollection = null;
                }


            }



        }
    }
}
