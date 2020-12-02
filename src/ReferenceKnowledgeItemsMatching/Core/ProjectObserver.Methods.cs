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

            project.References.ForEach(reference => ObserveReference(reference, true));
            project.AllKnowledgeItems.ForEach(knowledgeItem => ObserveKnowledgeItem(knowledgeItem, true));
        }

        void RemoveProject(Project project)
        {
            if (!_projects.ContainsKey(project) && Program.ProjectShells.OfType<ProjectShell>().Any(ps => ps.Project == project)) return;

            project.ProjectSettings.SettingChanging -= ProjectSettings_SettingChanging;
            project.References.CollectionChanged -= References_CollectionChanged;
            project.AllKnowledgeItems.CollectionChanged -= AllKnowledgeItems_CollectionChanged;

            project.References.ForEach(reference => ObserveReference(reference, false));
            project.AllKnowledgeItems.ForEach(knowledgeItem => ObserveKnowledgeItem(knowledgeItem, false));

            _projects.Remove(project);
        }

        void ObserveReference(Reference reference, bool start)
        {
            if (reference is null) return;

            if (start)
            {
                reference.Categories.CollectionChanged += Reference_Categories_CollectionChanged;
                reference.Keywords.CollectionChanged += Reference_Keywords_CollectionChanged;
                reference.Groups.CollectionChanged += Reference_Groups_CollectionChanged;
            }
            else
            {
                reference.Categories.CollectionChanged -= Reference_Categories_CollectionChanged;
                reference.Keywords.CollectionChanged -= Reference_Keywords_CollectionChanged;
                reference.Groups.CollectionChanged -= Reference_Groups_CollectionChanged;
            }
        }

        void ObserveKnowledgeItem(KnowledgeItem knowledgeItem, bool start)
        {
            if (knowledgeItem is null) return;

            if (start)
            {
                knowledgeItem.Categories.CollectionChanged += KnowledgeItem_Categories_CollectionChanged;
                knowledgeItem.Keywords.CollectionChanged += KnowledgeItem_Keywords_CollectionChanged;
                knowledgeItem.Groups.CollectionChanged += KnowledgeItem_Groups_CollectionChanged;
            }
            else
            {
                knowledgeItem.Categories.CollectionChanged -= KnowledgeItem_Categories_CollectionChanged;
                knowledgeItem.Keywords.CollectionChanged -= KnowledgeItem_Keywords_CollectionChanged;
                knowledgeItem.Groups.CollectionChanged -= KnowledgeItem_Groups_CollectionChanged;
            }
        }
    }
}
