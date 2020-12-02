using SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon.Properties;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class SettingsDialog
    {
        protected override void OnApplicationIdle()
        {
            base.OnApplicationIdle();

            pnlSettings.Visible = chkActivateMatching.Checked;
            btnOk.Enabled = AreSettingsValid();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            chkActivateMatching.Checked = _addonSettings.IsMatchingActivate;
            chkCategeoriesFromReference.Checked = _addonSettings.MatchCategoriesFromReferenceToKnowledgeItems;
            chkCategoriesFromKnowledgeItem.Checked = _addonSettings.MatchCategoriesFromKnowledgeItemToReference;
            chkGroupsFromKnowledgeItem.Checked = _addonSettings.MatchGroupsFromKnowledgeItemToReference;
            chkGroupsFromReference.Checked = _addonSettings.MatchGroupsFromReferenceToKnowledgeItems;
            chkKeywordsFromKnowledgeItem.Checked = _addonSettings.MatchKeywordsFromKnowledgeItemToReference;
            chkKeywordsFromReference.Checked = _addonSettings.MatchKeywordsfromReferenceToKnowledgeItems;
        }

        public override void Localize()
        {
            base.Localize();

            Text = Resources.SettingsDialog_Text;
            btnCancel.Text = Resources.SettingsDialog_Cancel;
            btnOk.Text = Resources.SettingsDialog_Ok;
            llHelp.Text = Resources.SettingsDialog_Help;

            chkActivateMatching.Text = Resources.SettingsDialog_ActivateMatching;
            
            chkCategeoriesFromReference.Text = Resources.SettingsDialog_Categories;
            chkKeywordsFromReference.Text = Resources.SettingsDialog_Keywords_NotRecommended;
            chkGroupsFromReference.Text = Resources.SettingsDialog_Groups;

            chkCategoriesFromKnowledgeItem.Text = Resources.SettingsDialog_Categories;
            chkGroupsFromKnowledgeItem.Text = Resources.SettingsDialog_Groups;
            chkKeywordsFromKnowledgeItem.Text = Resources.SettingsDialog_Keywords;

            lblReferenceToKnowledgeItemsDescription.Text = Resources.SettingsDialog_DescriptionFromReferenceToKnowledgeItems;
            lblKnowledgeItemsToreferenceDescription.Text = Resources.SettingsDialog_DescriptionFromKnowdlegeItemToReference;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.OK && chkActivateMatching.Checked)
            {
                RunMatchingForExistingItems = MessageBox.Show(this, Resources.MessageBox_MatchingForExistingItems, Owner.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            }

            base.OnFormClosed(e);
        }
    }
}
