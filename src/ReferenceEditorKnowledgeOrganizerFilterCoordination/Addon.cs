using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceEditorKnowledgeOrganizerFilterCoordinationAddon
{
    public class Addon : CitaviAddOn<MainForm>
    {
        // Fields

        bool _eventsSuspended;

        // Methods

        public override void OnHostingFormLoaded(MainForm mainForm) => ObserveMainForm(mainForm, true);

        void ObserveMainForm(MainForm mainForm, bool start)
        {
            if (start)
            {
                mainForm.FormClosed += MainForm_FormClosed;
                mainForm.ReferenceEditorFilterSet.Filters.CollectionChanged += ReferenceEditorFilters_CollectionChanged;
                mainForm.KnowledgeOrganizerFilterSet.Filters.CollectionChanged += KnowledgeOrganizerFilters_CollectionChanged;
            }
            else
            {
                mainForm.FormClosed -= MainForm_FormClosed;
                mainForm.ReferenceEditorFilterSet.Filters.CollectionChanged -= ReferenceEditorFilters_CollectionChanged;
                mainForm.KnowledgeOrganizerFilterSet.Filters.CollectionChanged -= KnowledgeOrganizerFilters_CollectionChanged;
            }
        }

        // EventHandlers

        void MainForm_FormClosed(object sender, FormClosedEventArgs e) => ObserveMainForm((MainForm)sender, false);

        void KnowledgeOrganizerFilters_CollectionChanged(object sender, CollectionChangedEventArgs<KnowledgeItemFilter> e)
        {
            if (_eventsSuspended) return;

            var eventsSuspended = _eventsSuspended;
            try
            {
                _eventsSuspended = true;

                var activeMainForm = GetActiveMainFormOrDefault(mainForm => sender == mainForm.KnowledgeOrganizerFilterSet.Filters);

                if (activeMainForm == null) return;

                if (activeMainForm.KnowledgeOrganizerFilterSet.Filters.Count == 1)
                {
                    if (activeMainForm.KnowledgeOrganizerFilterSet.Filters[0].Category != null)
                    {
                        var filter = new ReferenceFilter(activeMainForm.KnowledgeOrganizerFilterSet.Filters[0].Category);
                        var list = new List<ReferenceFilter> { filter };
                        activeMainForm.ReferenceEditorFilterSet.Filters.ReplaceBy(list);
                    }

                    else if (activeMainForm.KnowledgeOrganizerFilterSet.Filters[0].Keyword != null)
                    {
                        var filter = new ReferenceFilter(activeMainForm.KnowledgeOrganizerFilterSet.Filters[0].Keyword);
                        var list = new List<ReferenceFilter> { filter };
                        activeMainForm.ReferenceEditorFilterSet.Filters.ReplaceBy(list);
                    }
                }
            }
            finally
            {
                _eventsSuspended = eventsSuspended;
            }
        }

        void ReferenceEditorFilters_CollectionChanged(object sender, CollectionChangedEventArgs<ReferenceFilter> e)
        {
            if (_eventsSuspended) return;

            var eventsSuspended = _eventsSuspended;
            try
            {
                _eventsSuspended = true;

                var activeMainForm = GetActiveMainFormOrDefault(mainForm => sender == mainForm.ReferenceEditorFilterSet.Filters);

                if (activeMainForm == null) return;

                if (activeMainForm.ReferenceEditorFilterSet.Filters.Count == 1)
                {
                    if (activeMainForm.ReferenceEditorFilterSet.Filters[0].Category != null)
                    {
                        var filter = new KnowledgeItemFilter(activeMainForm.ReferenceEditorFilterSet.Filters[0].Category);
                        var list = new List<KnowledgeItemFilter> { filter };
                        activeMainForm.KnowledgeOrganizerFilterSet.Filters.ReplaceBy(list);
                    }
                    else if (activeMainForm.ReferenceEditorFilterSet.Filters[0].Keyword != null)
                    {
                        var filter = new KnowledgeItemFilter(activeMainForm.ReferenceEditorFilterSet.Filters[0].Keyword);
                        var list = new List<KnowledgeItemFilter> { filter };
                        activeMainForm.KnowledgeOrganizerFilterSet.Filters.ReplaceBy(list);
                    }
                }
            }
            finally
            {
                _eventsSuspended = eventsSuspended;
            }
        }

        MainForm GetActiveMainFormOrDefault(System.Func<MainForm,bool> filter)
        {
            foreach (var mainForms in Program.ProjectShells.Select(projectShell => projectShell.MainForms))
            {
                if (mainForms.FirstOrDefault(filter) is MainForm mainForm) return mainForm;
            }

            return null;
        }
    }
}