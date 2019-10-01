using SwissAcademic.Addons.DerivePageCountFromPageRange.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SwissAcademic.Addons.DerivePageCountFromPageRange
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_Button_DerivePageCountFromPageRange = "SwissAcademic.Addons.DerivePageCountFromPageRange.CommandbarButton";

        #endregion

        #region Fields

        List<Project> _observedProjects;

        #endregion

        #region Constructors

        public Addon()
        {
            _observedProjects = new List<Project>();
        }

        #endregion

        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(Controls.BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_DerivePageCountFromPageRange, StringComparison.OrdinalIgnoreCase) && e.Form is MainForm mainForm)
            {
                try
                {
                    e.Handled = true;

                    if (MessageBox.Show(mainForm, DerivePageCountFromPageRangeResources.FilterInfo, mainForm.ProductName, MessageBoxButtons.OKCancel) != DialogResult.OK) return;

                    DerivePageCountFromPageRange(mainForm.GetFilteredReferences());

                    MessageBox.Show(mainForm, DerivePageCountFromPageRangeResources.Finished, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                catch (Exception exception)
                {
                    MessageBox.Show(mainForm, exception.Message, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            base.OnBeforePerformingCommand(e);
        }

        protected override void OnHostingFormLoaded(Form form)
        {
            if (form is MainForm mainForm)
            {
                try
                {
                    if (!_observedProjects.Contains(mainForm.Project))
                    {
                        _observedProjects.Add(mainForm.Project);
                        ObserveProject(mainForm.Project, true);
                    }

                    ObserveMainForm(mainForm, true);

                    var button = mainForm.GetMainCommandbarManager()
                                         .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                         .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                         .AddCommandbarButton(Key_Button_DerivePageCountFromPageRange, DerivePageCountFromPageRangeResources.DerivePageCountFromPageRange, image: DerivePageCountFromPageRangeResources.addon);
                    if (button != null) button.HasSeparator = true;


                    base.OnHostingFormLoaded(form);
                }

                catch (Exception exception)
                {
                    MessageBox.Show(mainForm, exception.Message, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            if (form is MainForm mainForm)
            {
                var button = mainForm.GetMainCommandbarManager()
                                     .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                     .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                     .GetCommandbarButton(Key_Button_DerivePageCountFromPageRange);

                if (button != null) button.Text = DerivePageCountFromPageRangeResources.DerivePageCountFromPageRange;
            }

            base.OnLocalizing(form);
        }

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

        void ObserveMainForm(MainForm mainForm, bool start)
        {
            if (start) mainForm.FormClosing += MainForm_FormClosing;
            else mainForm.FormClosing -= MainForm_FormClosing;
        }

        void ObserveProject(Project project, bool start)
        {
            if (start) project.References.CollectionChanged += References_CollectionChanged;
            else project.References.CollectionChanged -= References_CollectionChanged;
        }

        #endregion

        #region Eventhandlers

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is MainForm mainForm)
            {
                ObserveMainForm(mainForm, false);

                if (mainForm.ProjectShell.MainForms.Count == 1)
                {
                    _observedProjects.Remove(mainForm.Project);
                    ObserveProject(mainForm.Project, false);
                }
            }
        }

        void References_CollectionChanged(object sender, Collections.CollectionChangedEventArgs<Reference> e)
        {
            switch (e.ChangeType)
            {
                case Collections.CollectionChangeType.ItemsAdded:
                    DerivePageCountFromPageRange(e.Records.ConvertAll(record => record.Item));
                    break;

                case Collections.CollectionChangeType.ItemsChanged:
                    {
                        foreach (var record in e.Records)
                        {
                            if (record.Trigger != null && record.Trigger.Property == ReferencePropertyDescriptor.PageRange)
                            {
                                DerivePageCountFromPageRange(record.Item);
                            }
                        }
                    }
                    break;
            }
        }

        #endregion
    }
}
