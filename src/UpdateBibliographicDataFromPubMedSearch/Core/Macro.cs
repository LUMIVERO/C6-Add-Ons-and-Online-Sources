using SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.DataExchange;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch
{
    internal static class Macro
    {
        public async static void Run(MainForm mainForm, MacroSettings settings)
        {
            var referencesWithPmid = mainForm.GetFilteredReferences()
                                            .Where(reference => !string.IsNullOrEmpty(reference.PubMedId))
                                            .ToList();

            var referencesWithDoi = mainForm.GetFilteredReferences()
                                           .Where(reference => string.IsNullOrEmpty(reference.PubMedId) && !string.IsNullOrEmpty(reference.Doi))
                                           .ToList();

            try
            {
                var mergedReferences = await GenericProgressDialog.RunTask(mainForm, RunAsync, Tuple.Create(mainForm.Project, referencesWithPmid, referencesWithDoi, settings));

                if (mergedReferences.Any())
                {
                    if (MessageBox.Show(mainForm, Resources.ProcessFinishWithChangesMessage.FormatString(mergedReferences.Count()), mainForm.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        var filter = new ReferenceFilter(mergedReferences, "References with locations from file", false);
                        mainForm.ReferenceEditorFilterSet.Filters.ReplaceBy(new List<ReferenceFilter> { filter });
                    }
                }
                else
                {
                    MessageBox.Show(mainForm, Resources.ProcessFinishWithoutChangesMessage, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (OperationCanceledException)
            {
                // What exactly does Task.WhenAll do when a cancellation is requested? We don't know and are too lazy to find out ;-)
                // To be on the safe side, we catch a possible exception and return;
                return;
            }
        }

        static async Task<IEnumerable<Reference>> RunAsync(Tuple<Project, List<Reference>, List<Reference>, MacroSettings> tuple, IProgress<PercentageAndTextProgressInfo> progress, CancellationToken cancellationToken)
        {
            var project = tuple.Item1;
            var referencesWithPmid = tuple.Item2;
            var referencesWithDoi = tuple.Item3;
            var settings = tuple.Item4;
            var mergedReferences = new List<Reference>();

            var count = referencesWithDoi.Count + referencesWithPmid.Count;

            var identifierSupport = new ReferenceIdentifierSupport(project);

            // PMID

            for (int i = 0; i < referencesWithPmid.Count; i++)
            {
                var reference = referencesWithPmid[i];
                cancellationToken.ThrowIfCancellationRequested();

                var lookedUpReference = await identifierSupport.FindReferenceAsync(project, new ReferenceIdentifier() { Type = ReferenceIdentifierType.PubMedId, Value = reference.PubMedId }, cancellationToken);
                if (lookedUpReference == null)
                {
                    progress.ReportSafe(Convert.ToInt32(100.00 / count * i));
                    continue;
                }
                var omitData = new List<ReferencePropertyId>
                    {
                        ReferencePropertyId.CoverPath,
                        ReferencePropertyId.Locations
                    };

                if (reference.Periodical != null) omitData.Add(ReferencePropertyId.Periodical);

                if (!settings.OverwriteAbstract) omitData.Add(ReferencePropertyId.Abstract);
                if (!settings.OverwriteTableOfContents) omitData.Add(ReferencePropertyId.TableOfContents);
                if (!settings.OverwriteKeywords) omitData.Add(ReferencePropertyId.Keywords);

                reference.MergeReference(lookedUpReference, true, omitData);

                if (settings.ClearNotes) reference.Notes = string.Empty;
                if (project.Engine.Settings.BibTeXCitationKey.IsTeXEnabled) reference.BibTeXKey = project.BibTeXKeyAssistant.GenerateKey(reference);
                if (project.Engine.Settings.BibTeXCitationKey.IsCitationKeyEnabled) reference.CitationKey = project.CitationKeyAssistant.GenerateKey(reference);

                mergedReferences.Add(reference);
                progress.ReportSafe(Convert.ToInt32(100.00 / count * i));
            }

            // DOI

            for (int i = 0; i < referencesWithDoi.Count; i++)
            {
                var reference = referencesWithDoi[i];
                cancellationToken.ThrowIfCancellationRequested();

                var lookedUpReference = await identifierSupport.FindReferenceAsync(project, new ReferenceIdentifier() { Type = ReferenceIdentifierType.Doi, Value = reference.Doi }, cancellationToken);
                if (lookedUpReference == null)
                {
                    progress.ReportSafe(Convert.ToInt32(100.00 / count * (i + referencesWithPmid.Count)));
                    continue;
                }
                var omitData = new List<ReferencePropertyId>
                    {
                        ReferencePropertyId.CoverPath,
                        ReferencePropertyId.Locations
                    };

                if (reference.Periodical != null) omitData.Add(ReferencePropertyId.Periodical);

                if (!settings.OverwriteAbstract) omitData.Add(ReferencePropertyId.Abstract);
                if (!settings.OverwriteTableOfContents) omitData.Add(ReferencePropertyId.TableOfContents);
                if (!settings.OverwriteKeywords) omitData.Add(ReferencePropertyId.Keywords);

                reference.MergeReference(lookedUpReference, true, omitData);

                if (settings.ClearNotes) reference.Notes = string.Empty;
                if (project.Engine.Settings.BibTeXCitationKey.IsTeXEnabled) reference.BibTeXKey = project.BibTeXKeyAssistant.GenerateKey(reference);
                if (project.Engine.Settings.BibTeXCitationKey.IsCitationKeyEnabled) reference.CitationKey = project.CitationKeyAssistant.GenerateKey(reference);

                mergedReferences.Add(reference);
                progress.ReportSafe(Convert.ToInt32(100.00 / count * (i + referencesWithPmid.Count)));
            }


            return mergedReferences;
        }
    }
}
