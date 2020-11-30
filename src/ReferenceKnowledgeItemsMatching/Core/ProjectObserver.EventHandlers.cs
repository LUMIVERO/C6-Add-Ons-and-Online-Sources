using SwissAcademic.Citavi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class ProjectObserver
    {
        void ProjectSettings_SettingChanging(object sender, SettingChangingEventArgs e)
        {
            if (e.SettingName.Equals(Addon.Keys.ProjectSettings, StringComparison.OrdinalIgnoreCase) && sender is ProjectSettings projectSettings)
            {
                _projects[projectSettings.Project] = AddonSettings.LoadFromJson(e.NewValue.ToString());
            }
        }

        void References_CollectionChanged(object sender, Collections.CollectionChangedEventArgs<Reference> e)
        {
            if (sender is Reference reference)
            { 
            
            }
        }

        private void AllKnowledgeItems_CollectionChanged(object sender, Collections.CollectionChangedEventArgs<KnowledgeItem> e)
        {
          
        }
    }
}
