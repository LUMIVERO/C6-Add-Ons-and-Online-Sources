using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.OpenWith
{
    public partial class ApplicationDialog : Form
    {
        #region Fields

        Application _application;
        bool _isNew;

        #endregion

        #region Constructors

        public ApplicationDialog()
        {
            InitializeComponent();
            _isNew = true;
            _application = new Application();
            Localize();
            InitializeApplication();
        }

        public ApplicationDialog(Application application)
        {
            InitializeComponent();
            _application = application;
            _isNew = false;
            Localize();
            InitializeApplication();
        }

        #endregion

        #region Properties

        public Application Application
        {
            get
            {
                return _application;
            }
        }

        #endregion

        #region Methods

        void InitializeApplication()
        {
            txtName.Text = _application.Name;
            txtPath.Text = _application.Path;
            txtArgument.Text = _application.Argument;
            txtFilter.Text = string.Join(" | ", _application.Filters);
            toolTip.SetToolTip(txtPath, _application.Path);
        }

        void Localize()
        {
            Text = Properties.OpenWithResources.ApplicationDialog_Text;

            if (_isNew) btnAdd.Text = Properties.OpenWithResources.Dialog_Add;
            else btnAdd.Text = Properties.OpenWithResources.Dialog_Ok;

            btnCancel.Text = Properties.OpenWithResources.Dialog_Cancel;
            lbArgument.Text = Properties.OpenWithResources.ApplicationDialog_LB_Argument;
            lblFilter.Text = Properties.OpenWithResources.ApplicationDialog_LB_Filter;
            lblName.Text = Properties.OpenWithResources.ApplicationDialog_LB_Name;
            lblPath.Text = Properties.OpenWithResources.ApplicationDialog_LB_Path;
            toolTip.SetToolTip(txtPath, null);
            toolTip.SetToolTip(txtArgument, Properties.OpenWithResources.ApplicationDialog_Tooltip_Arguments);
            toolTip.SetToolTip(txtFilter, Properties.OpenWithResources.ApplicationDialog_Tooltip_Filter);
        }

        #endregion

        #region Eventhandlers

        void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show(this, Properties.OpenWithResources.ApplicationDialog_Message_AddName, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtPath.Text))
            {
                MessageBox.Show(this, Properties.OpenWithResources.ApplicationDialog_Message_AddPath, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!txtArgument.Text.Contains("%1"))
            {
                MessageBox.Show(this, Properties.OpenWithResources.ApplicationDialog_Message_ValidArgument, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _application.Name = txtName.Text;
            _application.Path = txtPath.Text;
            _application.Filters = txtFilter.Text.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();
            _application.Argument = txtArgument.Text;

            DialogResult = DialogResult.OK;
        }

        void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        void BtnAddPath_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "EXE (*.exe)|*.exe";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtPath.Text = dialog.FileName;
                    toolTip.SetToolTip(txtPath, dialog.FileName);

                    if (string.IsNullOrEmpty(txtName.Text)) txtName.Text = FileVersionInfo.GetVersionInfo(dialog.FileName)?.ProductName;
                }
            }
        }

        void BtnAddDataTypes_Click(object sender, EventArgs e)
        {
            using (var dialog = new DataTypesDialog(txtFilter.Text.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList()) { Owner = this })
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtFilter.Text = string.Join(" | ", dialog.SelectedDataTypes);
                }
            }
        }

        #endregion
    }
}
