using SwissAcademic.Citavi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using ImportJournalsAddon.Properties;

namespace ImportJournalsAddon
{
    static class ImportJournalsByPubMedMacro
    {
        public static void Run(Form form, Project activeProject)
        {
            var journalUrl = @"ftp://ftp.ncbi.nih.gov/pubmed/J_Entrez.txt"; // URL for journal list text file
            var journalCollection = new List<Periodical>();

            string completeList;

            try
            {
                // Get list of journals from website
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
                MessageBox.Show(form, ImportJournalsStrings.PubMedMacroReadErrorMessage.FormatString(journalUrl, e.Message), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                var entrySplitters = new string[] { @"--------------------------------------------------------" };
                var individualJournalEntries = completeList.Split(entrySplitters, StringSplitOptions.RemoveEmptyEntries).ToList();

                var counter = 0;

                var splitEntry = new Regex(@"^(?:JrId: )(?<JournalId>\d+?)(?:\nJournalTitle: )(?<JournalTitle>.*?)(?:\nMedAbbr: )(?<Abbreviation2>.*?)(?:\nISSN \(Print\): )(?<IssnPrint>.*?)(?:\nISSN \(Online\): )(?<IssnOnline>.*?)(?:\nIsoAbbr: )(?<Abbreviation1>.*?)(?:\nNlmId: )(?<NlmId>.*?)$",
                RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.Multiline);

                foreach (string journalEntry in individualJournalEntries)
                {

                    counter++;

                    string journalTitle;
                    string abbreviation1; // this one should have full stops
                    string abbreviation2; // this one shouldn't
                    string abbreviation3; // this one should be any all uppercase acronym after a colon in JournalTitle
                    string issnPrint;
                    string issnOnline;
                    string nlmId;

                    // split into fields
                    Match m = splitEntry.Match(journalEntry);

                    //if (String.IsNullOrEmpty(m.Groups["JournalId"].Value)) continue; // nothing here

                    journalTitle = m.Groups["JournalTitle"].Value;
                    abbreviation1 = m.Groups["Abbreviation1"].Value;
                    abbreviation2 = m.Groups["Abbreviation2"].Value;
                    issnPrint = m.Groups["IssnPrint"].Value; // to be validated
                    issnOnline = m.Groups["IssnOnline"].Value;
                    nlmId = m.Groups["NlmId"].Value;

                    // check format of abbreviation1
                    if (String.IsNullOrEmpty(abbreviation1)) abbreviation1 = abbreviation2;
                    if (!abbreviation1.Contains(".") && !String.IsNullOrEmpty(abbreviation1))
                    {
                        string[] journalTitleWords = journalTitle.ToLowerInvariant().Split(new char[] { ' ', '.', ';', ',', ':', '&', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] abbreviation1Words = abbreviation1.Split(' ');
                        List<string> abbreviation1WithFullStops = new List<string>();


                        foreach (string word in abbreviation1Words)
                        {

                            if (word.StartsWith("(") || word.EndsWith(")"))
                            {
                                abbreviation1WithFullStops.Add(word);
                            }
                            else if (!Array.Exists(journalTitleWords, x => x == word.ToLowerInvariant()))
                            {
                                abbreviation1WithFullStops.Add(word + ".");
                            }
                            else
                            {
                                abbreviation1WithFullStops.Add(word);
                            }
                        }

                        abbreviation1 = String.Join(" ", abbreviation1WithFullStops);
                    }

                    // try to establish Abbreviation3
                    abbreviation3 = Regex.Match(journalTitle, @"(?:: )[A-Z]{2,6}$").ToString();

                    var journal = new Periodical(activeProject, journalTitle);
                    if (!String.IsNullOrEmpty(abbreviation1)) journal.StandardAbbreviation = abbreviation1;
                    if (!String.IsNullOrEmpty(abbreviation2)) journal.UserAbbreviation1 = abbreviation2;
                    if (!String.IsNullOrEmpty(abbreviation3)) journal.UserAbbreviation2 = abbreviation3;

                    if (!string.IsNullOrEmpty(issnPrint) && IssnValidator.IsValid(issnPrint)) journal.Issn = issnPrint;
                    else if (!string.IsNullOrEmpty(issnOnline) && IssnValidator.IsValid(issnOnline)) journal.Issn = issnOnline;

                    if (!string.IsNullOrEmpty(issnPrint) && IssnValidator.IsValid(issnPrint) && !string.IsNullOrEmpty(issnOnline) && IssnValidator.IsValid(issnOnline))
                    {
                        journal.Notes = "ISSN (Online): " + issnOnline;
                    }

                    if (!String.IsNullOrEmpty(nlmId)) journal.Notes = journal.Notes + "\nNlmID: " + nlmId;

                    journalCollection.Add(journal);

                }
                activeProject.Periodicals.AddRange(journalCollection);

            }
            catch (Exception e)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(form, ImportJournalsStrings.MacroImportingErrorMessage.FormatString(e.Message), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            finally
            {
                Cursor.Current = Cursors.Default;

                if (journalCollection != null)
                {
                    MessageBox.Show(form, ImportJournalsStrings.PubMedMacroResultMessage.FormatString(journalCollection.Count.ToString()), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    journalCollection = null;
                }

            }

        }
    }
}
