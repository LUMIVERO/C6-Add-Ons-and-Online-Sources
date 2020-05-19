using SwissAcademic.Addons.ReferenceEvaluationAddon.Properties;
using SwissAcademic.Citavi.Shell;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceEvaluationAddon
{
    public partial class ReferenceEvaluationDialog : Form
    {
        #region Fields

        readonly MainForm _mainForm;

        #endregion

        #region Constructors

        public ReferenceEvaluationDialog(MainForm mainForm)
        {
            InitializeComponent();
            Owner = mainForm;
            _mainForm = mainForm;
            Localizen();
            Initialize();
        }

        #endregion

        #region Methods

        void Localizen()
        {
            Text = Resources.Form_Text;
            btnClose.Text = Resources.Form_Close;
            btnClipboard.Text = Resources.Form_Clipboard;
            lblResult.Text = Resources.Form_Result;
            lblChoice.Text = Resources.Form_Choice;
            chbShowHeaders.Text = Resources.Form_ShowHeader;
            lblOptions.Text = Resources.Form_Options;
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

        void BtnClose_Click(object sender, EventArgs e) => Close();

        void BtnClipboard_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtResult.Text))
            {
                Clipboard.SetText(txtResult.Text);
            }
        }

        void CbFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFunctions.SelectedItem is BaseEvaluator evaluator)
            {
                evaluator.ShowHeader = chbShowHeaders.Checked;
                txtResult.Text = evaluator.Run(_mainForm);
            }
        }

        void ChbShowHeaders_CheckedChanged(object sender, EventArgs e) => CbFunctions_SelectedIndexChanged(sender, e);

        #endregion
    }
}
