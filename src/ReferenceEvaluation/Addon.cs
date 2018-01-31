using SwissAcademic.Addons.ReferenceEvaluation.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceEvaluation
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_Button_Addon = "SwissAcademic.Addons.ReferenceEvaluation.ButtonCommand";

        #endregion

        #region Properties
        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Form is MainForm mainForm && e.Key.Equals(Key_Button_Addon, System.StringComparison.Ordinal))
            {
                e.Handled = true;

                using (var form = new ReferenceEvaluationDialog(mainForm))
                {
                    form.ShowDialog();
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
                        .AddCommandbarButton(Key_Button_Addon, ReferenceEvaluationResources.Addon_Command, image: ReferenceEvaluationResources.addon);
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
                     .GetCommandbarButton(Key_Button_Addon);

                if (button != null) button.Text = ReferenceEvaluationResources.Addon_Command;
            }
            base.OnLocalizing(form);
        }

        #endregion
    }
}