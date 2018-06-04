using SwissAcademic.Addons.PushAndMerge.Properties;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Controls;
using SwissAcademic.Resources;
using System.Linq;

namespace SwissAcademic.Addons.PushAndMerge
{
    public partial class PushAndMergeDialog
    {
        #region Fields

        ComboBoxHelperCollection _mergeReferenceContentOptionsCollection = new ComboBoxHelperCollection();
        ComboBoxHelperCollection _mergeReferenceOptionsCollection = new ComboBoxHelperCollection();

        #endregion

        #region Methods

        #region LocalizeMergeTitleData
        void LocalizeMergeTitleData()
        {
            nextButton.Text = PushAndMergeResources.NextButton;

            mergeTitlesTabTitleLabel.Text = PushAndMergeResources.MergeTitlesTabTitle;
            knowledgeItemsLabel.Text = PushAndMergeResources.KnowledgeItemsLabel;
            includeKeywordsCheckbox.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Keywords.ToString());
            includeGroupsCheckbox.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Groups.ToString());
            includeCategoriesCheckbox.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Categories.ToString());

            abstractLabel.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Abstract.ToString());
            tableOfContentsLabel.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.TableOfContents.ToString());
            evaluationLabel.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Evaluation.ToString());
            noteLabel.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Notes.ToString());
        }
        #endregion

        #region InitTitleSelection
        void InitMergeTitleData()
        {
            backButton.Visible = true;

            includeKeywordsCheckbox.Checked = _pushAndMergeOptions.MergeKnowledgeItemKeywords;
            includeGroupsCheckbox.Checked = _pushAndMergeOptions.MergeKnowldgeItemGroups;
            includeCategoriesCheckbox.Checked = _pushAndMergeOptions.MergeKnowledgeItemCategories;

            if(!_mergeReferenceContentOptionsCollection.Any())
            {
                _mergeReferenceContentOptionsCollection.Add(MergeReferenceContentOptions.Complete, PushAndMergeResources.MergeReferenceOptionComplete);
                _mergeReferenceContentOptionsCollection.Add(MergeReferenceContentOptions.CompleteIfEmpty, PushAndMergeResources.MergeReferenceOptionCompleteIfEmpty);
                _mergeReferenceContentOptionsCollection.Add(MergeReferenceContentOptions.Ignore, PushAndMergeResources.MergeReferenceOptionIgnore);
                _mergeReferenceContentOptionsCollection.Add(MergeReferenceContentOptions.Override, PushAndMergeResources.MergeReferenceOptionOverride);
                _mergeReferenceContentOptionsCollection.Add(MergeReferenceContentOptions.CompleIfNotEqual, PushAndMergeResources.MergeReferenceOptionsCompleteIfNotEqual);

                abstractMergeOptionsTextEditor.ListItems = _mergeReferenceContentOptionsCollection;
                evalutationMergeOptionsTextEditor.ListItems = _mergeReferenceContentOptionsCollection;
                tableOfContentsMergeOptionsTextEditor.ListItems = _mergeReferenceContentOptionsCollection;
                notesMergeOptionsTextEditor.ListItems = _mergeReferenceContentOptionsCollection;

                var comboxHelperCollection = new ComboBoxHelperCollection();
                comboxHelperCollection.Add(true, PushAndMergeResources.IgnoreKnowledgeItemOnMatchText);
                comboxHelperCollection.Add(false, PushAndMergeResources.CloneKnowledgeItemOnMatchText);

                mergeKnowledgeItemsTextEditor.ListItems = comboxHelperCollection;

                _mergeReferenceOptionsCollection.Add(MergeReferenceOptions.Ignore, PushAndMergeResources.MergeReferenceOptionIgnore);
                _mergeReferenceOptionsCollection.Add(MergeReferenceOptions.Merge, PushAndMergeResources.MergeReferenceOptionComplete);
                _mergeReferenceOptionsCollection.Add(MergeReferenceOptions.Replace, PushAndMergeResources.MergeReferenceOptionsReplace);

                keywordMergeOptionsEditor.ListItems = _mergeReferenceOptionsCollection;
                categoryMergeOptionsEditor.ListItems = _mergeReferenceOptionsCollection;
                groupsMergeOptionsEditor.ListItems = _mergeReferenceOptionsCollection;
            }

            abstractMergeOptionsTextEditor.SelectedItem = _pushAndMergeOptions.MergeReferenceOptionAbstract;
            evalutationMergeOptionsTextEditor.SelectedItem = _pushAndMergeOptions.MergeReferenceOptionEvaluation;
            tableOfContentsMergeOptionsTextEditor.SelectedItem = _pushAndMergeOptions.MergeReferenceOptionTableOfContents;
            notesMergeOptionsTextEditor.SelectedItem = _pushAndMergeOptions.MergeReferenceOptionNotes;

            mergeKnowledgeItemsTextEditor.SelectedItem = _pushAndMergeOptions.IgnoreKnowledgeItemOnMatch;

            keywordMergeOptionsEditor.SelectedItem = _pushAndMergeOptions.MergeReferenceOptionsKeywords;
            categoryMergeOptionsEditor.SelectedItem = _pushAndMergeOptions.MergeReferenceOptionsCategories;
            groupsMergeOptionsEditor.SelectedItem = _pushAndMergeOptions.MergeReferenceOptionsGroups;

        }
        #endregion

        #region SetMergeTitleData
        void SetMergeTitleData()
        {
            _pushAndMergeOptions.MergeKnowledgeItemKeywords = includeKeywordsCheckbox.Checked;
            _pushAndMergeOptions.MergeKnowldgeItemGroups = includeGroupsCheckbox.Checked;
            _pushAndMergeOptions.MergeKnowledgeItemCategories = includeCategoriesCheckbox.Checked;

            _pushAndMergeOptions.MergeReferenceOptionAbstract = (MergeReferenceContentOptions)abstractMergeOptionsTextEditor.SelectedItem;
            _pushAndMergeOptions.MergeReferenceOptionEvaluation = (MergeReferenceContentOptions)evalutationMergeOptionsTextEditor.SelectedItem;
            _pushAndMergeOptions.MergeReferenceOptionTableOfContents = (MergeReferenceContentOptions)tableOfContentsMergeOptionsTextEditor.SelectedItem;
            _pushAndMergeOptions.MergeReferenceOptionNotes = (MergeReferenceContentOptions)notesMergeOptionsTextEditor.SelectedItem;

            _pushAndMergeOptions.MergeReferenceOptionsCategories = (MergeReferenceOptions)categoryMergeOptionsEditor.SelectedItem;
            _pushAndMergeOptions.MergeReferenceOptionsKeywords = (MergeReferenceOptions)keywordMergeOptionsEditor.SelectedItem;
            _pushAndMergeOptions.MergeReferenceOptionsGroups = (MergeReferenceOptions)groupsMergeOptionsEditor.SelectedItem;

            _pushAndMergeOptions.IgnoreKnowledgeItemOnMatch = (bool)mergeKnowledgeItemsTextEditor.SelectedItem;
        }
        #endregion

        

        #endregion
    }
}
