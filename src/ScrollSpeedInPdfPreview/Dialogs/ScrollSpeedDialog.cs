using SwissAcademic.Addons.ScrollSpeedInPdfPreview.Properties;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ScrollSpeedInPdfPreview
{
    public partial class ScrollSpeedDialog : FormBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tbSpeed.Value = _scrollSpeed;
            chkOnllyInPreviewMode.Checked = _onlyInFullScreenMode;
        }

        public override void Localize()
        {
            base.Localize();

            btnCancel.Text = Resources.Button_Cancel;
            btnOk.Text = Resources.Button_OK;
            btnReset.Text = Resources.Button_Reset;
            Text = Resources.Dialog_Text;
            chkOnllyInPreviewMode.Text = Resources.CheckBox_Text;
        }

        protected override void OnApplicationIdle()
        {
            base.OnApplicationIdle();

            lblSpeed.Text = Resources.Label_Speed.FormatString(tbSpeed.Value / 100.0);
        }

        readonly int _scrollSpeed;
        readonly bool _onlyInFullScreenMode;

        public ScrollSpeedDialog(Form owner, double scrollSpeed, bool onlyInFullScreenMode) : base(owner)
        {
            InitializeComponent();
            Owner = owner;
            _scrollSpeed = Convert.ToInt32(scrollSpeed * 100);
            _onlyInFullScreenMode = onlyInFullScreenMode;
        }

        public double ScrollSpeed => tbSpeed.Value / 100.0;
        public bool OnlyInFullScreenMode => chkOnllyInPreviewMode.Checked;

        void BtnReset_Click(object sender, EventArgs e) => tbSpeed.Value = 100;
    }
}
