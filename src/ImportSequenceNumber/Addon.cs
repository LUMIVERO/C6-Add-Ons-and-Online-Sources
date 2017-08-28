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
                                     .AddCommandbarButton(AddonKeys.CommandbarButton, ImportSequenceNumberResources.MenuCaption);
                if (button != null) button.HasSeparator = true;
            }

            base.OnHostingFormLoaded(form);
        }

        async protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Form is MainForm mainForm && e.Key.Equals(AddonKeys.CommandbarButton, StringComparison.OrdinalIgnoreCase))
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

                        if (openFileDialog.ShowDialog(e.Form) == DialogResult.OK)
                        {
                            try
                            {
                                var projectConfiguration = await DesktopProjectConfiguration.OpenAsync(SwissAcademic.Citavi.Shell.Program.Engine, openFileDialog.FileName);

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
                                                        MessageBox.Show(e.Form, ImportSequenceNumberResources.CompleteZeroResultMessage, ImportSequenceNumberResources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        break;
                                                    case 1:
                                                        MessageBox.Show(e.Form, ImportSequenceNumberResources.CompleteSingleResultMessage, ImportSequenceNumberResources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        break;
                                                    default:
                                                        MessageBox.Show(e.Form, ImportSequenceNumberResources.CompleteMultipleResultsMessage.FormatString(successCount), ImportSequenceNumberResources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(e.Form, ImportSequenceNumberResources.FoundReferenceCountNull, ImportSequenceNumberResources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(e.Form, ImportSequenceNumberResources.OpenProtectedProjectMessage, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show(e.Form, ImportSequenceNumberResources.OpenProjectConfigurationException, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                }
                else
                {
                    MessageBox.Show(e.Form, ImportSequenceNumberResources.OnlyDesktopProjectsSupport, ImportSequenceNumberResources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                                     .GetCommandbarButton(AddonKeys.CommandbarButton);
                if (button != null) button.Text = ImportSequenceNumberResources.MenuCaption;
            }

            base.OnLocalizing(form);
        }

        #endregion
    }
}
