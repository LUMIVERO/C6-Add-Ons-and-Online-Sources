using SwissAcademic.Addons.ExportAttachmentsToCategoryFolderStructureAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ExportAttachmentsToCategoryFolderStructureAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {
        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(ButtonKey, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;

                var previews = Previews.Instance;

                try
                {
                    previews.Close();

                    if (ChooseDirectory(out string exportPath))
                    {
                        Macro.Run(mainForm, exportPath);
                    }
                }
                finally
                {
                    previews.Open();
                }
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            mainForm.GetMainCommandbarManager()
                    .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                    .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                    .InsertCommandbarButton(4, ButtonKey, Resources.Button_Text, image: Resources.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm.GetMainCommandbarManager()
                                 .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                 .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                                 .GetCommandbarButton(ButtonKey);
            if (button != null)
            {
                button.Text = Resources.Button_Text;
            }
        }

        static bool ChooseDirectory(out string directory)
        {
            directory = null;

            using (var folderBrowserDialog = new FolderBrowserDialog { Description = Resources.Messages_SelectRootFolder, SelectedPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) })
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    directory = folderBrowserDialog.SelectedPath;
                    return true;
                }
            }

            return false;
        }
    }
}