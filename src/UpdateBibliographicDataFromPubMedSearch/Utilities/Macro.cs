using SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.DataExchange;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch
{
    internal static class Macro
    {
        public async static void Run(MainForm mainForm, MacroSettings fields)
        {
            var counter = 0;

            try
            {

                var references = mainForm.GetFilteredReferences();
                var referencesWithDoi = references.Where(reference => !string.IsNullOrEmpty(reference.PubMedId) || !string.IsNullOrEmpty(reference.Doi)).ToList();
                var project = mainForm.Project;
                var identifierSupport = new ReferenceIdentifierSupport();
                var currentRef = 0;
                var overallRefs = referencesWithDoi.Count();

                foreach (Reference reference in referencesWithDoi)
                {
                    currentRef++;

                    Reference lookedUpReference = await identifierSupport.FindReferenceAsync(project, new ReferenceIdentifier() { Type = ReferenceIdentifierType.PubMedId, Value = reference.PubMedId }, CancellationToken.None);
                    if (lookedUpReference == null) continue;

                    var omitData = new List<ReferencePropertyId>
                    {
                        ReferencePropertyId.CoverPath,
                        ReferencePropertyId.Locations
                    };

                    if (!fields.OverwriteAbstract) omitData.Add(ReferencePropertyId.Abstract);
                    if (!fields.OverwriteTableOfContents) omitData.Add(ReferencePropertyId.TableOfContents);
                    if (!fields.OverwriteKeywords) omitData.Add(ReferencePropertyId.Keywords);

                    reference.MergeReference(lookedUpReference, true, omitData);

                    counter++;

                    if (!string.IsNullOrEmpty(reference.Notes) && fields.ClearNotes) reference.Notes = string.Empty;
                    if (project.Engine.Settings.BibTeXCitationKey.IsTeXEnabled) reference.BibTeXKey = project.BibTeXKeyAssistant.GenerateKey(reference);
                    if (project.Engine.Settings.BibTeXCitationKey.IsCitationKeyEnabled) reference.CitationKey = project.CitationKeyAssistant.GenerateKey(reference);
                }
            }

            finally
            {
                MessageBox.Show(mainForm, UpdateBibliographicDataFromPubMedSearchResources.MacroFinallyMessage.FormatString(counter.ToString()), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
