using SwissAcademic.Addons.BookOrderByEmail.Properties;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.BookOrderByEmail
{
    public partial class ConfigDialog : Form
    {
        #region Constructors

        public ConfigDialog(string receiver, string body)
        {
            InitializeComponent();
            Localize();
            txtBody.Text = body;
            txtReceiver.Text = receiver;
        }

        #endregion

        #region Properties

        public string Receiver => txtReceiver.Text;

        public string Body => txtBody.Text;

        #endregion

        #region Methods

        void Localize()
        {
            Text = BookOrderByEmailResources.ConfigDialog_Text;
            btnOk.Text = BookOrderByEmailResources.ConfigDialog_Ok;
            btnCancel.Text = BookOrderByEmailResources.ConfigDialog_Cancel;
            lblReceiver.Text = BookOrderByEmailResources.ConfigDialog_lbl_Receiver;
            lblBody.Text = BookOrderByEmailResources.ConfigDialog_lbl_Body;
        }

        #endregion

        #region Eventhandlers

        void BtnOk_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

        void BtnCancel_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

        #endregion
    }
}
