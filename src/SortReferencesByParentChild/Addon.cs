using System;
using System.Windows.Forms;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Linq;

namespace SwissAcademic.Addons.SortReferencesByParentChild
{
    public class SortReferencesByParentChildAddOn : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_Addon = "SwissAcademic.Addons.SortReferencesByParentChild.ButtonCommand";
        const string Key_Settings_Addon = "SwissAcademic.Addons.SortReferencesByParentChild.RestoreComparer";


        #endregion

        #region Fields

        CommandbarButton _button;

        #endregion

        #region EventHandlers

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sender is MainForm mainForm)
            {
                if (Program.ProjectShells.Count == 1)
                {
                    if (mainForm.Project.References.Comparer is ReferenceComparerByParentChild)
                    {
                        Settings.Add(Key_Settings_Addon, "Restore");
                    }
                }

                mainForm.FormClosing -= MainForm_FormClosing;
            }
        }

        #endregion

        #region Methods

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            if (!mainForm.IsPreviewFullScreenForm)
            {
                _button = mainForm
                             .GetReferenceEditorNavigationCommandbarManager()
                             .GetCommandbar(MainFormReferenceEditorNavigationCommandbarId.Toolbar)
                             .GetCommandbarMenu(MainFormReferenceEditorNavigationCommandbarMenuId.Sort)
                             .AddCommandbarButton(Key_Button_Addon, Properties.Resources.ParentChild);
                _button.HasSeparator = true;

                if (Settings.ContainsKey(Key_Settings_Addon))
                {
                    mainForm.Project.References.Comparer = ReferenceComparerByParentChild.Default;
                    mainForm.Project.References.AutoSort = true;
                    Settings.Remove(Key_Settings_Addon);
                }

                mainForm.FormClosing += MainForm_FormClosing;
            }

            base.OnHostingFormLoaded(mainForm);
        }

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_Addon, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    e.Handled = true;
                    mainForm.Project.References.Comparer = ReferenceComparerByParentChild.Default;
                    mainForm.Project.References.AutoSort = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(e.Form, exception.Message, e.Form.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            base.OnBeforePerformingCommand(mainForm, e);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            if (_button != null) _button.Text = Properties.Resources.ParentChild;
            base.OnLocalizing(mainForm);
        }

        public override void OnApplicationIdle(MainForm mainForm)
        {
            if (mainForm.Project.References.Comparer is ReferenceComparerByParentChild)
            {
                if (mainForm.ProjectShell.NavigationGridItemFilter == null)
                {
                    mainForm.ProjectShell.NavigationGridItemFilter = new ItemFilter(mainForm);
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }

                //we MUST display child items indented if not yet the case
                if (!(mainForm.ProjectShell.NavigationGridItemFilter is ItemFilter))
                {
                    mainForm.ProjectShell.NavigationGridItemFilter = new ItemFilter();
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }
            }
            else
            {
                if (mainForm.ProjectShell.NavigationGridItemFilter != null)
                {
                    mainForm.ProjectShell.NavigationGridItemFilter = null;
                    mainForm.ReferenceEditorNavigationGrid.Refresh();
                }

                //we MUST NOT display child items indented
                if (mainForm.ProjectShell.NavigationGridItemFilter is ItemFilter)
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