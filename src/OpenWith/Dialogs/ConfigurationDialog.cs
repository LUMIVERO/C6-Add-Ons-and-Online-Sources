using System;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.OpenWith
{
    public partial class ConfigurationDialog : Form
    {
        #region Fields

        Configuration _configuration;

        #endregion

        #region Constructors

        public ConfigurationDialog(Configuration configuration)
        {
            _configuration = configuration;

            InitializeComponent();
            InitializeConfiguration();
            Localize();
            lbApps.SelectedItem = null;
        }

        #endregion

        #region Properties

        public Configuration Configuration => _configuration;

        #endregion

        #region Eventhandlers

        void BtnOk_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

        void BtnCancel_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

        void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new ApplicationDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _configuration.Applications.Add(dialog.Application);
                    InitializeConfiguration();
                }
            }
        }

        void BtnEdit_Click(object sender, EventArgs e)
        {
            if (lbApps.SelectedItem is Application application)
            {
                using (var dialog = new ApplicationDialog(application))
                {
                    dialog.ShowDialog(this);
                    InitializeConfiguration();
                }
            }
        }

        void BtnRemove_Click(object sender, EventArgs e)
        {
            if (lbApps.SelectedItem is Application application)
            {
                lbApps.Items.Remove(application);
                _configuration.Applications.Remove(application);
                lbApps.Refresh();
            }
        }

        void LbApps_SelectedValueChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = lbApps.SelectedItem != null && lbApps.SelectedItem is Application;
            btnRemove.Enabled = lbApps.SelectedItem != null && lbApps.SelectedItem is Application;
        }

        #endregion

        #region Methods

        void Localize()
        {
            Text = Properties.OpenWithResources.ConfigurationDialog_Text;
            btnCancel.Text = Properties.OpenWithResources.Dialog_Cancel;
            btnOk.Text = Properties.OpenWithResources.Dialog_Ok;
            btnAdd.Text = Properties.OpenWithResources.Dialog_Add;
            btnEdit.Text = Properties.OpenWithResources.Dialog_Edit;
            btnRemove.Text = Properties.OpenWithResources.Dialog_Remove;
        }

        void InitializeConfiguration()
        {
            lbApps.Items.Clear();
            lbApps.Items.AddRange(_configuration.Applications.ToArray());
            btnEdit.Enabled = false;
            btnRemove.Enabled = false;
            btnAdd.Enabled = true;
            lbApps.SelectedItem = _configuration.Applications.FirstOrDefault();
        }

        #endregion
    }
}
