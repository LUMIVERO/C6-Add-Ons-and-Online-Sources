using SwissAcademic.Addons.CheckUrlAndSetDateAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;

namespace SwissAcademic.Addons.CheckUrlAndSetDateAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {
        public async override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(ButtonKey, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;
                await Macro.RunAsync(mainForm);
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm
                .GetMainCommandbarManager()
                .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                .InsertCommandbarButton(7, ButtonKey, Resources.CheckUrlAndSetDateCommandText, image: Resources.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm
                            .GetMainCommandbarManager()
                            .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                            .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                            .GetCommandbarButton(ButtonKey);

            if (button != null)
            {
                button.Text = Resources.CheckUrlAndSetDateCommandText;
            }
        }
    }
}