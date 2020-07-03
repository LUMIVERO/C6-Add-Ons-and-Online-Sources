using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Metadata;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SwissAcademic.Addons.DerivePageCountFromPageRangeAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {

        // Fields

        readonly List<Project> _observedProjects = new List<Project>();

        // Methods

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_DerivePageCountFromPageRange, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;

                try
                {
                    if (MessageBox.Show(mainForm, Properties.Resources.FilterInfo, mainForm.ProductName, MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        DerivePageCountFromPageRange(mainForm.GetFilteredReferences());

                        MessageBox.Show(mainForm, Properties.Resources.Finished, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                catch (Exception exception)
                {
                    MessageBox.Show(mainForm, exception.Message, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
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
                                     .AddCommandbarButton(Key_Button_DerivePageCountFromPageRange, Properties.Resources.DerivePageCountFromPageRange, image: Properties.Resources.addon);
                if (button != null)
                {
                    button.HasSeparator = true;
                }
            }

            catch (Exception exception)
            {
                MessageBox.Show(mainForm, exception.Message, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                  .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                  .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                  .GetCommandbarButton(Key_Button_DerivePageCountFromPageRange);

            if (button != null)
            {
                button.Text = Properties.Resources.DerivePageCountFromPageRange;
            }
        }


        static void DerivePageCountFromPageRange(Reference reference)
        {
            if (reference.PageRange == PageRange.Empty) return;

            int startPage;
            int endPage;

            if (!reference.PageRange.StartPage.Number.HasValue) return;

            startPage = reference.PageRange.StartPage.Number.Value;

            endPage = reference.PageRange.EndPage.Number ?? startPage;

            reference.PageCount = (endPage - startPage + 1).ToString();
        }

        static void DerivePageCountFromPageRange(IList<Reference> references) => references.ForEach(reference => DerivePageCountFromPageRange(reference));

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

        // EventHandlers

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
    }
}
