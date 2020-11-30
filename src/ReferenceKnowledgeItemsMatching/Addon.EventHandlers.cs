using SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class Addon
    {
        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Keys.CommandbarButton, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;
                ShowSettingsDialog(mainForm, AddonSettings.LoadfromProjectSettings(mainForm.Project.ProjectSettings));
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            if (!mainForm.IsPreviewFullScreenForm)
            {
                var button = mainForm
                                .GetMainCommandbarManager()
                                .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                                .GetCommandbarMenu("ThisProject")
                                .AddCommandbarButton(Keys.CommandbarButton, Resources.CommandbarButton, image: Resources.addon);
                button.HasSeparator = true;
            }
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm
                             .GetMainCommandbarManager()
                             .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                             .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                             .GetCommandbarMenu("ThisProject")
                             .GetCommandbarButton(Keys.CommandbarButton);

            if (button != null) button.Text = Resources.CommandbarButton;
        }
    }
}
