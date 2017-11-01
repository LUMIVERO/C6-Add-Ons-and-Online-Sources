using SwissAcademic.Addons.ImportSequenceNumber.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Resources;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportSequenceNumber
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_CommandbarButton = "SwissAcademic.Addons.ImportSequenceNumber.CommandbarButton";

        #endregion

        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnHostingFormLoaded(Form form)
        {
            if (form is MainForm mainForm)
            {
                var button = mainForm.GetMainCommandbarManager()
                                     .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                     .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.FileThisProject)
                                     .AddCommandbarButton(Key_CommandbarButton, ImportSequenceNumberResources.MenuCaption, image: ImportSequenceNumberResources.addon);
                if (button != null) button.HasSeparator = true;
            }

            base.OnHostingFormLoaded(form);
        }

        async protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Form is MainForm mainForm && e.Key.Equals(Key_CommandbarButton, StringComparison.OrdinalIgnoreCase))
            {
                if (mainForm.Project.ProjectType == ProjectType.DesktopSQLite)
                {
                    using (var openFileDialog = new OpenFileDialog()
                    {
                        Title = ImportSequenceNumberResources.OpenFileDialogTitle,
                        Filter = ImportSequenceNumberResources.OpenFileDialogFilters,
                        CheckFileExists = true,
                        CheckPathExists = true,
                        Multiselect = false
                    })

                        if (openFileDialog.ShowDialog(mainForm) == DialogResult.OK)
                        {
                            try
                            {
                                var projectConfiguration = await DesktopProjectConfiguration.OpenAsync(Program.Engine, openFileDialog.FileName);

                                if (projectConfiguration.SQLiteProjectInfo.IsProtected.GetValueOrDefault(false) == false)
                                {
                                    var sequenceNumbers = SequenceNumberImportInfoUtilities.Load(openFileDialog.FileName);
                                    sequenceNumbers.LoadTargetReferences(mainForm.Project.References);

                                    if (sequenceNumbers.Count != 0)
                                    {
                                        using (var chooseTargetFieldDialog = new ChoosePropertyIdDialog(mainForm.Project, sequenceNumbers) { Icon = e.Form.Icon })
                                        {
                                            if (chooseTargetFieldDialog.ShowDialog(e.Form) == DialogResult.OK)
                                            {
                                                sequenceNumbers.StoreTargetReferences(chooseTargetFieldDialog.SelectedPropertyId);

                                                var successCount = sequenceNumbers.GetSuccesImportCount();
                                                switch (successCount)
                                                {
                                                    case 0:
                                                        MessageBox.Show(mainForm, ImportSequenceNumberResources.CompleteZeroResultMessage, ImportSequenceNumberResources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        break;
                                                    case 1:
                                                        MessageBox.Show(mainForm, ImportSequenceNumberResources.CompleteSingleResultMessage, ImportSequenceNumberResources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        break;
                                                    default:
                                                        MessageBox.Show(mainForm, ImportSequenceNumberResources.CompleteMultipleResultsMessage.FormatString(successCount), ImportSequenceNumberResources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(mainForm, ImportSequenceNumberResources.FoundReferenceCountNull, ImportSequenceNumberResources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(mainForm, ImportSequenceNumberResources.OpenProtectedProjectMessage, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show(mainForm, ImportSequenceNumberResources.OpenProjectConfigurationException, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                }
                else
                {
                    MessageBox.Show(mainForm, ImportSequenceNumberResources.OnlyDesktopProjectsSupport, ImportSequenceNumberResources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                e.Handled = true;
            }
        }

        protected override void OnLocalizing(Form form)
        {
            if (form is MainForm mainForm)
            {
                var button = mainForm.GetMainCommandbarManager()
                                     .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                     .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.FileThisProject)
                                     .GetCommandbarButton(Key_CommandbarButton);
                if (button != null) button.Text = ImportSequenceNumberResources.MenuCaption;
            }

            base.OnLocalizing(form);
        }

        #endregion
    }
}
