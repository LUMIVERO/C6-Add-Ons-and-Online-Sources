using SwissAcademic.Citavi;
using System;

namespace SwissAcademic.Addons.PushAndMerge
{
    public class PushAndMergeOptions
    {
        #region Properties

        #region CopyAllNonMatchedReferences
        public bool CopyAllNonMatchedReferences { get; set; } = false;
        #endregion

        #region IncludeAbstract

        public bool IncludeAbstract { get; set; } = true;

        #endregion

        #region IncludeCategories

        public bool IncludeCategories { get; set; } = true;

        #endregion

        #region IncludeCustomField1

        public bool IncludeCustomField1 { get; set; } = true;

        #endregion

        #region IncludeCustomField2

        public bool IncludeCustomField2 { get; set; } = true;

        #endregion

        #region IncludeCustomField3

        public bool IncludeCustomField3 { get;  set; } = true;

        #endregion

        #region IncludeCustomField4

        public bool IncludeCustomField4 { get; set; } = true;

        #endregion

        #region IncludeCustomField5

        public bool IncludeCustomField5 { get; set; } = true;

        #endregion

        #region IncludeCustomField6

        public bool IncludeCustomField6 { get; set; } = true;

        #endregion

        #region IncludeCustomField7

        public bool IncludeCustomField7 { get; set; } = true;

        #endregion

        #region IncludeCustomField8

        public bool IncludeCustomField8 { get; set; } = true;

        #endregion

        #region IncludeCustomField9

        public bool IncludeCustomField9 { get; set; } = true;

        #endregion

        #region IncludeEvaluation

        public bool IncludeEvaluation { get; set; } = true;

        #endregion

        #region IncludeGroups

        public bool IncludeGroups { get; set; } = true;

        #endregion

        #region IncludeKeywords

        public bool IncludeKeywords { get; set; } = true;

        #endregion

        #region IncludeLocations

        public bool IncludeLocations { get; set; } = true;

        #endregion

        #region IncludeNotes

        public bool IncludeNotes { get; set; } = true;

        #endregion

        #region IncludeTableOfContents

        public bool IncludeTableOfContents { get; set; } = true;

        #endregion

        #region IncludeTasks
        public bool IncludeTasks { get; set; } = true;
        #endregion

        #region ReferenceSelection
        public ReferenceSelection ReferenceSelection { get; set; } = ReferenceSelection.Selected;
        #endregion

        #region IgnoreKnowledgeItemOnMatch
        public bool IgnoreKnowledgeItemOnMatch { get; set; } = true;
        #endregion

        #region MergeProjectOptions
        public MergeProjectOptions MergeProjectOptions { get; set; } = MergeProjectOptions.EqualIdentifiers | MergeProjectOptions.EqualEssentialFields | MergeProjectOptions.EqualStaticId;
        #endregion

        #region MergeKnowldgeItemKeywords
        public bool MergeKnowledgeItemKeywords { get; set; }
        #endregion

        #region MergeKnowldgeItemCategories
        public bool MergeKnowledgeItemCategories { get; set; }
        #endregion

        #region MergeKnowldgeItemGroups
        public bool MergeKnowldgeItemGroups { get; set; }
        #endregion

        #region MergeReferenceOptionAbstract
        public MergeReferenceContentOptions MergeReferenceOptionAbstract { get; set; } = MergeReferenceContentOptions.CompleIfNotEqual;
        #endregion

        #region MergeReferenceOptionTableOfContents
        public MergeReferenceContentOptions MergeReferenceOptionTableOfContents { get; set; } = MergeReferenceContentOptions.CompleIfNotEqual;
        #endregion

        #region MergeReferenceOptionEvaluation 
        public MergeReferenceContentOptions MergeReferenceOptionEvaluation { get; set; } = MergeReferenceContentOptions.CompleIfNotEqual;
        #endregion

        #region MergeReferenceOptionNotes
        public MergeReferenceContentOptions MergeReferenceOptionNotes { get; set; } = MergeReferenceContentOptions.CompleIfNotEqual;
        #endregion

        #region MergeReferenceOptionCategories

        public MergeReferenceOptions MergeReferenceOptionsCategories { get; set; } = MergeReferenceOptions.Ignore;

        #endregion

        #region MergeReferenceOptionsKeywords

        public MergeReferenceOptions MergeReferenceOptionsKeywords { get; set; } = MergeReferenceOptions.Merge;

        #endregion

        #region MergeReferenceOptionsGroups

        public MergeReferenceOptions MergeReferenceOptionsGroups { get; set; } = MergeReferenceOptions.Ignore;

        #endregion

        #endregion
    }

    [Flags]
    public enum MergeProjectOptions
    {
        EqualIdentifiers = 1,
        EqualEssentialFields = 1 << 1,
        EqualStaticId = 1 << 2
    }
    public enum MergeReferenceContentOptions
    {
        Ignore,
        Override,
        Complete,
        CompleteIfEmpty,
        CompleIfNotEqual
    }
    public enum MergeReferenceOptions
    {
        Ignore,
        Replace,
        Merge
    }
}
