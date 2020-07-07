using SwissAcademic.Addons.CheckUrlAndSetDateAddon.Properties;
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

namespace SwissAcademic.Addons.CheckUrlAndSetDateAddon
{
    internal static class Macro
    {
        public async static Task Run(MainForm mainForm)
        {
            var referencesWithUrl = mainForm.GetFilteredReferences()
                                            .Where(reference => !string.IsNullOrEmpty(reference.OnlineAddress))
                                            .ToList();

            if (referencesWithUrl.Count == 0)
            {
                MessageBox.Show(mainForm, Resources.NoReferencesFoundedMessage, mainForm.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                var result = await GenericProgressDialog.RunTask(mainForm, CheckReferencesAsync, referencesWithUrl);

                if (result.InvalidCount != 0)
                {
                    if (MessageBox.Show(string.Format(Resources.MacroResultMessage, referencesWithUrl.Count, result.ChangedCount, result.InvalidCount), mainForm.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes && result.InvalidReferences.Count > 0)
                    {
                        var filter = new ReferenceFilter(result.InvalidReferences, Resources.ReferenceInvalidFilterName, false);
                        mainForm.ReferenceEditorFilterSet.Filters.ReplaceBy(new List<ReferenceFilter> { filter });
                    }
                }
                else
                {
                    MessageBox.Show(string.Format(Resources.MacroResultMessageWithoutSelection, referencesWithUrl.Count.ToString(), result.ChangedCount.ToString(), result.InvalidCount.ToString()), mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (OperationCanceledException) { }
        }

        async static Task<(bool, string)> RemoteFileExists(string url, int timeOut)
        {
            string urlResult;
            try
            {
                var request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                request.Method = "HEAD";
                request.Timeout = timeOut;


                using (var response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    if ((int)response.StatusCode >= 300 && (int)response.StatusCode <= 400)
                    {
                        if (response.Headers["Location"] != null)
                        {
                            urlResult = ((int)response.StatusCode).ToString(Resources.Culture) + " " + response.StatusDescription + " - Redirect: " + response.Headers["Location"].ToStringSafe();
                        }
                        else
                        {
                            urlResult = ((int)response.StatusCode).ToString(Resources.Culture) + " " + response.StatusDescription;
                        }
                    }
                    else
                    {
                        urlResult = ((int)response.StatusCode).ToString(Resources.Culture) + " " + response.StatusDescription;
                    }

                    return (response.StatusCode == HttpStatusCode.OK, urlResult);
                }
            }
            catch (Exception e)
            {
                urlResult = e.Message.ToString();
                return (false, urlResult);
            }
        }

        async static Task<MacroResult> CheckReferencesAsync(List<Reference> references, IProgress<PercentageAndTextProgressInfo> progress, CancellationToken cancellationToken)
        {

            var result = new MacroResult();
            var timeOut = 3000;
            var newAccessDate = DateTime.Today.ToString(Program.Engine.Settings.General.DateTimeFormat, Resources.Culture);

            for (int i = 0; i < references.Count; i++)
            {
                var reference = references[i];

                if (reference == null)
                {
                    progress.ReportSafe(reference.ToString(), (100 / references.Count * i));
                    continue;
                }

                cancellationToken.ThrowIfCancellationRequested();

                var location = reference
                                .Locations
                                .FirstOrDefault(currentLocation => currentLocation.MirrorsReferenceOnlineAddress == ReferencePropertyDescriptor.OnlineAddress);

                if (location == null || location.Address == null || location.Address.LinkedResourceType != LinkedResourceType.RemoteUri)
                {
                    progress.ReportSafe(reference.ToString(), 100 / references.Count * i);
                    continue;
                }

                var url = location.Address.UriString;

                result.LoopedCount++;

                var oldAccessDate = reference.AccessDate ?? string.Empty;

                (bool exist, string urlResult) = await RemoteFileExists(url, timeOut);

                if (exist)
                {
                    reference.Notes += string.Format(Resources.Culture, Resources.LinkCheckNotes, reference.OnlineAddress ?? string.Empty, DateTime.Now.ToString(Resources.Culture), urlResult ?? string.Empty, oldAccessDate ?? string.Empty);
                    reference.AccessDate = newAccessDate;
                    result.ChangedCount++;
                }
                else
                {
                    reference.Notes += string.Format(Resources.Culture, Resources.LinkCheckNotes, reference.OnlineAddress ?? string.Empty, DateTime.Now.ToString(Resources.Culture), urlResult ?? string.Empty, oldAccessDate ?? string.Empty);
                    result.InvalidCount++;
                    result.InvalidReferences.Add(reference);
                }
                progress.ReportSafe(reference.ToString(), (100 / references.Count * i));
            }
            return result;
        }
    }
}
