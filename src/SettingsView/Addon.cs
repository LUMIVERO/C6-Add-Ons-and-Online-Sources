using SwissAcademic.Addons.SettingsView.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.SettingsView
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_Button_ShowSettingsViewDialog = "SwissAcademic.Addons.SettingsView.ShowSettingsViewDialog";

        #endregion

        #region Properties
        public override AddOnHostingForm HostingForm
        {
            get { return AddOnHostingForm.MainForm; }
        }

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_ShowSettingsViewDialog, StringComparison.OrdinalIgnoreCase))
            {
                using (var dialog = new SettingsViewDialog(e.Form))
                {
                    dialog.ShowDialog();
                }

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
                        .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.Tools)
                        .InsertCommandbarButton(6, Key_Button_ShowSettingsViewDialog, SettingsViewResources.ShowSettingsView, image: SettingsViewResources.addon);
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            if (form is MainForm mainForm)
            {

                CommandbarButton button = mainForm.GetMainCommandbarManager()
                                                  .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                                  .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.Tools)
                                                  .GetCommandbarButton(Key_Button_ShowSettingsViewDialog);

                if (button != null) button.Text = SettingsViewResources.ShowSettingsView;
            }

            base.OnLocalizing(form);
        }

        #endregion
    }
}