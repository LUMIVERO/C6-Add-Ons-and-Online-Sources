using SwissAcademic.Addons.ReferenceEvaluationAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceEvaluationAddon
{
    public partial class ReferenceEvaluationForm : FormBase
    {
        // Events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var evaluators = BaseEvaluator.GetAvailableEvaluators().OrderBy(evaluator => evaluator.Caption).ToList();
            cboFunctions.DataSource = new BindingSource(evaluators, null);
            cboFunctions.DisplayMember = "Caption";
            cboFunctions.Focus();
        }

        protected override void OnApplicationIdle()
        {
            base.OnApplicationIdle();

            btnClipboard.Enabled = !string.IsNullOrEmpty(txtResult.Text);
        }

        // Constructors

        public ReferenceEvaluationForm(MainForm mainForm) : base(mainForm) => InitializeComponent();

        //  Methods

        public override void Localize()
        {
            base.Localize();

            Text = Resources.Form_Text;
            btnClose.Text = Resources.Form_Close;
            btnClipboard.Text = Resources.Form_Clipboard;
            lblResult.Text = Resources.Form_Result;
            lblChoice.Text = Resources.Form_Choice;
            chbShowHeaders.Text = Resources.Form_ShowHeader;
            lblOptions.Text = Resources.Form_Options;
        }

        // Eventhandler

        void BtnClipboard_Click(object sender, EventArgs e) => Clipboard.SetText(txtResult.Text);

        void CbFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFunctions.SelectedItem is BaseEvaluator evaluator)
            {
                evaluator.ShowHeader = chbShowHeaders.Checked;
                txtResult.Text = evaluator.Run(Owner as MainForm);
            }
        }

        void ChbShowHeaders_CheckedChanged(object sender, EventArgs e) => CbFunctions_SelectedIndexChanged(sender, e);
    }
}
