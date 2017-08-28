using SwissAcademic.Addons.CheckUrlAndSetDate.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.CheckUrlAndSetDate
{
    public class Addon : CitaviAddOn
    {
        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(AddonKeys.CommandbarButton, StringComparison.OrdinalIgnoreCase) && (e.Form is MainForm mainForm))
            {
                if (Program.ProjectShells.Count != 0)
                {
                    CheckUrlAndSetDateStringsMacro.Run(mainForm);
                    e.Handled = true;
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
                        .InsertCommandbarButton(7, AddonKeys.CommandbarButton, CheckUrlAndSetDateResources.CheckUrlAndSetDateCommandText, image: CheckUrlAndSetDateResources.addon);
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
                    .GetCommandbarButton(AddonKeys.CommandbarButton);

                if (button != null) button.Text = CheckUrlAndSetDateResources.CheckUrlAndSetDateCommandText;
            }
            base.OnLocalizing(form);
        }

        #endregion
    }
}