using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissAcademic.Addons.PushAndMerge
{
    public static class PushAndMergeHandler
    {
        #region Fields

        static DateTime _executionTime;
        static readonly Regex _dividerRegex = new Regex("<--- [0-9]{4}-(?:0[1-9]|1[0-2])-(?:0[1-9]|[1-2][0-9]|3[0-1])_(?:2[0-3]|[01][0-9])-[0-5][0-9] - Do not change --->");

        #endregion

        #region Properties

        #region TextDividerStart
        static string Divider => "---";//$"<--- {_executionTime.ToString("yyyy-MM-dd_HH-mm")} - Do not change --->";
        #endregion

        #endregion

        #region Methods

        #region AddCategories
        static void AddCategories(IEnumerable<Category> categoriesToAdd, Project sourceProject, Project targetProject, Reference target)
        {
            var list = new List<Category>();

            for (int i = 0; i < categoriesToAdd.Count(); i++)
            {
                list.Clear();
                var category = categoriesToAdd.ElementAt(i);

                list.Add(category);
                while (!category.IsRootCategory)
                {
                    list.Insert(0, category.ParentCategory);
                    category = category.ParentCategory;
                }

                ICategoryHierarchyParent parent = targetProject;
                for (int c = 0; c < list.Count; c++)
                {
                    var existing = parent.Categories.FindId(list[c].Id);

                    if (existing == null)
                    {
                        existing = parent.Categories.Where(x => x.Name.Equals(list[c].Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    }

                    if (existing == null)
                    {
                        parent = parent.Categories.Add(list[c].Clone(parent));
                    }
                    else
                    {
                        parent = existing;
                    }
                }
            }
            target.Categories.AddRange(categoriesToAdd);
        }

        #endregion

        #region ConnectAnnotations
        static void ConnectAnnotations(KnowledgeItem quotation, Location location, IEnumerable<Annotation> annotations)
        {
            if (quotation == null || location == null) return;
            ClonePool.Reset();
            foreach (var annotation in annotations)
            {
                var newAnnotation = annotation.Clone(location);
                newAnnotation = location.Annotations.Add(newAnnotation);

                location.Project.EntityLinks.Add(quotation, newAnnotation, EntityLink.PdfKnowledgeItemIndication);
            }
        }
        #endregion

        #region ExecuteAsync
        public static async Task ExecuteAsync(
            Form dialogOwner,
            Project sourceProject,
            Project targetProject, 
            PushAndMergeOptions options, 
            Progress<PercentageAndTextProgressInfo> progress, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            _executionTime = DateTime.Now;

            #region Selection
            var referenceSelectionSupporter = dialogOwner as ISupportReferenceSelection;

            IList<Reference> references = null;

            if (referenceSelectionSupporter == null)
            {
                references = sourceProject.References.ToList();
            }
            else
            {

                var referenceSelection = options.ReferenceSelection;

                if (referenceSelection == ReferenceSelection.Selected && referenceSelectionSupporter.IsAllSelected) referenceSelection = ReferenceSelection.Filter;
                if (referenceSelection == ReferenceSelection.Filter && !referenceSelectionSupporter.HasFilter) referenceSelection = ReferenceSelection.All;

                switch (referenceSelection)
                {
                    #region Filter

                    case ReferenceSelection.Filter:
                        references = referenceSelectionSupporter.GetFilteredReferences();
                        break;

                    #endregion

                    #region Selected

                    case ReferenceSelection.Selected:
                        references = referenceSelectionSupporter.GetSelectedReferences();
                        break;

                    #endregion

                    #region default

                    default:
                        {
                            references = sourceProject.References.ToList();
                        }
                        break;

                    #endregion
                }
            }
            #endregion

            targetProject.SuspendTrackingOfModificationInfo();

            try
            {
                var cloneOptions = new CitaviEntityCloneOptions
                {
                    CloneKnowledgeItemCategories = options.MergeKnowledgeItemCategories,
                    CloneKnowledgeItemKeywords = options.MergeKnowledgeItemKeywords,
                    CloneKnowledgeItemGroups = options.MergeKnowldgeItemGroups,
                    CloneReferenceAbstract = options.IncludeAbstract,
                    CloneReferenceCategories = options.IncludeCategories,
                    CloneReferenceCustomField1 = options.IncludeCustomField1,
                    CloneReferenceCustomField2 = options.IncludeCustomField2,
                    CloneReferenceCustomField3 = options.IncludeCustomField3,
                    CloneReferenceCustomField4 = options.IncludeCustomField4,
                    CloneReferenceCustomField5 = options.IncludeCustomField5,
                    CloneReferenceCustomField6 = options.IncludeCustomField6,
                    CloneReferenceCustomField7 = options.IncludeCustomField7,
                    CloneReferenceCustomField8 = options.IncludeCustomField8,
                    CloneReferenceCustomField9 = options.IncludeCustomField9,
                    CloneReferenceGroups = options.IncludeGroups,
                    CloneReferenceKeywords = options.IncludeKeywords,
                    CloneReferenceNotes = options.IncludeNotes,
                    CloneReferenceTableOfContents = options.IncludeTableOfContents,
                    MatchByReferenceIdentifier = options.MergeProjectOptions.HasFlag(MergeProjectOptions.EqualIdentifiers),
                    UpdateCreationAndModificationInfo = false
                };

                ClonePool.Reset();

                var clonesWithResult = references.CloneCollectionWithResults(targetProject, cloneOptions);
                var referencesToImport = new List<Reference>();
                var mergedRefrences = new List<Reference>();

                foreach (var r in clonesWithResult.Results)
                {
                    var reference = r.Clone as Reference;

                    if (reference == null) continue;

                    if (r.CloneResult == CloneResult.ReferenceIdentifierMatch)
                    {
                        MergeReferenceData((Reference)r.Source, reference, options);
                        await MergeKnowledgeItemsAsync((Reference)r.Source, reference, options);
                        mergedRefrences.Add(reference);
                        continue;
                    }

                    if (options.MergeProjectOptions.HasFlag(MergeProjectOptions.EqualStaticId))
                    {
                        foreach (var staticId in reference.StaticIds)
                        {
                            var matchedReference = targetProject.References.FindStaticId(staticId);

                            if (matchedReference != null)
                            {
                                MergeReferenceData(reference, matchedReference, options);
                                await MergeKnowledgeItemsAsync(reference, matchedReference, options);
                                mergedRefrences.Add(matchedReference);
                                continue;
                            }
                        }
                    }

                    if (options.MergeProjectOptions.HasFlag(MergeProjectOptions.EqualEssentialFields))
                    {
                        var matchedReference = targetProject.References.FirstOrDefault(
                            i => i.ReferenceType == reference.ReferenceType &&
                                 i.Authors.ContentEquals(reference.Authors, false) &&
                                 i.Title == reference.Title &&
                                 i.Subtitle == reference.Subtitle &&
                                 i.Year == reference.Year &&
                                 i.Edition == reference.Edition);

                        if (matchedReference != null)
                        {
                            MergeReferenceData(reference, matchedReference, options);
                            await MergeKnowledgeItemsAsync(reference, matchedReference, options);
                            mergedRefrences.Add(matchedReference);
                            continue;
                        }
                    }

                    if (options.CopyAllNonMatchedReferences)
                    {
                        referencesToImport.Add(reference);
                    }
                }
                targetProject.References.AddRange(referencesToImport);

                ImportGroup group = null;

                if(referencesToImport.Any())
                {
                    var importGroup = new ImportGroup(targetProject, ImportGroupType.FileImport);
                    importGroup.Source = "Add-On: Copied";
                    importGroup.References.AddRange(referencesToImport);

                    targetProject.ImportGroups.Add(importGroup);

                    group = importGroup;
                }

                if (mergedRefrences.Any())
                {
                    var importGroup = new ImportGroup(targetProject, ImportGroupType.FileImport);
                    importGroup.Source = "Add-On: Merged";
                    importGroup.References.AddRange(mergedRefrences);

                    targetProject.ImportGroups.Add(importGroup);

                    group = importGroup;
                }

                if (group != null)
                {
                    var shell = Program.ProjectShells.Single(i => i.Project == targetProject);
                    shell.PrimaryMainForm.ReferenceEditorFilterSet.Filters.ReplaceBy(new ReferenceFilter[] { new ReferenceFilter(group) });
                }
            }
            finally
            {
                targetProject.ResumeTrackingOfModificationInfo();
            }
        }
        #endregion

        #region HandleReferenceMergeOptions
        static string HandleReferenceMergeOptions(string source, string target, MergeReferenceContentOptions options)
        {
            switch (options)
            {
                case MergeReferenceContentOptions.Complete:
                    return $"{source}{System.Environment.NewLine}{Divider}{System.Environment.NewLine}{target}";
                case MergeReferenceContentOptions.CompleteIfEmpty:
                    return string.IsNullOrEmpty(target) ? source : target;
                case MergeReferenceContentOptions.CompleIfNotEqual:
                    return target.Equals(source, StringComparison.OrdinalIgnoreCase) ? target : SplitAndCompareTextData(source, target); 
                case MergeReferenceContentOptions.Ignore:
                    return target;
                case MergeReferenceContentOptions.Override:
                    return source;
                default: throw new InvalidOperationException();
            }
        }
        #endregion

        #region MergeReferenceData
        static void MergeReferenceData(Reference source, Reference target, PushAndMergeOptions options)
        {
            target.Abstract.Text = HandleReferenceMergeOptions(source.Abstract.Text, target.Abstract.Text, options.MergeReferenceOptionAbstract);
            target.Evaluation.Text = HandleReferenceMergeOptions(source.Evaluation.Text, target.Evaluation.Text, options.MergeReferenceOptionEvaluation);
            target.Notes = HandleReferenceMergeOptions(source.Notes, target.Notes, options.MergeReferenceOptionNotes);
            target.TableOfContents.Text = HandleReferenceMergeOptions(source.TableOfContents.Text, target.TableOfContents.Text, options.MergeReferenceOptionTableOfContents);

            if(options.OverrideRating)
            {
                target.Rating = source.Rating;
            }

            switch (options.MergeReferenceOptionsCategories)
            {
                case MergeReferenceOptions.Ignore: break;
                case MergeReferenceOptions.Replace:
                    target.Categories.Clear();
                    AddCategories(source.Categories, source.Project, target.Project, target);
                    break;
                case MergeReferenceOptions.Merge:
                    AddCategories(source.Categories, source.Project, target.Project, target);
                    break;
            }
            switch(options.MergeReferenceOptionsKeywords)
            {
                case MergeReferenceOptions.Ignore: break;
                case MergeReferenceOptions.Replace:
                    target.Keywords.Clear();
                    target.Keywords.AddRange(source.Keywords.CloneCollection(target.Project));
                    break;
                case MergeReferenceOptions.Merge:
                    target.Keywords.AddRange(source.Keywords.CloneCollection(target.Project));
                    break;
            }
            switch (options.MergeReferenceOptionsGroups)
            {
                case MergeReferenceOptions.Ignore: break;
                case MergeReferenceOptions.Replace:
                    target.Groups.Clear();
                    target.Groups.AddRange(source.Groups.CloneCollection(target.Project));
                    break;
                case MergeReferenceOptions.Merge:
                    target.Groups.AddRange(source.Groups.CloneCollection(target.Project));
                    break;
            }
        }
        #endregion

        #region MergeKnowledgeItemsAsync
        static async Task MergeKnowledgeItemsAsync(Reference source, Reference target, PushAndMergeOptions options)
        {
            ClonePool.Reset();
            var cloneOptions = new CitaviEntityCloneOptions
            {
                CloneKnowledgeItemCategories = options.MergeKnowledgeItemCategories,
                CloneKnowledgeItemKeywords = options.MergeKnowledgeItemKeywords,
                CloneKnowledgeItemGroups = options.MergeKnowldgeItemGroups,
                UpdateCreationAndModificationInfo = false,
                CreateNewId = true,
                MatchById = false
            };
            
            foreach (var quotation in source.Quotations)
            {
                var matchedKnowledgeItem = target.Quotations.FirstOrDefault(i => i.CreatedBy == quotation.CreatedBy && i.CreatedOn == quotation.CreatedOn);

                Location l = null;
                KnowledgeItem newQuotation = null;

                if (matchedKnowledgeItem != null)
                {
                    if (options.IgnoreKnowledgeItemOnMatch) continue;

                    newQuotation = quotation.Clone(target, cloneOptions);
                }
                else
                {
                    newQuotation = quotation.Clone(target, cloneOptions);
                }


                newQuotation = target.Quotations.Add(newQuotation);

                var quotationAnnotations = quotation.EntityLinks.Where(i => i.Target is Annotation).Select(i => i.Target as Annotation);

                if (quotationAnnotations.Any())
                {
                    if (!target.Locations.Any(
                        i => i.Address?.LinkedResourceType == LinkedResourceType.AbsoluteFileUri || 
                        i.Address?.LinkedResourceType == LinkedResourceType.RelativeFileUri || 
                        i.Address?.LinkedResourceType == LinkedResourceType.AttachmentFile || 
                        i.Address?.LinkedResourceType == LinkedResourceType.AttachmentRemote))
                    {
                        l = quotationAnnotations.First().Location.Clone(newQuotation.Reference);
                        l.Annotations.Clear();

                        l = target.Locations.Add(l);
                    }
                    else
                    {
                        l = await quotationAnnotations.First().Location.TryFindEqualFileLocation(newQuotation.Reference.Locations);
                    }

                    ConnectAnnotations(newQuotation, l, quotationAnnotations);
                }
            }
       
        }
        #endregion

        #region SplitAndCompareTextData

        static string SplitAndCompareTextData(string sourceText, string targetText)
        {
            try
            {

                sourceText = sourceText.Trim();
                targetText = targetText.Trim();

                var sourceValues = Regex.Split(sourceText, Divider);
                var targetValues = Regex.Split(targetText, Divider);

                var sourceValuesList = new List<string>();
                var targetValuesList = new List<string>();

                foreach(var i in sourceValues)
                {
                    var value = i.Trim();

                    if (string.IsNullOrEmpty(value)) continue;
                    sourceValuesList.Add(value);
                }

                foreach (var i in targetValues)
                {
                    var value = i.Trim();

                    if (string.IsNullOrEmpty(value)) continue;
                    targetValuesList.Add(value);
                }

                var builder = new StringBuilder();
                foreach (var i in sourceValuesList)
                {
                    if (string.IsNullOrEmpty(i.Trim())) continue;

                    if (!targetValuesList.Contains(i.Trim()))
                    {
                        builder.AppendLine($"{Divider}{System.Environment.NewLine}{i.Trim()}");
                    }
                }

                var result = builder.ToString();

                if (!targetText.StartsWith(Divider))
                {
                    builder.AppendLine(Divider);
                }

                return string.IsNullOrEmpty(result) ? targetText : $"{builder}{targetText}";
            }
            catch
            {
                return $"{Divider}{System.Environment.NewLine}{sourceText}{System.Environment.NewLine}{Divider}{System.Environment.NewLine}{targetText}";
            }
        }

        #endregion

        #endregion
    }
}
