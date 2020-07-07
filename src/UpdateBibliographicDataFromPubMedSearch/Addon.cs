using SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearchAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearchAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {
        public async override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(ButtonKey, System.StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;

                using (var dialog = new OverrideFieldsForm(mainForm))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        await Macro.Run(mainForm, dialog.Settings);
                    }
                }
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm
                .GetMainCommandbarManager()
                .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                .InsertCommandbarButton(4, ButtonKey, Resources.CommandText, image: Resources.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                 .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                 .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                 .GetCommandbarButton(ButtonKey);
            if (button != null)
            {
                button.Text = Resources.CommandText;
            }
        }
    }
}