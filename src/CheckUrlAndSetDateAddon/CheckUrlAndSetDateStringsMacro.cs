using CheckUrlAndSetDateAddon.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace CheckUrlAndSetDateAddon
{
    internal static class CheckUrlAndSetDateStringsMacro
    {
        public static void Run(MainForm mainForm)
        {
            //****************************************************************************************************************
            // This macro checks URLs for validity and sets AccessDate to today's date or the error message of the URL check
            // Version 1.5 -- 2016-01-19
            //
            //
            // EDIT HERE

            var timeOut = 3000; // time in milliseconds until URL check is aborted
            string setToDate = null; // if not null, string is used for AccessDate, e.g. setToDate = "05.02.2013", otherwise today's date;
            var setDateAlways = false; // if true, AccessDate is set regardless of outcome of URL check

            // DO NOT EDIT BELOW THIS LINE
            // ****************************************************************************************************************

            if (MessageBox.Show(mainForm, CheckUrlAndSetDateStrings.IsBackupAvailableMessage.Replace("\r\n", Environment.NewLine), "Citavi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK) return;

            var dateTimeFormat = Program.Engine.Settings.General.DateTimeFormat;
            var newAccessDate = setToDate ?? DateTime.Today.ToString(dateTimeFormat);

            //iterate over all references in the current filter (or over all, if there is no filter)
            var references = mainForm.GetFilteredReferences();
            var referencesWithUrl = references.Where(reference => !String.IsNullOrEmpty(reference.OnlineAddress)).ToList();
            var referencesWithInvalidUrls = new List<Reference>();

            //reference to active Project
            var activeProject = mainForm.Project;

            var loopCounter = 0;
            var changeCounter = 0;
            var invalidCounter = 0;

            if (referencesWithUrl.Count == 0)
            {
                MessageBox.Show(mainForm, CheckUrlAndSetDateStrings.NoReferencesFoundedMessage, "Citavi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (var reference in referencesWithUrl)
            {

                loopCounter++;

                // get URL to check
                var url = reference.OnlineAddress;

                // get previous last access date
                var oldAccessDate = reference.AccessDate;

                if (RemoteFileExists(url, timeOut, out string urlResult))
                {
                    reference.Notes += String.Format(CheckUrlAndSetDateStrings.LinkCheckNotes, reference.OnlineAddress, DateTime.Now.ToString(), urlResult, oldAccessDate);
                    reference.AccessDate = newAccessDate;
                    changeCounter++;
                }
                else
                {
                    reference.Notes += String.Format(CheckUrlAndSetDateStrings.LinkCheckNotes, reference.OnlineAddress, DateTime.Now.ToString(), urlResult, oldAccessDate);
                    if (setDateAlways) reference.AccessDate = newAccessDate;
                    invalidCounter++;
                    referencesWithInvalidUrls.Add(reference);
                }
            }

            if (MessageBox.Show(string.Format(CheckUrlAndSetDateStrings.MacroResultMessage, referencesWithUrl.Count.ToString(), changeCounter.ToString(), invalidCounter.ToString()), "Citavi", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes && referencesWithInvalidUrls.Count > 0)
            {
                var filter = new ReferenceFilter(referencesWithInvalidUrls, CheckUrlAndSetDateStrings.ReferenceInvalidFilterName, false);
                mainForm.ReferenceEditorFilterSet.Filters.ReplaceBy(new List<ReferenceFilter> { filter });
            }

        }

        static bool RemoteFileExists(string url, int timeOut, out string urlResult)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                request.Timeout = timeOut;

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    urlResult = null;

                    if ((int)response.StatusCode >= 300 && (int)response.StatusCode <= 400)
                    {
                        if (response.Headers["Location"] != null)
                        {
                            urlResult = ((int)response.StatusCode).ToString() + " " + response.StatusDescription + " - Redirect: " + response.Headers["Location"].ToStringSafe();
                        }
                        else
                        {
                            urlResult = ((int)response.StatusCode).ToString() + " " + response.StatusDescription;
                        }
                    }
                    else
                    {
                        urlResult = ((int)response.StatusCode).ToString() + " " + response.StatusDescription;
                    }

                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception e)
            {
                urlResult = e.Message.ToString();
                return false;
            }
        }
    }
}
