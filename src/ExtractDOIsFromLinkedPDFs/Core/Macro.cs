﻿using SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs.Properties;
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

namespace SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs
{
    internal static class Macro
    {
        public static async Task Run(Form form, Project project)
        {
            if (project.ProjectType == ProjectType.DesktopCloud)
            {
                var cts = new CancellationTokenSource();

                try
                {
                    await GenericProgressDialog.RunTask(form, FetchAllAttributes, project, ExtractDOIsFromLinkedPDFsResources.GenericDialogFetchAttributsTitle, null, cts);
                }
                catch (OperationCanceledException x)
                {
                    // What exactly does Task.WhenAll do when a cancellation is requested? We don't know and are too lazy to find out ;-)
                    // To be on the safe side, we catch a possible exception and return;
                    return;
                }

                // But probably, we will land and return here...
                if (cts.IsCancellationRequested) return;


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
                    MessageBox.Show(form, ExtractDOIsFromLinkedPDFsResources.UserMessageUnavailableAttachements, "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }


            var fileLocations = (from location in project.AllLocations
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
                                 select (Location: location, Path: path)).ToList();

            try
            {
                await GenericProgressDialog.RunTask(form, FindDois, fileLocations);
            }
            catch (OperationCanceledException)
            {
                // Do nothing when cancelled
            }
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


        static Task FindDois(List<(Location Location, string Path)> fileLocations, IProgress<PercentageAndTextProgressInfo> progress, CancellationToken cancellationToken)
        {
            var supporter = new ReferenceIdentifierSupport();
            var counter = 0;

            return Task.Run(() =>
            {
                foreach (var item in fileLocations)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    counter++;
                    var percentage = counter * 100 / fileLocations.Count;

                    if (!string.IsNullOrEmpty(item.Location.Reference.Doi))
                    {
                        // Another location of the same reference has already added the DOI
                        progress.ReportSafe(item.Location.ToString(), percentage);
                        continue;
                    }

                    var matches = supporter.FindIdentifierInFile(item.Path, ReferenceIdentifierType.Doi, false);
                    if (matches.Count == 0)
                    {
                        progress.ReportSafe(item.Location.ToString(), percentage);
                        continue;
                    }

                    var match = matches.ElementAt(0);

                    item.Location.Reference.Doi = match.ToString();
                    progress.ReportSafe(item.Location.ToString(), percentage);
                }
            });
        }
    }
}