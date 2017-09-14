using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;
using SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs.Properties;

namespace SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs
{
    public class Addon : CitaviAddOn
    {
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
                    case (AddonKeys.DoiButtonCommand):
                        {
                            Macro.Run(mainForm, mainForm.Project);
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
                        .InsertCommandbarButton(4, AddonKeys.DoiButtonCommand, ExtractDOIsFromLinkedPDFsResources.CommandButtonText);
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
                                     .GetCommandbarButton(AddonKeys.DoiButtonCommand);
                if (button != null) button.Text = ExtractDOIsFromLinkedPDFsResources.CommandButtonText;
            }

            base.OnLocalizing(form);
        }

        #endregion
    }
}