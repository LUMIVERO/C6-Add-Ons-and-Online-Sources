using SwissAcademic.Addons.SendReferenceByEmail.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.SendReferenceByEmail
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_Button_SendReferenceByEmail = "SwissAcademic.Addons.SendReferenceByEmail.CommandbarButton";

        #endregion

        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Form is MainForm mainForm && e.Key.Equals(Key_Button_SendReferenceByEmail, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    mainForm.ActiveReference.SendByEMailAsync(mainForm);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(mainForm, ee.ToString(), mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                e.Handled = true;
            }
            base.OnBeforePerformingCommand(e);
        }

        protected override void OnHostingFormLoaded(Form form)
        {
            if (form is MainForm mainForm)
            {
                mainForm.GetMainCommandbarManager()
                        .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                        .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                        .AddCommandbarButton(Key_Button_SendReferenceByEmail, SendReferenceByEmailResources.ButtonCaption, image: SendReferenceByEmailResources.addon);
            }
            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            if (form is MainForm mainForm)
            {
                var button = mainForm.GetMainCommandbarManager()
                                     .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                     .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                     .GetCommandbarButton(Key_Button_SendReferenceByEmail);
                if (button != null) button.Text = SendReferenceByEmailResources.ButtonCaption;
            }
            base.OnLocalizing(form);
        }

        #endregion
    }
}