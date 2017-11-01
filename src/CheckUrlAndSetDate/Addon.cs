using SwissAcademic.Addons.CheckUrlAndSetDate.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.CheckUrlAndSetDate
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_CommandbarButton = "SwissAcademic.Addons.CheckUrlAndSetDate.CommandbarButton";

        #endregion

        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_CommandbarButton, StringComparison.OrdinalIgnoreCase) && (e.Form is MainForm mainForm))
            {
                Macro.Run(mainForm);
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
                        .InsertCommandbarButton(7, Key_CommandbarButton, CheckUrlAndSetDateResources.CheckUrlAndSetDateCommandText, image: CheckUrlAndSetDateResources.addon);
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
                    .GetCommandbarButton(Key_CommandbarButton);

                if (button != null) button.Text = CheckUrlAndSetDateResources.CheckUrlAndSetDateCommandText;
            }
            base.OnLocalizing(form);
        }

        #endregion
    }
}