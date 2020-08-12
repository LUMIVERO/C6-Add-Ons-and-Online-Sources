using SwissAcademic.Addons.ExtractDOIsFromLinkedPDFsAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ExtractDOIsFromLinkedPDFsAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {
        public async override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(ButtonKey, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;
                try
                {
                    await Macro.RunAsync(mainForm, mainForm.Project);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(mainForm, ex.ToString(), mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm
                .GetMainCommandbarManager()
                .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                .InsertCommandbarButton(4, ButtonKey, Resources.CommandButtonText, image: Resources.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                 .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                 .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                 .GetCommandbarButton(ButtonKey);
            if (button != null)
            {
                button.Text = Resources.CommandButtonText;
            }
        }
    }
}