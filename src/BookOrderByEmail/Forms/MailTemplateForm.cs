using SwissAcademic.Addons.BookOrderByEmailAddon.Properties;
using SwissAcademic.Controls;
using System.Collections.Generic;

namespace SwissAcademic.Addons.BookOrderByEmailAddon
{
    public partial class MailTemplateForm : FormBase
    {
        // Constructors

        MailTemplateForm() => InitializeComponent();

        public MailTemplateForm(IDictionary<string, string> settings) : this()
        {
            txtBody.Text = settings.GetValueOrDefault(Addon.SettingsKey_Body, string.Empty); ;
            txtReceiver.Text = settings.GetValueOrDefault(Addon.SettingsKey_Receiver, string.Empty);
        }

        // Properties

        public string Receiver => txtReceiver.Text;

        public string Body => txtBody.Text;

        //  Methods

        public override void Localize()
        {
            base.Localize();
            Text = Resources.ConfigDialog_Text;
            btnOk.Text = Resources.ConfigDialog_Ok;
            btnCancel.Text = Resources.ConfigDialog_Cancel;
            lblReceiver.Text = Resources.ConfigDialog_lbl_Receiver;
            lblBody.Text = Resources.ConfigDialog_lbl_Body;
        }
    }
}
