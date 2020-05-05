﻿using SwissAcademic.Addons.C6ToC5Export.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.C6ToC5Export
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_Export = "SwissAcademic.Addons.C6ToC5Export.ExportButtonCommand";

        #endregion

        #region Methods

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case (Key_Button_Export):
                    {
                        if (mainForm.Project.ProjectType == ProjectType.DesktopCloud && mainForm.Project.AllLocations.HasFileLocations())
                        {
                            using (var messageBox = new CitaviMessageBox(mainForm))
                            {
                                messageBox.DescriptionEditor.DetectHiddenLinks = true;
                                messageBox.DescriptionTagged = Resources.ExportCloudProjectErrorMessage;
                                messageBox.Icon = MessageBoxIcon.Error;
                                messageBox.CancelledButton.Visible = false;
                                messageBox.ShowDialog(mainForm);
                            }
                        }
                        else
                        {
                            using (var saveFileDialog = new SaveFileDialog { Filter = Resources.ProjectFilters, CheckPathExists = true, Title = Resources.ExportTitle })
                            {
                                if (saveFileDialog.ShowDialog(e.Form) != DialogResult.OK) return;

                                try
                                {
                                    mainForm.Project.SaveAsXml(saveFileDialog.FileName, ProjectXmlExportCompatibility.Citavi5);
                                    MessageBox.Show(e.Form, Resources.ExportFinallyMessage, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                catch (Exception ex)
                                {
                                    if (MessageBox.Show(e.Form, Resources.ExportExceptionMessage.FormatString(ex.Message), mainForm.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                                    {
                                        Clipboard.SetText(e.ToString());
                                    }
                                }
                                break;
                            }
                        }
                    }
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm.GetMainCommandbarManager()
                    .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                    .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                    .GetCommandbarMenu("ThisProject")
                    .InsertCommandbarButton(3, Key_Button_Export, Resources.ExportCitaviButtonText, image: Resources.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                 .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                 .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                                 .GetCommandbarMenu("ThisProject")
                                 .GetCommandbarButton(Key_Button_Export);

            if (button != null)
            {
                button.Text = Resources.ExportCitaviButtonText;
            }
        }

        #endregion
    }
}