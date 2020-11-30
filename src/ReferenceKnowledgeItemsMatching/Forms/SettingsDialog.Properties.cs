namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class SettingsDialog
    {
        public bool RunMatchingForExistingItems { get; private set; }

        public AddonSettings AddonSettings => new AddonSettings
        {
            IsMatchingActivate = chkActivateMatching.Checked,
            MatchCategoriesFromReferenceToKnowledgeItems = chkCategeoriesFromReference.Checked,
            MatchGroupsFromReferenceToKnowledgeItems = chkGroupsFromReference.Checked,
            MatchKeywordsfromReferenceToKnowledgeItems = chkKeywordsFromReference.Checked,
            MatchCategoriesFromKnowledgeItemToReference = chkCategoriesFromKnowledgeItem.Checked,
            MatchGroupsFromKnowledgeItemToReference = chkGroupsFromKnowledgeItem.Checked,
            MatchKeywordsFromKnowledgeItemToReference = chkKeywordsFromKnowledgeItem.Checked
        };
    }
}
