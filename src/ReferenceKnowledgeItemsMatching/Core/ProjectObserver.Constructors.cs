using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using System.Collections.Generic;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class ProjectObserver
    {
        public ProjectObserver()
        {
            _projects = new Dictionary<Project, AddonSettings>();
            Program.ProjectShells.CollectionChanged += ProjectShells_CollectionChanged;
        }
    }
}
