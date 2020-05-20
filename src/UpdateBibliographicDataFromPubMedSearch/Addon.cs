using SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearchAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearchAddon
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_UpdateBibliographicDataFromPubMedSearch = "SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch.UpdateBibliograficCommand";

        #endregion

        #region Methods


        public async override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_UpdateBibliographicDataFromPubMedSearch, System.StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;

                using (var dialog = new OverrideFieldsDialog(mainForm))
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
            mainForm.GetMainCommandbarManager()
                    .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                    .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                    .InsertCommandbarButton(4, Key_Button_UpdateBibliographicDataFromPubMedSearch, Resources.CommandText, image: Resources.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                 .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                 .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                 .GetCommandbarButton(Key_Button_UpdateBibliographicDataFromPubMedSearch);
            if (button != null)
            {
                button.Text = Resources.CommandText;
            }
        }

        #endregion
    }
}