using SwissAcademic.Citavi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushAndMerge
{
    public class PushAndMergeOptions
    {
        #region Properties

        #region CopyAllNonMatchedReferences
        public bool CopyAllNonMatchedReferences { get; set; }
        #endregion

        #region IncludeAbstract

        public bool IncludeAbstract { get; set; }

        #endregion

        #region IncludeCategories

        public bool IncludeCategories { get; set; }

        #endregion

        #region IncludeCustomField1

        public bool IncludeCustomField1 { get; set; }

        #endregion

        #region IncludeCustomField2

        public bool IncludeCustomField2 { get; set; }

        #endregion

        #region IncludeCustomField3

        public bool IncludeCustomField3 { get;  set; }

        #endregion

        #region IncludeCustomField4

        public bool IncludeCustomField4 { get; set; }

        #endregion

        #region IncludeCustomField5

        public bool IncludeCustomField5 { get; set; }

        #endregion

        #region IncludeCustomField6

        public bool IncludeCustomField6 { get; set; }

        #endregion

        #region IncludeCustomField7

        public bool IncludeCustomField7 { get; set; }

        #endregion

        #region IncludeCustomField8

        public bool IncludeCustomField8 { get; set; }

        #endregion

        #region IncludeCustomField9

        public bool IncludeCustomField9 { get; set; }

        #endregion

        #region IncludeEvaluation

        public bool IncludeEvaluation { get; set; }

        #endregion

        #region IncludeGroups

        public bool IncludeGroups { get; set; } 

        #endregion

        #region IncludeKeywords

        public bool IncludeKeywords { get; set; }

        #endregion

        #region IncludeLocations

        public bool IncludeLocations { get; set; }

        #endregion

        #region IncludeNotes

        public bool IncludeNotes { get; set; }

        #endregion

        #region IncludeTableOfContents

        public bool IncludeTableOfContents { get; set; }

        #endregion

        #region IncludeTasks

        public bool IncludeTasks { get; set; }


        #region ReferenceSelection
        public ReferenceSelection ReferenceSelection { get; set; }
        #endregion

        #endregion

        #region IgnoreKnowledgeItemOnMatch
        public bool IgnoreKnowledgeItemOnMatch { get; set; }
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
        public MergeReferenceOptions MergeReferenceOptionAbstract { get; set; }
        #endregion

        #region MergeReferenceOptionTableOfContents
        public MergeReferenceOptions MergeReferenceOptionTableOfContents { get; set; }
        #endregion

        #region MergeReferenceOptionRating
        public MergeReferenceOptions MergeReferenceOptionRating { get; set; }
        #endregion

        #region MergeReferenceOptionNote
        public MergeReferenceOptions MergeReferenceOptionNote { get; set; }
        #endregion

        #endregion
    }

    [Flags]
    public enum MergeProjectOptions
    {
        EqualIdentifiers = 1,
        EqualImportantFields = 1 << 1,
        EqualStaticId = 1 << 2
    }
    public enum MergeReferenceOptions
    {
        Ignore,
        Override,
        Complete,
        CompleteIfEmpty
    }
}
