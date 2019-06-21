using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Addons.ExportAttachmentsToCategoryFolderStructure.Properties;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ExportAttachmentsToCategoryFolderStructure
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_ExportAttachmentsToCategoryFolderStructure = "SwissAcademic.Addons.ExportAttachmentsToCategoryFolderStructure.Button";

        #endregion

        #region Methods

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(Key_Button_ExportAttachmentsToCategoryFolderStructure, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;

                var previews = Previews.Create();

                try
                {
                    previews.Close();

                    if (AskForExportPath(out string exportPath))
                    {
                        Macro.Run(mainForm, exportPath);
                    }
                }
                finally
                {
                    previews.Open();
                }
            }

            base.OnBeforePerformingCommand(mainForm, e);
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm.GetMainCommandbarManager()
                       .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                       .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                       .InsertCommandbarButton(4, Key_Button_ExportAttachmentsToCategoryFolderStructure, ExportAttachmentsToCategoryFolderStructureResources.Button_Text, image: ExportAttachmentsToCategoryFolderStructureResources.addon);

            base.OnHostingFormLoaded(mainForm);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                    .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                    .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                    .GetCommandbarButton(Key_Button_ExportAttachmentsToCategoryFolderStructure);
            if (button != null) button.Text = ExportAttachmentsToCategoryFolderStructureResources.Button_Text;

            base.OnLocalizing(mainForm);
        }

        public bool AskForExportPath(out string exportPath)
        {
            exportPath = null;
            using (var fodlerBrowserDialog = new FolderBrowserDialog { Description = ExportAttachmentsToCategoryFolderStructureResources.Messages_SelectRootFolder, SelectedPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) })
            {
                if (fodlerBrowserDialog.ShowDialog() != DialogResult.OK) return false;

                exportPath = fodlerBrowserDialog.SelectedPath;
                return true;
            }
        }

        #endregion
    }
}