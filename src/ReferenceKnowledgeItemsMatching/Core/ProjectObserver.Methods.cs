using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using System.Linq;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class ProjectObserver
    {
        void AddProject(Project project)
        {
            if (_projects.ContainsKey(project)) return;

            _projects.Add(project, AddonSettings.LoadfromProjectSettings(project.ProjectSettings));

            project.ProjectSettings.SettingChanging += ProjectSettings_SettingChanging;
            project.References.CollectionChanged += References_CollectionChanged;
            project.AllKnowledgeItems.CollectionChanged += AllKnowledgeItems_CollectionChanged;
        }

        void RemoveProject(Project project)
        {
            if (!_projects.ContainsKey(project) && Program.ProjectShells.OfType<ProjectShell>().Any(ps => ps.Project == project)) return;

            project.ProjectSettings.SettingChanging -= ProjectSettings_SettingChanging;
            project.References.CollectionChanged -= References_CollectionChanged;
            project.AllKnowledgeItems.CollectionChanged -= AllKnowledgeItems_CollectionChanged;

            _projects.Remove(project);
        }

        private void ProjectShells_CollectionChanged(object sender, Collections.CollectionChangedEventArgs<ProjectShell> e)
        {
            if (!e.HasRecords) return;

            switch (e.ChangeType)
            {
                case Collections.CollectionChangeType.ItemsAdded:
                    e.Records.ForEach(record => AddProject(record.Item?.Project));
                    break;
                case Collections.CollectionChangeType.ItemsDeleted:
                    e.Records.ForEach(record => RemoveProject(record.Item?.Project));
                    break;
            }
        }
    }
}
