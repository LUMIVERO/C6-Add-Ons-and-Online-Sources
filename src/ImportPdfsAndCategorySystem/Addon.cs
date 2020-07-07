using SwissAcademic.Addons.ImportPdfsAndCategorySystemAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportPdfsAndCategorySystemAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {
        public override async void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case Key_Button_File:
                    {
                        if (ChooseDirectory(mainForm, out string directory))
                        {
                            await DirectoryImporter.RunAsync(mainForm, directory);
                        }
                    }
                    break;
                case Key_Button_References:
                    {
                        if (ChooseDirectory(mainForm, out string directory))
                        {
                            await DirectoryImporter.RunAsync(mainForm, directory);
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
            var commandBar = mainForm.GetMainCommandbarManager()
                                     .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu);
            commandBar.GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                      .InsertCommandbarButton(6, Key_Button_File, Resource.AddonCommandbarButton, image: Resource.addon);

            commandBar.GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                      .InsertCommandbarButton(3, Key_Button_References, Resource.AddonCommandbarButton, image: Resource.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var menu = mainForm.GetMainCommandbarManager()
                               .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu);

            var button = menu?.GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File).GetCommandbarButton(Key_Button_File);

            if (button != null)
            {
                button.Text = Resource.AddonCommandbarButton;
            }

            button = menu?.GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                          .GetCommandbarButton(Key_Button_References);

            if (button != null)
            {
                button.Text = Resource.AddonCommandbarButton;
            }
        }

        bool ChooseDirectory(Form form, out string directory)
        {
            using (var folderDialog = new FolderBrowserDialog { Description = Resource.FolderBrowserDialogDescription })
            {
                if (folderDialog.ShowDialog(form) == DialogResult.OK)
                {
                    directory = folderDialog.SelectedPath;
                    return true;
                }
            }

            directory = null;
            return false;
        }
    }
}