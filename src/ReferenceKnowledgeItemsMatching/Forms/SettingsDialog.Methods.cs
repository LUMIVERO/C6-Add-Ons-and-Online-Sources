using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class SettingsDialog
    {
        bool AreSettingsValid() => chkActivateMatching.Checked && pnlSettings.Controls.OfType<CheckBox>().Any(chk => chk.Checked) || !chkActivateMatching.Checked;
    }
}
