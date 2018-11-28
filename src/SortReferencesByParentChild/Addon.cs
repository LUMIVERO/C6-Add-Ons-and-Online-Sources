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

            //Check if user has several MainForms open, if so, deactivate ItemFilter.
            //Reason: MainForm.NavigationGridItemFilter is static property and therefore application-wide, but sorting isn't.
            //In order to avoid, that the ItemFilter collides with a sorting, that does NOT show children below their parents, we just switch off the ItemFilter.

            //The DEBUG version of C3 has a new NavigationGridItemFilterInstace property and can therefore handle several MainForms independently.
            //if (Program.ProjectShells.Count > 1 || Program.ProjectShells[0].MainForms.Count > 1)
            if (Program.ProjectShells.Count > 1)
            {
                if (MainForm.NavigationGridItemFilter is ItemFilter itemFilter)
                {
                    MessageBox.Show(Properties.Resources.WarningMoreThanOneProjectOpen, mainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm.NavigationGridItemFilter = null;
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }
                return;
            }

            if (mainForm.Project.References.Comparer is ReferenceComparerByParentChild specialComparer)
            {



                if (MainForm.NavigationGridItemFilter == null)
                {
                    MainForm.NavigationGridItemFilter = new ItemFilter(mainForm);
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }

                //we MUST display child items indented if not yet the case
                if (!(MainForm.NavigationGridItemFilter is ItemFilter itemFilter))
                {
                    MainForm.NavigationGridItemFilter = new ItemFilter();
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine(string.Format("ApplicationIdle: {0} - Has other comparer", mainForm.Project.Name));

                if (MainForm.NavigationGridItemFilter != null)
                {
                    MainForm.NavigationGridItemFilter = null;
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }

                //we MUST NOT display child items indented
                if (MainForm.NavigationGridItemFilter is ItemFilter itemFilter)
                {
                    MainForm.NavigationGridItemFilter = null;
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }

            }

            base.OnApplicationIdle(mainForm);
        }

        #endregion
    }
}