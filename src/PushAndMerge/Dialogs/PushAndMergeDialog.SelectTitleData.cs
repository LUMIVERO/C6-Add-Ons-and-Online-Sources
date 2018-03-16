using Infragistics.Win.UltraWinTree;
using SwissAcademic.Addons.PushAndMerge.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissAcademic.Addons.PushAndMerge
{
    public partial class PushAndMergeDialog
    {
        #region Fields

        bool _eventsSuspended;

        #endregion

        #region Methods

        #region LocalizeSelectTitleData
        void LocalizeSelectTitleData(Project sourceProject)
        {
            nextButton.Text = PushAndMergeResources.NextButton;

            tasksCheckbox.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Tasks.ToString());
            keywordCheckbox.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Keywords.ToString());
            categoriesCheckbox.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Categories.ToString());
            groupsCheckbox.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Groups.ToString());
            abstractCheckbox.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Abstract.ToString());
            tableOfContentsCheckbox.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.TableOfContents.ToString());
            evaluationCheckbox.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Evaluation.ToString());
            notesCheckbox.Text = ResourceHelper.GetPropertyName("Reference", ReferencePropertyId.Notes.ToString());

            customTextsCheckboxTreeView.GetNodeByKey("CustomFields").Text = PushAndMergeResources.CustomFields;
            customTextsCheckboxTreeView.GetNodeByKey("CustomField1").Text = ReferencePropertyDescriptor.CustomField1.GetNameLocalized(sourceProject);
            customTextsCheckboxTreeView.GetNodeByKey("CustomField2").Text = ReferencePropertyDescriptor.CustomField2.GetNameLocalized(sourceProject);
            customTextsCheckboxTreeView.GetNodeByKey("CustomField3").Text = ReferencePropertyDescriptor.CustomField3.GetNameLocalized(sourceProject);
            customTextsCheckboxTreeView.GetNodeByKey("CustomField4").Text = ReferencePropertyDescriptor.CustomField4.GetNameLocalized(sourceProject);
            customTextsCheckboxTreeView.GetNodeByKey("CustomField5").Text = ReferencePropertyDescriptor.CustomField5.GetNameLocalized(sourceProject);
            customTextsCheckboxTreeView.GetNodeByKey("CustomField6").Text = ReferencePropertyDescriptor.CustomField6.GetNameLocalized(sourceProject);
            customTextsCheckboxTreeView.GetNodeByKey("CustomField7").Text = ReferencePropertyDescriptor.CustomField7.GetNameLocalized(sourceProject);
            customTextsCheckboxTreeView.GetNodeByKey("CustomField8").Text = ReferencePropertyDescriptor.CustomField8.GetNameLocalized(sourceProject);
            customTextsCheckboxTreeView.GetNodeByKey("CustomField9").Text = ReferencePropertyDescriptor.CustomField9.GetNameLocalized(sourceProject);
        }
        #endregion

        #region InitTitleSelection
        void InitSelectTitleData()
        {
            backButton.Visible = true;

            tasksCheckbox.Checked = _pushAndMergeOptions.IncludeTasks;
            keywordCheckbox.Checked = _pushAndMergeOptions.IncludeKeywords;
            categoriesCheckbox.Checked = _pushAndMergeOptions.IncludeCategories;
            groupsCheckbox.Checked = _pushAndMergeOptions.IncludeGroups;
            abstractCheckbox.Checked = _pushAndMergeOptions.IncludeAbstract;
            tableOfContentsCheckbox.Checked = _pushAndMergeOptions.IncludeTableOfContents;
            evaluationCheckbox.Checked = _pushAndMergeOptions.IncludeEvaluation;
            notesCheckbox.Checked = _pushAndMergeOptions.IncludeNotes;

            customTextsCheckboxTreeView.GetNodeByKey("CustomField1").CheckedState = _pushAndMergeOptions.IncludeCustomField1 ? CheckState.Checked : CheckState.Unchecked;
            customTextsCheckboxTreeView.GetNodeByKey("CustomField2").CheckedState = _pushAndMergeOptions.IncludeCustomField2 ? CheckState.Checked : CheckState.Unchecked;
            customTextsCheckboxTreeView.GetNodeByKey("CustomField3").CheckedState = _pushAndMergeOptions.IncludeCustomField3 ? CheckState.Checked : CheckState.Unchecked;
            customTextsCheckboxTreeView.GetNodeByKey("CustomField4").CheckedState = _pushAndMergeOptions.IncludeCustomField4 ? CheckState.Checked : CheckState.Unchecked;
            customTextsCheckboxTreeView.GetNodeByKey("CustomField5").CheckedState = _pushAndMergeOptions.IncludeCustomField5 ? CheckState.Checked : CheckState.Unchecked;
            customTextsCheckboxTreeView.GetNodeByKey("CustomField6").CheckedState = _pushAndMergeOptions.IncludeCustomField6 ? CheckState.Checked : CheckState.Unchecked;
            customTextsCheckboxTreeView.GetNodeByKey("CustomField7").CheckedState = _pushAndMergeOptions.IncludeCustomField7 ? CheckState.Checked : CheckState.Unchecked;
            customTextsCheckboxTreeView.GetNodeByKey("CustomField8").CheckedState = _pushAndMergeOptions.IncludeCustomField8 ? CheckState.Checked : CheckState.Unchecked;
            customTextsCheckboxTreeView.GetNodeByKey("CustomField9").CheckedState = _pushAndMergeOptions.IncludeCustomField9 ? CheckState.Checked : CheckState.Unchecked;
        }
        #endregion

        #region UpdateCustomFieldsNode

        void UpdateCustomFieldsNode()
        {
            var checkedState = customTextsCheckboxTreeView.GetNodeByKey("CustomField1").CheckedState;
            var indeterminate =
                customTextsCheckboxTreeView.GetNodeByKey("CustomField2").CheckedState != checkedState ||
                customTextsCheckboxTreeView.GetNodeByKey("CustomField3").CheckedState != checkedState ||
                customTextsCheckboxTreeView.GetNodeByKey("CustomField4").CheckedState != checkedState ||
                customTextsCheckboxTreeView.GetNodeByKey("CustomField5").CheckedState != checkedState ||
                customTextsCheckboxTreeView.GetNodeByKey("CustomField6").CheckedState != checkedState ||
                customTextsCheckboxTreeView.GetNodeByKey("CustomField7").CheckedState != checkedState ||
                customTextsCheckboxTreeView.GetNodeByKey("CustomField8").CheckedState != checkedState ||
                customTextsCheckboxTreeView.GetNodeByKey("CustomField9").CheckedState != checkedState;

            var eventsSuspended = _eventsSuspended;
            try
            {
                _eventsSuspended = true;

                if (indeterminate) customTextsCheckboxTreeView.Nodes["CustomFields"].CheckedState = CheckState.Indeterminate;
                else customTextsCheckboxTreeView.Nodes["CustomFields"].CheckedState = customTextsCheckboxTreeView.GetNodeByKey("CustomField1").CheckedState;
            }
            finally
            {
                _eventsSuspended = false;
            }
        }

        #endregion

        #region SetSelectTitleData

        void SetSelectTitleData()
        {
            _pushAndMergeOptions.IncludeTasks = tasksCheckbox.Checked;
            _pushAndMergeOptions.IncludeKeywords = keywordCheckbox.Checked;
            _pushAndMergeOptions.IncludeCategories = categoriesCheckbox.Checked;
            _pushAndMergeOptions.IncludeGroups = groupsCheckbox.Checked;
            _pushAndMergeOptions.IncludeAbstract = abstractCheckbox.Checked;
            _pushAndMergeOptions.IncludeTableOfContents = tableOfContentsCheckbox.Checked;
            _pushAndMergeOptions.IncludeEvaluation = evaluationCheckbox.Checked;
            _pushAndMergeOptions.IncludeNotes = notesCheckbox.Checked;

            _pushAndMergeOptions.IncludeCustomField1 = customTextsCheckboxTreeView.GetNodeByKey("CustomField1").CheckedState == CheckState.Checked;
            _pushAndMergeOptions.IncludeCustomField2 = customTextsCheckboxTreeView.GetNodeByKey("CustomField2").CheckedState == CheckState.Checked;
            _pushAndMergeOptions.IncludeCustomField3 = customTextsCheckboxTreeView.GetNodeByKey("CustomField3").CheckedState == CheckState.Checked;
            _pushAndMergeOptions.IncludeCustomField4 = customTextsCheckboxTreeView.GetNodeByKey("CustomField4").CheckedState == CheckState.Checked;
            _pushAndMergeOptions.IncludeCustomField5 = customTextsCheckboxTreeView.GetNodeByKey("CustomField5").CheckedState == CheckState.Checked;
            _pushAndMergeOptions.IncludeCustomField6 = customTextsCheckboxTreeView.GetNodeByKey("CustomField6").CheckedState == CheckState.Checked;
            _pushAndMergeOptions.IncludeCustomField7 = customTextsCheckboxTreeView.GetNodeByKey("CustomField7").CheckedState == CheckState.Checked;
            _pushAndMergeOptions.IncludeCustomField8 = customTextsCheckboxTreeView.GetNodeByKey("CustomField8").CheckedState == CheckState.Checked;
            _pushAndMergeOptions.IncludeCustomField9 = customTextsCheckboxTreeView.GetNodeByKey("CustomField9").CheckedState == CheckState.Checked;
        }

        #endregion

        #endregion

        #region EventHandlers

        void CustomTextsCheckboxTreeViewAfterCheck(object sender, NodeEventArgs e)
        {
            if (_eventsSuspended) return;

            if (e.TreeNode.Key.Equals("CustomFields", StringComparison.Ordinal))
            {
                var eventsSuspended = _eventsSuspended;

                try
                {
                    _eventsSuspended = true;

                    switch (e.TreeNode.CheckedState)
                    {
                        case CheckState.Checked:
                            {
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField1").CheckedState = CheckState.Checked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField2").CheckedState = CheckState.Checked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField3").CheckedState = CheckState.Checked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField4").CheckedState = CheckState.Checked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField5").CheckedState = CheckState.Checked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField6").CheckedState = CheckState.Checked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField7").CheckedState = CheckState.Checked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField8").CheckedState = CheckState.Checked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField9").CheckedState = CheckState.Checked;
                            }
                            break;

                        case CheckState.Unchecked:
                        case CheckState.Indeterminate:
                            {
                                e.TreeNode.CheckedState = CheckState.Unchecked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField1").CheckedState = CheckState.Unchecked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField2").CheckedState = CheckState.Unchecked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField3").CheckedState = CheckState.Unchecked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField4").CheckedState = CheckState.Unchecked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField5").CheckedState = CheckState.Unchecked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField6").CheckedState = CheckState.Unchecked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField7").CheckedState = CheckState.Unchecked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField8").CheckedState = CheckState.Unchecked;
                                customTextsCheckboxTreeView.GetNodeByKey("CustomField9").CheckedState = CheckState.Unchecked;
                            }
                            break;
                    }
                }
                finally
                {
                    _eventsSuspended = eventsSuspended;
                }
            }

            else if (e.TreeNode.Key.StartsWith("CustomField"))
            {
                UpdateCustomFieldsNode();
            }
        }

        #endregion
    }
}
