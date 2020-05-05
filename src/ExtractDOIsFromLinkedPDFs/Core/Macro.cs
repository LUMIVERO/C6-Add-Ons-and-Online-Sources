using SwissAcademic.Addons.ExtractDOIsFromLinkedPDFsAddon.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.DataExchange;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ExtractDOIsFromLinkedPDFsAddon
{
    internal static class Macro
    {
        public static async Task Run(MainForm mainForm, Project project)
        {
            if (project.ProjectType == ProjectType.DesktopCloud)
            {
                using (var cts = new CancellationTokenSource())
                {
                    try
                    {
                        await GenericProgressDialog.RunTask(mainForm, FetchAllAttributes, project, Resources.GenericDialogFetchAttributsTitle, null, cts);
                    }
                    catch (OperationCanceledException)
                    {
                        // What exactly does Task.WhenAll do when a cancellation is requested? We don't know and are too lazy to find out ;-)
                        // To be on the safe side, we catch a possible exception and return;
                        return;
                    }

                    // But probably, we will land and return here...
                    if (cts.IsCancellationRequested) return;
                }

                var hasUnavailableAttachments = (from location in project.AllLocations
                                                 where
                                                     location.LocationType == LocationType.ElectronicAddress &&
                                                     string.IsNullOrEmpty(location.Reference.Doi) &&
                                                     location.Address.LinkedResourceType == LinkedResourceType.AttachmentRemote &&
                                                     location.Address.Properties.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase) &&
                                                     location.Address.CachingStatus != CachingStatus.Available
                                                 select location).Any();

                if (hasUnavailableAttachments)
                {
                    MessageBox.Show(mainForm, Resources.UserMessageUnavailableAttachements, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }


            var containers = (from location in project.AllLocations
                              where
                                  location.LocationType == LocationType.ElectronicAddress &&
                                  string.IsNullOrEmpty(location.Reference.Doi) &&
                                  ((location.Address.LinkedResourceType == LinkedResourceType.AttachmentRemote &&
                                  location.Address.CachingStatus == CachingStatus.Available) ||
                                  (
                                      location.Address.LinkedResourceType == LinkedResourceType.AttachmentFile ||
                                      location.Address.LinkedResourceType == LinkedResourceType.AbsoluteFileUri ||
                                      location.Address.LinkedResourceType == LinkedResourceType.RelativeFileUri
                                  ))
                              let path = location.Address.Resolve().GetLocalPathSafe()
                              where
                                 File.Exists(path) &&
                                 Path.GetExtension(path).Equals(".pdf", StringComparison.OrdinalIgnoreCase)
                              select new LocationContainer { Location = location, Path = path }).ToList();
            var isCanceled = false;
            try
            {
                await GenericProgressDialog.RunTask(mainForm, FindDois, containers);
            }
            catch (OperationCanceledException)
            {
                isCanceled = true;
            }

            if (!isCanceled) MessageBox.Show(mainForm, Resources.ProcessFinishMessage, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static Task FetchAllAttributes(Project project, CancellationToken cancellationToken)
        {
            return Task.WhenAll(from location in project.AllLocations
                                where
                                    location.LocationType == LocationType.ElectronicAddress &&
                                    string.IsNullOrEmpty(location.Reference.Doi) &&
                                    location.Address.LinkedResourceType == LinkedResourceType.AttachmentRemote &&
                                    location.Address.CachingStatus != CachingStatus.Available
                                select location.Address.FetchAttributesAsync(cancellationToken));
        }

        static Task FindDois(List<LocationContainer> containers, IProgress<PercentageAndTextProgressInfo> progress, CancellationToken cancellationToken)
        {
            var supporter = new ReferenceIdentifierSupport();
            var counter = 0;

            return Task.Run(() =>
            {
                foreach (var container in containers)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    counter++;
                    var percentage = counter * 100 / containers.Count;

                    if (!string.IsNullOrEmpty(container.Location.Reference.Doi))
                    {
                        // Another location of the same reference has already added the DOI
                        progress.ReportSafe(container.Location.ToString(), percentage);
                        continue;
                    }

                    var matches = supporter.FindIdentifierInFile(container.Path, 5, ReferenceIdentifierType.Doi, false);
                    if (matches.Count == 0)
                    {
                        progress.ReportSafe(container.Location.ToString(), percentage);
                        continue;
                    }

                    var match = matches.ElementAt(0);

                    container.Location.Reference.Doi = match.ToString();
                    progress.ReportSafe(container.Location.ToString(), percentage);
                }
            });
        }
    }
}
