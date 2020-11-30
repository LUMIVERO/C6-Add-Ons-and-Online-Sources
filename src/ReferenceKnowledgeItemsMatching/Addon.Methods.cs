using SwissAcademic.Citavi.Shell;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class Addon
    {
        void ShowSettingsDialog(MainForm owner, AddonSettings settings)
        {
            using (var dialog = new SettingsDialog(owner, settings))
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                AddonSettings.SaveToProjectSettings(owner.Project.ProjectSettings, dialog.AddonSettings);
            }
        }
    }
}
