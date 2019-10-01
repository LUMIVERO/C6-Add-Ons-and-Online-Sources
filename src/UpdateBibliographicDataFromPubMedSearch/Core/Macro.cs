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
            var referencesWithDoi = mainForm.GetFilteredReferences()
                                            .Where(reference => !string.IsNullOrEmpty(reference.PubMedId))
                                            .ToList();
            try
            {
                var mergedReferences = await GenericProgressDialog.RunTask(mainForm, RunAsync, Tuple.Create(mainForm.Project, referencesWithDoi, settings));

                if (mergedReferences.Count() != 0)
                {
                    if (MessageBox.Show(mainForm, UpdateBibliographicDataFromPubMedSearchResources.ProcessFinishWithChangesMessage.FormatString(mergedReferences.Count()), mainForm.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        var filter = new ReferenceFilter(mergedReferences, "References with locations from file", false);
                        mainForm.ReferenceEditorFilterSet.Filters.ReplaceBy(new List<ReferenceFilter> { filter });
                    }
                }
                else
                {
                    MessageBox.Show(mainForm, UpdateBibliographicDataFromPubMedSearchResources.ProcessFinishWithoutChangesMessage, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (OperationCanceledException)
            {
                // What exactly does Task.WhenAll do when a cancellation is requested? We don't know and are too lazy to find out ;-)
                // To be on the safe side, we catch a possible exception and return;
                return;
            }
        }

        static async Task<IEnumerable<Reference>> RunAsync(Tuple<Project, List<Reference>, MacroSettings> tuple, IProgress<PercentageAndTextProgressInfo> progress, CancellationToken cancellationToken)
        {
            var identifierSupport = new ReferenceIdentifierSupport();
            var references = tuple.Item2;
            var project = tuple.Item1;
            var settings = tuple.Item3;
            var mergedReferences = new List<Reference>();

            for (int i = 0; i < references.Count; i++)
            {
                var reference = references[i];
                cancellationToken.ThrowIfCancellationRequested();

                var lookedUpReference = await identifierSupport.FindReferenceAsync(project, new ReferenceIdentifier() { Type = ReferenceIdentifierType.PubMedId, Value = reference.PubMedId }, cancellationToken);
                if (lookedUpReference == null)
                {
                    progress.ReportSafe(Convert.ToInt32(100.00 / references.Count * i));
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
                progress.ReportSafe(Convert.ToInt32(100.00 / references.Count * i));
            }

            return mergedReferences;
        }
    }
}
