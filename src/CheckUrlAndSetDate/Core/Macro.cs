using SwissAcademic.Addons.CheckUrlAndSetDate.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissAcademic.Addons.CheckUrlAndSetDate
{
    internal static class Macro
    {
        public async static void Run(MainForm mainForm)
        {
            var referencesWithUrl = mainForm.GetFilteredReferences()
                                            .Where(reference => !String.IsNullOrEmpty(reference.OnlineAddress))
                                            .ToList();

            if (referencesWithUrl.Count == 0)
            {
                MessageBox.Show(mainForm, CheckUrlAndSetDateResources.NoReferencesFoundedMessage, mainForm.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                return;
            }

            var isCanceled = false;
            MacroResult result = null;

            try
            {
                result = await GenericProgressDialog.RunTask(mainForm, CheckReferences, referencesWithUrl);
            }
            catch (OperationCanceledException)
            {
                isCanceled = true;
            }

            if (isCanceled) return;

            if (MessageBox.Show(string.Format(CheckUrlAndSetDateResources.MacroResultMessage, referencesWithUrl.Count.ToString(), result.ChangedCount.ToString(), result.InvalidCount.ToString()), mainForm.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes && result.InvalidReferences.Count > 0)
            {
                var filter = new ReferenceFilter(result.InvalidReferences, CheckUrlAndSetDateResources.ReferenceInvalidFilterName, false);
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

        static Task<MacroResult> CheckReferences(List<Reference> references, IProgress<PercentageAndTextProgressInfo> progress, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var result = new MacroResult();
                var timeOut = 3000;
                var newAccessDate = DateTime.Today.ToString(Program.Engine.Settings.General.DateTimeFormat);

                for (int i = 0; i < references.Count; i++)
                {
                    var reference = references[i];

                    if (reference == null)
                    {
                        progress.ReportSafe(reference.ToString(), (100 / references.Count * i));
                        continue;
                    }

                    cancellationToken.ThrowIfCancellationRequested();

                    var location = (from currentLocation in reference.Locations
                                    where currentLocation.MirrorsReferenceOnlineAddress == ReferencePropertyDescriptor.OnlineAddress
                                    select currentLocation).FirstOrDefault();

                    if (location == null || location.Address == null || location.Address.LinkedResourceType != LinkedResourceType.RemoteUri)
                    {
                        progress.ReportSafe(reference.ToString(), (100 / references.Count * i));
                        continue;
                    }

                    var url = location.Address.Resolve().ToString();

                    result.LoopedCount++;

                    var oldAccessDate = reference.AccessDate;

                    if (RemoteFileExists(url, timeOut, out string urlResult))
                    {
                        reference.Notes += String.Format(CheckUrlAndSetDateResources.LinkCheckNotes, reference.OnlineAddress, DateTime.Now.ToString(), urlResult, oldAccessDate);
                        reference.AccessDate = newAccessDate;
                        result.ChangedCount++;
                    }
                    else
                    {
                        reference.Notes += String.Format(CheckUrlAndSetDateResources.LinkCheckNotes, reference.OnlineAddress, DateTime.Now.ToString(), urlResult, oldAccessDate);
                        result.InvalidCount++;
                        result.InvalidReferences.Add(reference);
                    }
                    progress.ReportSafe(reference.ToString(), (100 / references.Count * i));
                }
                return result;
            });
        }
    }
}
