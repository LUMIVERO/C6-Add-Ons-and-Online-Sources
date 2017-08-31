using SwissAcademic.Addons.CheckUrlAndSetDate.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace SwissAcademic.Addons.CheckUrlAndSetDate
{
    internal static class CheckUrlAndSetDateStringsMacro
    {
        public static void Run(MainForm mainForm)
        {

            var timeOut = 3000;
            string setToDate = null;
            var setDateAlways = false;

            var dateTimeFormat = Program.Engine.Settings.General.DateTimeFormat;
            var newAccessDate = setToDate ?? DateTime.Today.ToString(dateTimeFormat);

            var references = mainForm.GetFilteredReferences();
            var referencesWithUrl = references.Where(reference => !String.IsNullOrEmpty(reference.OnlineAddress)).ToList();
            var referencesWithInvalidUrls = new List<Reference>();


            var activeProject = mainForm.Project;

            var loopCounter = 0;
            var changeCounter = 0;
            var invalidCounter = 0;

            if (referencesWithUrl.Count == 0)
            {
                MessageBox.Show(mainForm, CheckUrlAndSetDateResources.NoReferencesFoundedMessage, "Citavi", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (var reference in referencesWithUrl)
            {

                var location = (from l in reference.Locations
                                where l.MirrorsReferenceOnlineAddress == ReferencePropertyDescriptor.OnlineAddress
                                select l).FirstOrDefault();

                if (location?.Address.LinkedResourceType != LinkedResourceType.RemoteUri) continue;

                var url = location.Address.Resolve().ToString();

                loopCounter++;

                var oldAccessDate = reference.AccessDate;

                if (RemoteFileExists(url, timeOut, out string urlResult))
                {
                    reference.Notes += String.Format(CheckUrlAndSetDateResources.LinkCheckNotes, reference.OnlineAddress, DateTime.Now.ToString(), urlResult, oldAccessDate);
                    reference.AccessDate = newAccessDate;
                    changeCounter++;
                }
                else
                {
                    reference.Notes += String.Format(CheckUrlAndSetDateResources.LinkCheckNotes, reference.OnlineAddress, DateTime.Now.ToString(), urlResult, oldAccessDate);
                    if (setDateAlways) reference.AccessDate = newAccessDate;
                    invalidCounter++;
                    referencesWithInvalidUrls.Add(reference);
                }
            }

            if (MessageBox.Show(string.Format(CheckUrlAndSetDateResources.MacroResultMessage, referencesWithUrl.Count.ToString(), changeCounter.ToString(), invalidCounter.ToString()), "Citavi", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes && referencesWithInvalidUrls.Count > 0)
            {
                var filter = new ReferenceFilter(referencesWithInvalidUrls, CheckUrlAndSetDateResources.ReferenceInvalidFilterName, false);
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
