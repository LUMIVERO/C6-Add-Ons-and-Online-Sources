using SwissAcademic.Addons.SendReferenceByEmail.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.SendReferenceByEmail
{
    public class Addon : CitaviAddOn
    {
        #region Properties
        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Form is MainForm mainForm && e.Key.Equals(AddonKeys.CommandbarButton, StringComparison.OrdinalIgnoreCase))
            {
                Outlook.CreateEmail();
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
                        .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                        .AddCommandbarButton(AddonKeys.CommandbarButton, SendReferenceByEmailResources.ButtonCaption);
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
                                     .GetCommandbarButton(AddonKeys.CommandbarButton);
                if (button != null) button.Text = SendReferenceByEmailResources.ButtonCaption;
            }
            base.OnLocalizing(form);
        }

        #endregion
    }
}