using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Citavi;
using SwissAcademic;

namespace DerivePageCountFromPageRangeAddon
{
    public class DerivePageCountFromPageRangeAddon : CitaviAddOn
    {

        #region Fields

        List<Project> _observedProjects = new List<Project>();

        #endregion

        #region Properties

        #region HostingForm

        public override AddOnHostingForm HostingForm
        {
            get { return AddOnHostingForm.MainForm; }
        }

        #endregion

        #endregion

        #region Methods

        #region OnBeforePerformingCommand

        protected override void OnBeforePerformingCommand(SwissAcademic.Controls.BeforePerformingCommandEventArgs e)
        {
            try
            {
                switch (e.Key)
                {
                    case "DerivePageCountFromPageRange":
                        {
                            e.Handled = true;

                            if (MessageBox.Show(e.Form, Properties.Resources.FilterInfo, e.Form.ProductName, MessageBoxButtons.OKCancel) != DialogResult.OK) return;

                            DerivePageCountFromPageRange(((MainForm)e.Form).GetFilteredReferences());

                            MessageBox.Show(e.Form, Properties.Resources.Finished, e.Form.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                }
            }

            catch (Exception exception)
            {
                MessageBox.Show(e.Form, exception.Message, e.Form.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            base.OnBeforePerformingCommand(e);
        }

        #endregion

        #region OnHostingFormLoaded

        protected override void OnHostingFormLoaded(Form hostingForm)
        {
            string key = "DerivePageCountFromPageRange";
            string text = Properties.Resources.DerivePageCountFromPageRange;

            try
            {
                var mainForm = hostingForm as MainForm;
                if (mainForm == null)
                {
                    base.OnHostingFormLoaded(hostingForm);
                    return;
                }

                if (!_observedProjects.Contains(mainForm.Project))
                {
                    _observedProjects.Add(mainForm.Project);
                    ObserveProject(mainForm.Project, true);
                }

                ObserveMainForm(mainForm, true);

                var button = mainForm.GetMainCommandbarManager().
                    GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu).
                    GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References).
                    AddCommandbarButton(key, text);
                button.HasSeparator = true;
            }

            catch (Exception exception)
            {
                MessageBox.Show(hostingForm, exception.Message, hostingForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            base.OnHostingFormLoaded(hostingForm);
        }

        #endregion

        #region OnLocalizing

        protected override void OnLocalizing(Form hostingForm)
        {
            var mainForm = hostingForm as MainForm;
            var button = mainForm.GetMainCommandbarManager().
                    GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu).
                    GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References).GetCommandbarButton("DerivePageCountFromPageRange");
            if (button == null) return;

            button.Text = Properties.Resources.DerivePageCountFromPageRange;

            base.OnLocalizing(hostingForm);
        }

        #endregion

        #region DerivePageCountFromPageRange

        static void DerivePageCountFromPageRange(Reference reference)
        {
            if (reference.PageRange == PageRange.Empty) return;

            int startPage;
            int endPage;

            if (!reference.PageRange.StartPage.Number.HasValue) return;

            startPage = reference.PageRange.StartPage.Number.Value;

            endPage = reference.PageRange.EndPage.Number.HasValue
                             ? reference.PageRange.EndPage.Number.Value
                             : startPage;


            reference.PageCount = (endPage - startPage + 1).ToString();
        }

        static void DerivePageCountFromPageRange(IList<Reference> references)
        {
            foreach (var reference in references)
            {
                DerivePageCountFromPageRange(reference);
            }
        }

        #endregion

        #region ObserveMainForm

        void ObserveMainForm(MainForm mainForm, bool start)
        {
            if (start) mainForm.FormClosing += mainForm_FormClosing;
            else mainForm.FormClosing -= mainForm_FormClosing;
        }

        #endregion

        #region ObserveProject

        void ObserveProject(Project project, bool start)
        {
            if (start) project.References.CollectionChanged += References_CollectionChanged;
            else project.References.CollectionChanged -= References_CollectionChanged;
        }

        #endregion

        #endregion

        #region Event handlers

        #region MainForm_FormClosing

        void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var mainForm = sender as MainForm;
            if (mainForm == null) return;

            ObserveMainForm(mainForm, false);

            if (mainForm.ProjectShell.MainForms.Count == 1)
            {
                _observedProjects.Remove(mainForm.Project);
                ObserveProject(mainForm.Project, false);
            }
        }

        #endregion

        #region References_CollectionChanged

        void References_CollectionChanged(object sender, SwissAcademic.Collections.CollectionChangedEventArgs<Reference> e)
        {
            switch (e.ChangeType)
            {
                case SwissAcademic.Collections.CollectionChangeType.ItemsAdded:
                    DerivePageCountFromPageRange(e.Records.ConvertAll(record => record.Item));
                    break;

                case SwissAcademic.Collections.CollectionChangeType.ItemsChanged:
                    {

                        foreach (var record in e.Records)
                        {
                            if (record.Trigger != null &&
                                record.Trigger.Property == ReferencePropertyDescriptor.PageRange)
                            {
                                DerivePageCountFromPageRange(record.Item);
                            }
                        }
                    }
                    break;
            }
        }

        #endregion

        #endregion
    }
}
