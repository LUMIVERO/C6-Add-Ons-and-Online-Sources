using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;
using SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch.Properties;

namespace SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_UpdateBibliografic = "SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearch.UpdateBibliograficCommand";

        #endregion

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
                    case (Key_UpdateBibliografic):
                        {
                            using (var dialog = new OverrideFieldsDialog { Owner = e.Form })
                            {
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    Macro.Run(mainForm, dialog.Settings);
                                }
                            }
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
                        .InsertCommandbarButton(4, Key_UpdateBibliografic, UpdateBibliographicDataFromPubMedSearchResources.CommandText, image: UpdateBibliographicDataFromPubMedSearchResources.addon);
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
                                     .GetCommandbarButton(Key_UpdateBibliografic);
                if (button != null) button.Text = UpdateBibliographicDataFromPubMedSearchResources.CommandText;
            }

            base.OnLocalizing(form);
        }

        #endregion
    }
}