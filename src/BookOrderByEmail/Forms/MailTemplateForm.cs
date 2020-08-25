using SwissAcademic.Addons.BookOrderByEmailAddon.Properties;
using SwissAcademic.Controls;
using System;
using System.Collections.Generic;

namespace SwissAcademic.Addons.BookOrderByEmailAddon
{
    public partial class MailTemplateForm : FormBase
    {
        protected override void OnLoad(EventArgs e)
        {
            txtBody.Text = _settings.GetValueOrDefault(Addon.SettingsKey_Body, string.Empty); ;
            txtReceiver.Text = _settings.GetValueOrDefault(Addon.SettingsKey_Receiver, string.Empty);

            base.OnLoad(e);
        }

        readonly IDictionary<string, string> _settings;

        // Constructors

        MailTemplateForm() => InitializeComponent();

        public MailTemplateForm(IDictionary<string, string> settings) : this() => _settings = settings;

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
