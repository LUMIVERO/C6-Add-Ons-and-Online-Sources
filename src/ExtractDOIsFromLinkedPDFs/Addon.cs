using SwissAcademic.Addons.ExtractDOIsFromLinkedPDFsAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ExtractDOIsFromLinkedPDFsAddon
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_ExtractDOIsFromLinkedPDFs = "SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs.DoiButtonCommand";

        #endregion

        #region Methods

        public async override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_ExtractDOIsFromLinkedPDFs, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;
                try
                {
                    await Macro.Run(mainForm, mainForm.Project);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(mainForm, ex.ToString(), mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm.GetMainCommandbarManager()
                    .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                    .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                    .InsertCommandbarButton(4, Key_Button_ExtractDOIsFromLinkedPDFs, Resources.CommandButtonText, image: Resources.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                 .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                 .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                 .GetCommandbarButton(Key_Button_ExtractDOIsFromLinkedPDFs);
            if (button != null)
            {
                button.Text = Resources.CommandButtonText;
            }
        }

        #endregion
    }
}