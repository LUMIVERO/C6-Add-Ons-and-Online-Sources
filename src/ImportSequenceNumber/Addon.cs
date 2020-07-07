using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Resources;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportSequenceNumberAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {
        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                    .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                    .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.FileThisProject)
                                    .AddCommandbarButton(ButtonKey, Properties.Resources.MenuCaption, image: Properties.Resources.addon);
            if (button != null)
            {
                button.HasSeparator = true;
            }
        }

        public async override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(ButtonKey, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;

                if (mainForm.Project.ProjectType == ProjectType.DesktopSQLite)
                {
                    using (var openFileDialog = new OpenFileDialog()
                    {
                        Title = Properties.Resources.OpenFileDialogTitle,
                        Filter = Properties.Resources.OpenFileDialogFilters,
                        CheckFileExists = true,
                        CheckPathExists = true,
                        Multiselect = false
                    })

                        if (openFileDialog.ShowDialog(mainForm) == DialogResult.OK)
                        {
                            try
                            {
                                var projectConfiguration = await DesktopProjectConfiguration.OpenAsync(Program.Engine, openFileDialog.FileName);

                                if (!projectConfiguration.SQLiteProjectInfo.IsProtected.GetValueOrDefault(false))
                                {
                                    var sequenceNumbers = SequenceNumberImportInfoUtilities.Load(openFileDialog.FileName);
                                    sequenceNumbers.LoadTargetReferences(mainForm.Project.References);

                                    if (sequenceNumbers.Count != 0)
                                    {
                                        using (var chooseTargetFieldDialog = new ChoosePropertyIdForm(mainForm))
                                        {
                                            if (chooseTargetFieldDialog.ShowDialog(e.Form) == DialogResult.OK)
                                            {
                                                sequenceNumbers.StoreTargetReferences(chooseTargetFieldDialog.SelectedPropertyId);

                                                var successCount = sequenceNumbers.GetSuccesImportCount();
                                                switch (successCount)
                                                {
                                                    case 0:
                                                        MessageBox.Show(mainForm, Properties.Resources.CompleteZeroResultMessage, Properties.Resources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        break;
                                                    case 1:
                                                        MessageBox.Show(mainForm, Properties.Resources.CompleteSingleResultMessage, Properties.Resources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        break;
                                                    default:
                                                        MessageBox.Show(mainForm, Properties.Resources.CompleteMultipleResultsMessage.FormatString(successCount), Properties.Resources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(mainForm, Properties.Resources.FoundReferenceCountNull, Properties.Resources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(mainForm, Properties.Resources.OpenProtectedProjectMessage, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show(mainForm, Properties.Resources.OpenProjectConfigurationException, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                }
                else
                {
                    MessageBox.Show(mainForm, Properties.Resources.OnlyDesktopProjectsSupport, Properties.Resources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                   .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                   .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.FileThisProject)
                                   .GetCommandbarButton(ButtonKey);
            if (button != null)
            {
                button.Text = Properties.Resources.MenuCaption;
            }
        }
    }
}
