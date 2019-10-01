using SwissAcademic.Addons.ReferenceEvaluation.Properties;
using SwissAcademic.Citavi.Shell;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceEvaluation
{
    public partial class ReferenceEvaluationDialog : Form
    {
        #region Fields

        MainForm _mainForm;

        #endregion

        #region Constructors

        public ReferenceEvaluationDialog(MainForm mainForm)
        {
            InitializeComponent();
            this.Owner = mainForm;
            this._mainForm = mainForm;
            Localizen();
            Initialize();
        }

        #endregion

        #region Methods

        void Localizen()
        {
            Text = ReferenceEvaluationResources.Form_Text;
            btnClose.Text = ReferenceEvaluationResources.Form_Close;
            btnClipboard.Text = ReferenceEvaluationResources.Form_Clipboard;
            lblResult.Text = ReferenceEvaluationResources.Form_Result;
            lblChoice.Text = ReferenceEvaluationResources.Form_Choice;
            chbShowHeaders.Text = ReferenceEvaluationResources.Form_ShowHeader;
            lblOptions.Text = ReferenceEvaluationResources.Form_Options;
            Icon = _mainForm.Icon;
        }

        void Initialize()
        {
            var evaluators = BaseEvaluator.GetAvailableEvaluators().OrderBy(evaluator => evaluator.Caption).ToList();
            cboFunctions.DataSource = new BindingSource(evaluators, null);
            cboFunctions.DisplayMember = "Caption";
            cboFunctions.Focus();
        }

        #endregion

        #region Eventhandler

        void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void BtnClipboard_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtResult.Text)) return;

            Clipboard.SetText(txtResult.Text);
        }

        void CbFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFunctions.SelectedItem == null) return;

            var evaluator = cboFunctions.SelectedItem as BaseEvaluator;

            if (evaluator == null) return;

            evaluator.ShowHeader = chbShowHeaders.Checked;
            txtResult.Text = evaluator.Run(_mainForm);
        }

        void ChbShowHeaders_CheckedChanged(object sender, EventArgs e)
        {
            CbFunctions_SelectedIndexChanged(sender, e);
        }

        #endregion
    }
}
