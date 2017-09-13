using SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch.Properties;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch
{
    public partial class OverrideFieldsDialog : Form
    {
        #region Constructors

        public OverrideFieldsDialog()
        {
            InitializeComponent();

            chbOverrideAbstract.Text = UpdateBibliographicDataFromPubMedSearchResources.OverrideAbstract;
            chbOverrideKeywords.Text = UpdateBibliographicDataFromPubMedSearchResources.OverrideKeywords;
            chbOverrideTOC.Text = UpdateBibliographicDataFromPubMedSearchResources.OverrideToc;
            chbRemoveNotes.Text = UpdateBibliographicDataFromPubMedSearchResources.ClearNotes;
            btnCancel.Text = UpdateBibliographicDataFromPubMedSearchResources.Cancel;
            btnOk.Text = UpdateBibliographicDataFromPubMedSearchResources.Ok;
        }

        #endregion

        #region Properties

        public MacroSettings Settings
        {
            get { return new MacroSettings { ClearNotes = chbRemoveNotes.Checked, OverwriteAbstract = chbOverrideAbstract.Checked, OverwriteKeywords = chbOverrideKeywords.Checked, OverwriteTableOfContents = chbOverrideTOC.Checked }; }
        }

        #endregion

        #region Eventhandlers

        void BtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #endregion
    }
}
