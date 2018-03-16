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

        ComboBoxHelperCollection _mergeOptionsCollection = new ComboBoxHelperCollection();

        #endregion

        #region Methods

        #region LocalizeMergeTitleData
        void LocalizeMergeTitleData()
        {
            nextButton.Text = PushAndMergeResources.NextButton;
            
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

            if(!_mergeOptionsCollection.Any())
            {
                _mergeOptionsCollection.Add(MergeReferenceOptions.Complete, PushAndMergeResources.MergeReferenceOptionComplete);
                _mergeOptionsCollection.Add(MergeReferenceOptions.CompleteIfEmpty, PushAndMergeResources.MergeReferenceOptionCompleteIfEmpty);
                _mergeOptionsCollection.Add(MergeReferenceOptions.Ignore, PushAndMergeResources.MergeReferenceOptionIgnore);
                _mergeOptionsCollection.Add(MergeReferenceOptions.Override, PushAndMergeResources.MergeReferenceOptionOverride);

                abstractMergeOptionsTextEditor.ListItems = _mergeOptionsCollection;
                evalutationMergeOptionsTextEditor.ListItems = _mergeOptionsCollection;
                tableOfContentsMergeOptionsTextEditor.ListItems = _mergeOptionsCollection;
                notesMergeOptionsTextEditor.ListItems = _mergeOptionsCollection;

                var comboxHelperCollection = new ComboBoxHelperCollection();
                comboxHelperCollection.Add(true, PushAndMergeResources.IgnoreKnowledgeItemOnMatchText);
                comboxHelperCollection.Add(false, PushAndMergeResources.CloneKnowledgeItemOnMatchText);

                mergeKnowledgeItemsTextEditor.ListItems = comboxHelperCollection;
            }

            abstractMergeOptionsTextEditor.SelectedItem = _pushAndMergeOptions.MergeReferenceOptionAbstract;
            evalutationMergeOptionsTextEditor.SelectedItem = _pushAndMergeOptions.MergeReferenceOptionEvaluation;
            tableOfContentsMergeOptionsTextEditor.SelectedItem = _pushAndMergeOptions.MergeReferenceOptionTableOfContents;
            notesMergeOptionsTextEditor.SelectedItem = _pushAndMergeOptions.MergeReferenceOptionNotes;

            mergeKnowledgeItemsTextEditor.SelectedItem = _pushAndMergeOptions.IgnoreKnowledgeItemOnMatch;

        }
        #endregion

        #region SetMergeTitleData
        void SetMergeTitleData()
        {
            _pushAndMergeOptions.MergeKnowledgeItemKeywords = includeKeywordsCheckbox.Checked;
            _pushAndMergeOptions.MergeKnowldgeItemGroups = includeGroupsCheckbox.Checked;
            _pushAndMergeOptions.MergeKnowledgeItemCategories = includeCategoriesCheckbox.Checked;

            _pushAndMergeOptions.MergeReferenceOptionAbstract = (MergeReferenceOptions)abstractMergeOptionsTextEditor.SelectedItem;
            _pushAndMergeOptions.MergeReferenceOptionEvaluation = (MergeReferenceOptions)evalutationMergeOptionsTextEditor.SelectedItem;
            _pushAndMergeOptions.MergeReferenceOptionTableOfContents = (MergeReferenceOptions)tableOfContentsMergeOptionsTextEditor.SelectedItem;
            _pushAndMergeOptions.MergeReferenceOptionNotes = (MergeReferenceOptions)notesMergeOptionsTextEditor.SelectedItem;

            _pushAndMergeOptions.IgnoreKnowledgeItemOnMatch = (bool)mergeKnowledgeItemsTextEditor.SelectedItem;
        }
        #endregion

        

        #endregion
    }
}
