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
    static class ImportJournalsByPubMedMacro
    {
        public static void Run(PeriodicalList periodicalList)
        {
            var project = periodicalList.Project;
            var journalUrl = @"http://ftp.ncbi.nih.gov/pubmed/J_Entrez.txt"; // URL for journal list text file
            var journalCollection = new List<Periodical>();

            string completeList;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                using (var webClient = new WebClient2() { Timeout = 60000 })
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
                MessageBox.Show(periodicalList, Properties.Resources.PubMedMacroReadErrorMessage.FormatString(journalUrl, e.Message), periodicalList.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string abbreviation1;
                    string abbreviation2;
                    string abbreviation3;
                    string issnPrint;
                    string issnOnline;
                    string nlmId;


                    var match = splitEntry.Match(journalEntry);
                    journalTitle = match.Groups["JournalTitle"].Value;
                    abbreviation1 = match.Groups["Abbreviation1"].Value;
                    abbreviation2 = match.Groups["Abbreviation2"].Value;
                    issnPrint = match.Groups["IssnPrint"].Value;
                    issnOnline = match.Groups["IssnOnline"].Value;
                    nlmId = match.Groups["NlmId"].Value;

                    if (string.IsNullOrEmpty(abbreviation1)) abbreviation1 = abbreviation2;
                    if (!abbreviation1.Contains(".") && !String.IsNullOrEmpty(abbreviation1))
                    {
                        var journalTitleWords = journalTitle.ToLowerInvariant().Split(new char[] { ' ', '.', ';', ',', ':', '&', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        var abbreviation1Words = abbreviation1.Split(' ');
                        var abbreviation1WithFullStops = new List<string>();


                        foreach (var word in abbreviation1Words)
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

                        abbreviation1 = string.Join(" ", abbreviation1WithFullStops);
                    }

                    abbreviation3 = Regex.Match(journalTitle, @"(?:: )[A-Z]{2,6}$").ToString();

                    var journal = new Periodical(project, journalTitle);
                    if (!string.IsNullOrEmpty(abbreviation1)) journal.StandardAbbreviation = abbreviation1;
                    if (!string.IsNullOrEmpty(abbreviation2)) journal.UserAbbreviation1 = abbreviation2;
                    if (!string.IsNullOrEmpty(abbreviation3)) journal.UserAbbreviation2 = abbreviation3;

                    if (!string.IsNullOrEmpty(issnPrint) && IssnValidator.IsValid(issnPrint)) journal.Issn = issnPrint;
                    else if (!string.IsNullOrEmpty(issnOnline) && IssnValidator.IsValid(issnOnline)) journal.Issn = issnOnline;

                    if (!string.IsNullOrEmpty(issnPrint) && IssnValidator.IsValid(issnPrint) && !string.IsNullOrEmpty(issnOnline) && IssnValidator.IsValid(issnOnline))
                    {
                        journal.Notes = "ISSN (Online): " + issnOnline;
                    }

                    if (!string.IsNullOrEmpty(nlmId)) journal.Notes = journal.Notes + "\nNlmID: " + nlmId;

                    journalCollection.Add(journal);

                }
                project.Periodicals.AddRange(journalCollection);

            }
            catch (Exception exception)
            {
                Cursor.Current = Cursors.Default;
                journalCollection = null;
                MessageBox.Show(periodicalList, Properties.Resources.MacroImportingErrorMessage.FormatString(exception.Message), periodicalList.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                if (journalCollection != null)
                {
                    MessageBox.Show(periodicalList, Properties.Resources.PubMedMacroResultMessage.FormatString(journalCollection.Count.ToString()), periodicalList.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    journalCollection = null;
                }

            }

        }

        private class WebClient2 : System.Net.WebClient
        {
            public int Timeout { get; set; }

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest lWebRequest = base.GetWebRequest(uri);
                lWebRequest.Timeout = Timeout;
                ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;
                return lWebRequest;
            }
        }
    }
}
