using SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon.Properties;
using System.Diagnostics;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class SettingsDialog
    {
        void ShowHelp_Click(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(Resources.Help_Url);
    }
}
