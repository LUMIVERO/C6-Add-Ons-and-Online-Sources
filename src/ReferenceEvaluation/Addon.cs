using SwissAcademic.Addons.ReferenceEvaluation.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;

namespace SwissAcademic.Addons.ReferenceEvaluation
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_Addon = "SwissAcademic.Addons.ReferenceEvaluation.ButtonCommand";

        #endregion

        #region Methods

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_Addon, System.StringComparison.Ordinal))
            {
                e.Handled = true;

                using (var dialog = new ReferenceEvaluationDialog(mainForm))
                {
                    dialog.ShowDialog();
                }
            }
        }
        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm.GetMainCommandbarManager()
                    .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                    .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                    .AddCommandbarButton(Key_Button_Addon, Resources.Addon_Command, image: Resources.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                     .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                     .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                     .GetCommandbarButton(Key_Button_Addon);

            if (button != null)
            {
                button.Text = Resources.Addon_Command;
            }
        }

        #endregion
    }
}