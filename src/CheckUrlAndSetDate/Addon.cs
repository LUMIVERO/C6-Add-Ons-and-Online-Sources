using SwissAcademic.Addons.CheckUrlAndSetDateAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;

namespace SwissAcademic.Addons.CheckUrlAndSetDateAddon
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_CheckUrl = "SwissAcademic.Addons.CheckUrlAndSetDate.CommandbarButton";

        #endregion

        #region Methods

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_CheckUrl, StringComparison.OrdinalIgnoreCase))
            {
                Macro.Run(mainForm);
                e.Handled = true;
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm.GetMainCommandbarManager()
                    .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                    .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                    .InsertCommandbarButton(7, Key_Button_CheckUrl, Resources.CheckUrlAndSetDateCommandText, image: Resources.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                 .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                 .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                 .GetCommandbarButton(Key_Button_CheckUrl);

            if (button != null)
            {
                button.Text = Resources.CheckUrlAndSetDateCommandText;
            }
        }

        #endregion
    }
}