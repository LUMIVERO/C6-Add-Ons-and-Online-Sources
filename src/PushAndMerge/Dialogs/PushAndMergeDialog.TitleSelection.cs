using SwissAcademic.Addons.PushAndMerge.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
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

        ComboBoxHelperCollection _titleSelection = new ComboBoxHelperCollection();

        #endregion

        #region Methods

        #region LocalizeTitleSelection
        void LocalizeTitleSelection()
        {
            selectTitleLabel.Text = PushAndMergeResources.TitleSelectionTabTitle;
            selectTitlesSettingsTitleLabel.Text = PushAndMergeResources.TitleSelectionTabSettingsLabel;
            selectTitlesMergeSubtitleLabel.Text = PushAndMergeResources.TitleSelectionMergeSubtitleLabel;
            mergeSameIdsCheckbox.Text = PushAndMergeResources.mergeSameIdCheckboxText;
            mergeEssentialFieldsCheckBox.Text = PushAndMergeResources.mergeEssentialFieldsCheckboxText;
            mergeStaticIdCheckbox.Text = PushAndMergeResources.mergeStaticIdCheckboxText;

            allOtherTitlesSubtitleLabel.Text = PushAndMergeResources.allOtherTitlesSubtitleLabel;
            ignoreOtherTitlesRadioButton.Text = PushAndMergeResources.ignoreOtherTitlesCheckboxText;
            copyOtherTitlesRadioButton.Text = PushAndMergeResources.copyOtherTitlesCheckboxText;

            nextButton.Text = PushAndMergeResources.NextButton;
        }
        #endregion

        #region InitTitleSelection
        void InitTitleSelection(Form dialogOwner, Project sourceProject)
        {
            backButton.Visible = false;

            mergeSameIdsCheckbox.Checked = _pushAndMergeOptions.MergeProjectOptions.HasFlag(MergeProjectOptions.EqualIdentifiers);
            mergeEssentialFieldsCheckBox.Checked = _pushAndMergeOptions.MergeProjectOptions.HasFlag(MergeProjectOptions.EqualEssentialFields);
            mergeStaticIdCheckbox.Checked = _pushAndMergeOptions.MergeProjectOptions.HasFlag(MergeProjectOptions.EqualStaticId);

            if(_pushAndMergeOptions.CopyAllNonMatchedReferences)
            {
                copyOtherTitlesRadioButton.Checked = true;
            }
            else
            {
                ignoreOtherTitlesRadioButton.Checked = true;
            }

            var referenceSelectionSupporter = dialogOwner as ISupportReferenceSelection;
            var selectedReferences = referenceSelectionSupporter == null || referenceSelectionSupporter.IsAllSelected ? null : referenceSelectionSupporter.GetSelectedReferences();
            var filteredReferences = referenceSelectionSupporter == null || !referenceSelectionSupporter.HasFilter ? null : referenceSelectionSupporter.GetFilteredReferences();

            #region Selection


            if (!_titleSelection.Any())
            {
                #region SelectedItems

                if (selectedReferences != null && selectedReferences.Any())
                {
                    _titleSelection.Add(ReferenceSelection.Selected, PushAndMergeResources.ReferenceSelectionSelected_Smart.FormatSmart(selectedReferences.Count()));
                }

                #endregion

                #region FilteredReferences

                if (filteredReferences != null && filteredReferences.Any())
                {
                    var entry = PushAndMergeResources.ReferenceSelectionFiltered_Smart.FormatSmart(filteredReferences.Count());
                    _titleSelection.Add(ReferenceSelection.Filter, entry);
                }

                #endregion

                _titleSelection.Add(ReferenceSelection.All, PushAndMergeResources.ReferenceSelectionAll_Smart.FormatSmart(sourceProject.References.Count()));
                selectTitleTextEditor.ListItems = _titleSelection;
            }

            switch (_pushAndMergeOptions.ReferenceSelection)
            {
                case ReferenceSelection.Filter:
                    {
                        if (_titleSelection.Any(item => item.Value.Equals(ReferenceSelection.Filter))) selectTitleTextEditor.SelectedItem = ReferenceSelection.Filter;
                        else selectTitleTextEditor.SelectedItem = ReferenceSelection.All;
                    }
                    break;

                case ReferenceSelection.Selected:
                    {
                        if (_titleSelection.Any(item => item.Value.Equals(ReferenceSelection.Selected))) selectTitleTextEditor.SelectedItem = ReferenceSelection.Selected;
                        else if (_titleSelection.Any(item => item.Value.Equals(ReferenceSelection.Filter))) selectTitleTextEditor.SelectedItem = ReferenceSelection.Filter;
                        else selectTitleTextEditor.SelectedItem = ReferenceSelection.All;
                    }
                    break;

                case ReferenceSelection.All:
                default:
                    selectTitleTextEditor.SelectedItem = ReferenceSelection.All;
                    break;
            }

            #endregion
        }
        #endregion

        #region SetTitleSelectionData
        void SetTitleSelectionData()
        {
            if(mergeSameIdsCheckbox.Checked)
            {
                _pushAndMergeOptions.MergeProjectOptions |= MergeProjectOptions.EqualIdentifiers;
            }
            if(mergeEssentialFieldsCheckBox.Checked)
            {
                _pushAndMergeOptions.MergeProjectOptions |= MergeProjectOptions.EqualEssentialFields;
            }
            if(mergeStaticIdCheckbox.Checked)
            {
                _pushAndMergeOptions.MergeProjectOptions |= MergeProjectOptions.EqualStaticId;
            }
            _pushAndMergeOptions.CopyAllNonMatchedReferences = copyOtherTitlesRadioButton.Checked;
            _pushAndMergeOptions.ReferenceSelection = (ReferenceSelection)selectTitleTextEditor.SelectedItem;    
        }
        #endregion

        #endregion
    }
}
