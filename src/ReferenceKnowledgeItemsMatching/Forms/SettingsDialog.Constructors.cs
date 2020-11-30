using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class SettingsDialog
    {
        public SettingsDialog(Form owner, AddonSettings addonSettings) : base(owner)
        {
            InitializeComponent();
            Owner = owner;
            _addonSettings = addonSettings;
        }
    }
}
