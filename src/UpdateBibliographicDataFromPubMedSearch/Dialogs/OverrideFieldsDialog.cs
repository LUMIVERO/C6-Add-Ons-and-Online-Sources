using SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearchAddon.Properties;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearchAddon
{
    public partial class OverrideFieldsDialog : FormBase
    {
        #region Constructors

        public OverrideFieldsDialog(Form owner) : base(owner) => InitializeComponent();

        #endregion

        #region Methods

        public override void Localize()
        {
            base.Localize();

            chbOverrideAbstract.Text = Resources.OverrideAbstract;
            chbOverrideKeywords.Text = Resources.OverrideKeywords;
            chbOverrideTOC.Text = Resources.OverrideToc;
            chbRemoveNotes.Text = Resources.ClearNotes;
            btnCancel.Text = Resources.Cancel;
            btnOk.Text = Resources.Ok;
        }

        #endregion

        #region Properties

        public MacroSettings Settings => new MacroSettings
        {
            ClearNotes = chbRemoveNotes.Checked,
            OverwriteAbstract = chbOverrideAbstract.Checked,
            OverwriteKeywords = chbOverrideKeywords.Checked,
            OverwriteTableOfContents = chbOverrideTOC.Checked
        };

        #endregion
    }
}
