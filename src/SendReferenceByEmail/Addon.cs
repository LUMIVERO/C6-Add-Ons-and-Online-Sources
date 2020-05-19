using SwissAcademic.Addons.SendReferenceByEmailAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.SendReferenceByEmailAddon
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_SendReferenceByEmail = "SwissAcademic.Addons.SendReferenceByEmail.CommandbarButton";

        #endregion

        #region Methods

        public async override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_SendReferenceByEmail, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;
                try
                {
                    await mainForm.ActiveReference.SendByEMailAsync(mainForm);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(mainForm, exception.ToString(), mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm.GetMainCommandbarManager()
                    .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                    .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                    .AddCommandbarButton(Key_Button_SendReferenceByEmail, Resources.ButtonCaption, image: Resources.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                 .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                 .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                 .GetCommandbarButton(Key_Button_SendReferenceByEmail);
            if (button != null)
            {
                button.Text = Resources.ButtonCaption;
            }
        }

        #endregion
    }
}