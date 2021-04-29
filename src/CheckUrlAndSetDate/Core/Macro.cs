using SwissAcademic.Addons.CheckUrlAndSetDateAddon.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissAcademic.Addons.CheckUrlAndSetDateAddon
{
    internal static class Macro
    {
        public async static Task RunAsync(MainForm mainForm)
        {
            var referencesWithUrl = mainForm.GetFilteredReferences().Where(reference => !string.IsNullOrEmpty(reference?.OnlineAddress) && GetRemoteUriLocation(reference) != null).ToList();

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

        async static Task<(bool, string)> RemoteFileExists(HttpClient client, string url, CancellationToken cancellationToken)
        {
            string urlResult;
            try
            {
                var response = await client.GetAsync(new Uri(url), HttpCompletionOption.ResponseContentRead, cancellationToken);

                if ((int)response.StatusCode >= 300 && (int)response.StatusCode <= 400)
                {
                    if (response.Headers.Location != null)
                    {
                        urlResult = ((int)response.StatusCode).ToString(Resources.Culture) + " " + response.Content + " - Redirect: " + response.Headers.Location.ToStringSafe();
                    }
                    else
                    {
                        urlResult = ((int)response.StatusCode).ToString(Resources.Culture) + " " + response.ReasonPhrase;
                    }
                }
                else
                {
                    urlResult = ((int)response.StatusCode).ToString(Resources.Culture) + " " + response.ReasonPhrase;
                }

                return (response.StatusCode == HttpStatusCode.OK, urlResult);
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

            var newAccessDate = DateTime.Today.ToString(Program.Engine.Settings.General.DateTimeFormat, Resources.Culture);

            using (var client = new HttpClient { Timeout = new TimeSpan(0, 0, 5) })
            {
                for (int i = 0; i < references.Count; i++)
                {
                    var reference = references[i];

                    var location = GetRemoteUriLocation(reference);

                    cancellationToken.ThrowIfCancellationRequested();

                    var url = location.Address.UriString;

                    result.LoopedCount++;

                    var oldAccessDate = reference.AccessDate ?? string.Empty;

                    (bool exist, string urlResult) = await RemoteFileExists(client, url, cancellationToken);

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

                    progress.ReportSafe(reference.ToString(), 100 / references.Count * i);
                }
            }
            return result;
        }

        static Location GetRemoteUriLocation(Reference reference)
        {
            var location = reference?.Locations.FirstOrDefault(currentLocation => currentLocation.MirrorsReferenceOnlineAddress == ReferencePropertyDescriptor.OnlineAddress);

            if (location == null || location.Address == null || location.Address.LinkedResourceType != LinkedResourceType.RemoteUri)
            {
                return null;
            }

            return location;
        }
    }
}
