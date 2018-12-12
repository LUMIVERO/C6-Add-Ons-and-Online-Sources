using System;
using System.Windows.Forms;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;

namespace SwissAcademic.Addons.SortReferencesByParentChild
{
    public class SortReferencesByParentChildAddOn : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_Addon = "SwissAcademic.Addons.SortReferencesByParentChild.ButtonCommand";

        #endregion

        #region Methods

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            var button = mainForm
                         .GetReferenceEditorNavigationCommandbarManager()
                         .GetCommandbar(MainFormReferenceEditorNavigationCommandbarId.Toolbar)
                         .GetCommandbarMenu(MainFormReferenceEditorNavigationCommandbarMenuId.Sort)
                         .AddCommandbarButton(Key_Button_Addon, Properties.Resources.ParentChild);

            button.HasSeparator = true;

            base.OnHostingFormLoaded(mainForm);
        }

        public override void OnBeforePerformingCommand(MainForm form, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_Addon, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    e.Handled = true;
                    form.Project.References.Comparer = ReferenceComparerByParentChild.Default;
                    form.Project.References.AutoSort = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(e.Form, exception.Message, e.Form.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            base.OnBeforePerformingCommand(form, e);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm
                         .GetReferenceEditorNavigationCommandbarManager()
                         .GetCommandbar(MainFormReferenceEditorNavigationCommandbarId.Toolbar)
                         .GetCommandbarMenu(MainFormReferenceEditorNavigationCommandbarMenuId.Sort)
                         .GetCommandbarButton(Key_Button_Addon);
            if (button != null) button.Text = Properties.Resources.ParentChild;
            base.OnLocalizing(mainForm);
        }

        public override void OnApplicationIdle(MainForm mainForm)
        {
            if (mainForm.Project.References.Comparer is ReferenceComparerByParentChild specialComparer)
            {
                if (mainForm.ProjectShell.NavigationGridItemFilter == null)
                {
                    mainForm.ProjectShell.NavigationGridItemFilter = new ItemFilter(mainForm);
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }

                //we MUST display child items indented if not yet the case
                if (!(mainForm.ProjectShell.NavigationGridItemFilter is ItemFilter itemFilter))
                {
                    mainForm.ProjectShell.NavigationGridItemFilter = new ItemFilter();
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine(string.Format("ApplicationIdle: {0} - Has other comparer", mainForm.Project.Name));

                if (mainForm.ProjectShell.NavigationGridItemFilter != null)
                {
                    mainForm.ProjectShell.NavigationGridItemFilter = null;
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }

                //we MUST NOT display child items indented
                if (mainForm.ProjectShell.NavigationGridItemFilter is ItemFilter itemFilter)
                {
                    mainForm.ProjectShell.NavigationGridItemFilter = null;
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }

            }

            base.OnApplicationIdle(mainForm);
        }

        #endregion
    }
}