using SwissAcademic.Citavi;
using SwissAcademic.Citavi.DataExchange;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch
{
    internal static class Macro
    {
        public async static void Run(MainForm mainForm, MacroSettings settings)
        {
            var referencesWithDoi = mainForm
                                        .GetFilteredReferences()
                                        .Where(reference => !string.IsNullOrEmpty(reference.PubMedId))
                                        .ToList();
            try
            {
                await GenericProgressDialog.RunTask(mainForm, RunAsync, Tuple.Create(mainForm.Project, referencesWithDoi, settings));
            }
            catch (OperationCanceledException)
            {
                // What exactly does Task.WhenAll do when a cancellation is requested? We don't know and are too lazy to find out ;-)
                // To be on the safe side, we catch a possible exception and return;
                return;
            }
        }

        static async Task RunAsync(Tuple<Project, List<Reference>, MacroSettings> tuple, IProgress<PercentageAndTextProgressInfo> progress, CancellationToken cancellationToken)
        {
            var identifierSupport = new ReferenceIdentifierSupport();
            var references = tuple.Item2;
            var project = tuple.Item1;
            var settings = tuple.Item3;

            foreach (var reference in references)
            {
                var lookedUpReference = await identifierSupport.FindReferenceAsync(project, new ReferenceIdentifier() { Type = ReferenceIdentifierType.PubMedId, Value = reference.PubMedId }, CancellationToken.None);
                if (lookedUpReference == null)
                {
                    progress.ReportSafe(100 / references.Count * references.IndexOf(reference));
                    continue;
                }
                var omitData = new List<ReferencePropertyId>
                    {
                        ReferencePropertyId.CoverPath,
                        ReferencePropertyId.Locations
                    };

                if (!settings.OverwriteAbstract) omitData.Add(ReferencePropertyId.Abstract);
                if (!settings.OverwriteTableOfContents) omitData.Add(ReferencePropertyId.TableOfContents);
                if (!settings.OverwriteKeywords) omitData.Add(ReferencePropertyId.Keywords);

                reference.MergeReference(lookedUpReference, true, omitData);

                if (settings.ClearNotes) reference.Notes = string.Empty;
                if (project.Engine.Settings.BibTeXCitationKey.IsTeXEnabled) reference.BibTeXKey = project.BibTeXKeyAssistant.GenerateKey(reference);
                if (project.Engine.Settings.BibTeXCitationKey.IsCitationKeyEnabled) reference.CitationKey = project.CitationKeyAssistant.GenerateKey(reference);

                progress.ReportSafe(100 / references.Count * references.IndexOf(reference));
            }
        }
    }
}
