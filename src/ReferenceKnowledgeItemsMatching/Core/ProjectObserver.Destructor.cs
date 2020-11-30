using SwissAcademic.Citavi.Shell;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class ProjectObserver
    {
        ~ProjectObserver()
        {
            Program.ProjectShells.CollectionChanged -= ProjectShells_CollectionChanged;
        }
    }
}
