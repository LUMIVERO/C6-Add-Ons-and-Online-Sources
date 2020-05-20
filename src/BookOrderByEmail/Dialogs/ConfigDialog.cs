using SwissAcademic.Addons.BookOrderByEmailAddon.Properties;
using SwissAcademic.Controls;

namespace SwissAcademic.Addons.BookOrderByEmailAddon
{
    public partial class ConfigDialog : FormBase
    {
        #region Constructors

        ConfigDialog() => InitializeComponent();

        public ConfigDialog(string receiver, string body) : this()
        {
            txtBody.Text = body;
            txtReceiver.Text = receiver;
        }

        #endregion

        #region Properties

        public string Receiver => txtReceiver.Text;

        public string Body => txtBody.Text;

        #endregion

        #region Methods

        public override void Localize()
        {
            base.Localize();
            Text = Resources.ConfigDialog_Text;
            btnOk.Text = Resources.ConfigDialog_Ok;
            btnCancel.Text = Resources.ConfigDialog_Cancel;
            lblReceiver.Text = Resources.ConfigDialog_lbl_Receiver;
            lblBody.Text = Resources.ConfigDialog_lbl_Body;
        }

        #endregion
    }
}
