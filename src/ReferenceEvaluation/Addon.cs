using SwissAcademic.Addons.ReferenceEvaluationAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;

namespace SwissAcademic.Addons.ReferenceEvaluationAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {
        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(ButtonKey, System.StringComparison.Ordinal))
            {
                e.Handled = true;

                using (var dialog = new ReferenceEvaluationForm(mainForm))
                {
                    dialog.ShowDialog();
                }
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm
                .GetMainCommandbarManager()
                .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                .AddCommandbarButton(ButtonKey, Resources.Addon_Command, image: Resources.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                     .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                     .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                     .GetCommandbarButton(ButtonKey);

            if (button != null)
            {
                button.Text = Resources.Addon_Command;
            }
        }
    }
}