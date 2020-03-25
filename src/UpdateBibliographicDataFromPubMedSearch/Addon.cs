using SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_UpdateBibliographicDataFromPubMedSearch = "SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch.UpdateBibliograficCommand";

        #endregion

        #region Methods


        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_UpdateBibliographicDataFromPubMedSearch, System.StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;

                using (var dialog = new OverrideFieldsDialog { Owner = mainForm })
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        Macro.Run(mainForm, dialog.Settings);
                    }
                }
            }

            base.OnBeforePerformingCommand(mainForm, e);
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm.GetMainCommandbarManager()
                    .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                    .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                    .InsertCommandbarButton(4, Key_Button_UpdateBibliographicDataFromPubMedSearch, Resources.CommandText, image: Resources.addon);

            base.OnHostingFormLoaded(mainForm);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                 .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                 .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                 .GetCommandbarButton(Key_Button_UpdateBibliographicDataFromPubMedSearch);
            if (button != null) button.Text = Resources.CommandText;

            base.OnLocalizing(mainForm);
        }

        #endregion
    }
}