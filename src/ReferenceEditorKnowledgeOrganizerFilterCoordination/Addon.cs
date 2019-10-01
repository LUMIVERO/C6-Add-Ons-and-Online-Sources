using System.Collections.Generic;
using System.Windows.Forms;
using SwissAcademic.Collections;
using System.Linq;

namespace SwissAcademic.Citavi.Shell.AddOns.ReferenceManagerKnowledgeOrganizerFilterCoordination
{
    public class Addon: CitaviAddOn<MainForm>
	{
        #region Fields

        bool _eventsSuspended;

        #endregion

        #region Methods

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            ObserveMainForm(mainForm, true);
            base.OnHostingFormLoaded(mainForm);
        }

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

        #endregion

        #region Event handlers

        void MainForm_FormClosed(object sender, FormClosedEventArgs e) => ObserveMainForm((MainForm)sender, false);

        void KnowledgeOrganizerFilters_CollectionChanged(object sender, CollectionChangedEventArgs<KnowledgeItemFilter> e)
        {
            if (_eventsSuspended) return;

            var eventsSuspended = _eventsSuspended;
            try
            {
                _eventsSuspended = true;

                MainForm activeMainForm = null;

                foreach (var projectShell in Program.ProjectShells)
                {
                    activeMainForm = projectShell.MainForms.FirstOrDefault(mainForm => sender == mainForm.KnowledgeOrganizerFilterSet.Filters);
                    if (activeMainForm != null) break ;
                }

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

                MainForm activeMainForm = null;

                foreach (var projectShell in Program.ProjectShells)
                {
                    activeMainForm = projectShell.MainForms.FirstOrDefault(mainForm => sender == mainForm.ReferenceEditorFilterSet.Filters);
                    if (activeMainForm != null) break;
                }

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

        #endregion

	}
}