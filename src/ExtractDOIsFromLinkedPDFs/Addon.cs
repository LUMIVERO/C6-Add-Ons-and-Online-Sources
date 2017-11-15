using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;
using SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs.Properties;
using System;

namespace SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_Button_ExtractDOIsFromLinkedPDFs = "SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs.DoiButtonCommand";

        #endregion

        #region Properties
        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Form is MainForm mainForm)
            {
                e.Handled = true;

                switch (e.Key)
                {
                    case (Key_Button_ExtractDOIsFromLinkedPDFs):
                        {
                            try
                            {
                                Macro.Run(mainForm, mainForm.Project);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(mainForm, ex.ToString(), mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
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
                        .InsertCommandbarButton(4, Key_Button_ExtractDOIsFromLinkedPDFs, ExtractDOIsFromLinkedPDFsResources.CommandButtonText);
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
                                     .GetCommandbarButton(Key_Button_ExtractDOIsFromLinkedPDFs);
                if (button != null) button.Text = ExtractDOIsFromLinkedPDFsResources.CommandButtonText;
            }

            base.OnLocalizing(form);
        }

        #endregion
    }
}