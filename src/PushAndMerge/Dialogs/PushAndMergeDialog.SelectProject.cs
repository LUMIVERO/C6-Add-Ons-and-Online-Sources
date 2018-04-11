using SwissAcademic.Addons.PushAndMerge.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Settings;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissAcademic.Addons.PushAndMerge
{
    public partial class PushAndMergeDialog
    {
        #region Fields

        ComboBoxHelperCollection _targetProjectEditorComboBoxHelpers;

        #endregion

        #region ProjectCreationParametersExtended

        class ProjectCreationParametersExtended
        {
            public string FolderPath { get; set; }
            public string ProjectName { get; set; }
            public ProjectType ProjectType { get; set; }
        }

        #endregion

        #region Methods

        #region FinishAsync

        async Task FinishAsync()
        {
            var targetProjectData = (KnownProject)projectTextEditor.SelectedItem;

            ProjectShell targetProjectShell;

            using (var cancellationTokenSorce = new CancellationTokenSource())
            {
                targetProjectShell = await Program.OpenProjectAsync(this, targetProjectData.ConnectionString, cancellationTokenSorce.Token);
            }

            if (targetProjectShell == null) return;

            await GenericProgressDialog.RunTask(targetProjectShell.PrimaryMainForm, () =>
            {
                return PushAndMergeHandler.ExecuteAsync(DialogOwner, _sourceProject, targetProjectShell.Project, _pushAndMergeOptions, null);
            });
        }

        #endregion

        #region LocalizeSelectProject
        void LocalizeSelectProject()
        {
            nextButton.Text = PushAndMergeResources.FinishButton;
            projectNameLabel.Text = PushAndMergeResources.ProjectNameLabel;

            openProjectButton.Text = PushAndMergeResources.OpenProjectButtonText;

            nextButton.Text = PushAndMergeResources.FinishButton;
        }
        #endregion

        #region InitTitleSelection
        void InitSelectProject(Project sourceProject)
        {
            backButton.Visible = true;

            _targetProjectEditorComboBoxHelpers = new ComboBoxHelperCollection();

            foreach (var knownProject in Program.Engine.Settings.General.KnownProjects)
            {
                if (!knownProject.Exists() || knownProject.ConnectionString.Equals(sourceProject.DesktopProjectConfiguration.ConnectionIdentifier, StringComparison.OrdinalIgnoreCase)) continue;
                _targetProjectEditorComboBoxHelpers.Add(knownProject, knownProject.Name);
            }

            _targetProjectEditorComboBoxHelpers.Sort();

            projectTextEditor.ListItems = _targetProjectEditorComboBoxHelpers;
            UpdateProjectEditorLeftIcon();
        }
        #endregion

        #region UpdateProjectEditorLeftIcon
        private void UpdateProjectEditorLeftIcon()
        {
            ProjectType projectType;
            var selectedItem = projectTextEditor.SelectedItem as KnownProject;

            if (selectedItem == null)
            {
                var parameters = projectTextEditor.SelectedItem as ProjectCreationParametersExtended;

                if (parameters == null)
                {
                    projectTextEditor.LeftIcon = null;
                    return;
                }
                else
                    projectType = parameters.ProjectType;
            }
            else
                projectType = selectedItem.ProjectType;

            switch (projectType)
            {
                case ProjectType.DesktopSQLite:
                    projectTextEditor.LeftIcon = Citavi.Shell.Properties.Resources.ProjectType_SQLite_16x;
                    break;
                case ProjectType.DesktopSqlServer:
                    projectTextEditor.LeftIcon = Citavi.Shell.Properties.Resources.ProjectType_SqlServer_16x;
                    break;
                case ProjectType.DesktopCloud:
                    projectTextEditor.LeftIcon = Citavi.Shell.Properties.Resources.ProjectType_Cloud_16x;
                    break;
            }
        }
        #endregion

        #endregion

        #region EventHandlers

        #region ProjectTextEditorDrawItem
        void ProjectTextEditorDrawItem(object sender, TextEditorDrawItemEventArgs e)
        {
            if (e.Index == -1) return;

            Brush brush;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds.Left + Control2.ScaleX(20), e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
                brush = SystemBrushes.HighlightText;
            }
            else
            {
                e.DrawBackground();
                brush = SystemBrushes.WindowText;
            }

            ListBox listBox = (ListBox)sender;
            Bitmap icon;
            ProjectType projectType;
            var knownProject = ((ComboBoxHelper)listBox.Items[e.Index]).Value as KnownProject;

            if (knownProject != null)
            {
                projectType = knownProject.ProjectType;
            }
            else
            {
                var parameters = ((ComboBoxHelper)listBox.Items[e.Index]).Value as ProjectCreationParametersExtended;
                if (parameters == null) return;

                projectType = parameters.ProjectType;

            }

            switch (projectType)
            {
                case ProjectType.DesktopSQLite:
                    icon = Citavi.Shell.Properties.Resources.ProjectType_SQLite_16x;
                    break;

                case ProjectType.DesktopSqlServer:
                    icon = Citavi.Shell.Properties.Resources.ProjectType_SqlServer_16x;
                    break;

                case ProjectType.DesktopCloud:
                    icon = Citavi.Shell.Properties.Resources.ProjectType_Cloud_16x;
                    break;

                default:
                    icon = null;
                    break;
            }

            if (icon != null)
            {
                var factor = Math.Min(Control2.AutoScaleSize.Width, Control2.AutoScaleSize.Height);
                e.Graphics.DrawImage(icon, e.Bounds.Left, e.Bounds.Top, 16 * factor, 16 * factor);
            }
            e.Graphics.DrawString(listBox.Items[e.Index].ToString(), e.Font, brush, e.Bounds.Left + Control2.ScaleX(20), e.Bounds.Top);

            e.Handled = true;
        }
        #endregion

        #region ProjectTextEditorAfterDropDownClosed
        void ProjectTextEditorAfterDropDownClosed(object sender, EventArgs e)
        {
            UpdateProjectEditorLeftIcon();
        }
        #endregion

        #region OpenProjectButtonClick
        void OpenProjectButtonClick(object sender, EventArgs e)
        {
            // TSu: Create OpenProjectDialog via reflection since the constructor is internal
            var openProjectDialog = (OpenProjectDialog)Activator.CreateInstance(
                typeof(OpenProjectDialog), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { this, ProjectType.DesktopSQLite }, CultureInfo.InvariantCulture);

            try
            {
                if (openProjectDialog.ShowDialog() == DialogResult.OK)
                {
                    if (openProjectDialog.ConnectionString == _sourceProject.DesktopProjectConfiguration.ConnectionString)
                    {
                        MessageBox.Show(this, PushAndMergeResources.CantMoveItemsInTheSameProject, ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    var openProjectHelper = new OpenProjectHelper(this);
                    var evalualtionResult = openProjectHelper.EvaluateProjectTypeAndConnectionString(openProjectDialog.ConnectionString);

                    var knownProject = new KnownProject(evalualtionResult.ProjectType, openProjectDialog.ConnectionString);

                    var existingComboBoxHelper = _targetProjectEditorComboBoxHelpers.Find(i => ((KnownProject)i.Value).ConnectionIdentifier == knownProject.ConnectionIdentifier);

                    if (existingComboBoxHelper == null)
                    {
                        existingComboBoxHelper = _targetProjectEditorComboBoxHelpers.Insert(0, knownProject, knownProject.Name);
                        projectTextEditor.ListItems = _targetProjectEditorComboBoxHelpers;
                    }
                    projectTextEditor.SelectedItem = existingComboBoxHelper.Value;
                    UpdateProjectEditorLeftIcon();
                }
            }
            finally
            {
                openProjectDialog?.Dispose();
            }
        }
        #endregion

        #endregion
    }
}
