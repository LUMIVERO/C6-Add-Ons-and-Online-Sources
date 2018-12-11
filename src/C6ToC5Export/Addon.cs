using SwissAcademic.Addons.C6ToC5Export.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;
using System.Linq;

namespace SwissAcademic.Addons.C6ToC5Export
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_Button_Export = "SwissAcademic.Addons.C6ToC5Export.ExportButtonCommand";

        #endregion

        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;

            if (e.Form is MainForm mainForm)
            {
                switch (e.Key)
                {
                    case (Key_Button_Export):
                        {
                            if (mainForm.Project.ProjectType == ProjectType.DesktopCloud && mainForm.Project.AllLocations.HasFileLocations())
                            {
                                using (var messageBox = new CitaviMessageBox(mainForm))
                                {
                                    messageBox.DescriptionEditor.DetectHiddenLinks = true;
                                    messageBox.DescriptionTagged = C6ToC5ExportResources.ExportCloudProjectErrorMessage;
                                    messageBox.Icon = MessageBoxIcon.Error;
                                    messageBox.CancelledButton.Visible = false;
                                    messageBox.ShowDialog(mainForm);
                                }
                            }
                            else
                            {
                                using (var saveFileDialog = new SaveFileDialog { Filter = C6ToC5ExportResources.ProjectFilters, CheckPathExists = true, Title = C6ToC5ExportResources.ExportTitle })
                                {
                                    if (saveFileDialog.ShowDialog(e.Form) == DialogResult.OK)
                                    {
                                        try
                                        {
                                            mainForm.Project.SaveAsXml(saveFileDialog.FileName, ProjectXmlExportCompatibility.Citavi5);
                                            MessageBox.Show(e.Form, C6ToC5ExportResources.ExportFinallyMessage, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        catch (Exception ex)
                                        {
                                            if (MessageBox.Show(e.Form, C6ToC5ExportResources.ExportExceptionMessage.FormatString(ex.Message), mainForm.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                                            {
                                                Clipboard.SetText(e.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }

            base.OnBeforePerformingCommand(e);
        }

        protected override void OnHostingFormLoaded(Form form)
        {
            if (form is MainForm mainForm)
            {
                mainForm.GetMainCommandbarManager()
                        .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                        .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                        .GetCommandbarMenu("ThisProject")
                        .InsertCommandbarButton(3, Key_Button_Export, C6ToC5ExportResources.ExportCitaviButtonText, image: C6ToC5ExportResources.addon);
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            if (form is MainForm mainForm)
            {
                var button = mainForm.GetMainCommandbarManager()
                                     .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                     .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                                     .GetCommandbarMenu("ThisProject")
                                     .GetCommandbarButton(Key_Button_Export);

                if (button != null) button.Text = C6ToC5ExportResources.ExportCitaviButtonText;
            }

            base.OnLocalizing(form);
        }

        #endregion
    }

    public static class Extensions
    {
        public static bool HasFileLocations(this ProjectAllLocationsCollection locations)
        {
            return (from location in locations
                    where
                       location.LocationType == LocationType.ElectronicAddress &&
                       ((location.Address.LinkedResourceType == LinkedResourceType.AttachmentRemote) ||
                       (
                           location.Address.LinkedResourceType == LinkedResourceType.AttachmentFile ||
                           location.Address.LinkedResourceType == LinkedResourceType.AbsoluteFileUri ||
                           location.Address.LinkedResourceType == LinkedResourceType.RelativeFileUri
                       ))
                    select location).Count() > 0;
        }
    }
}