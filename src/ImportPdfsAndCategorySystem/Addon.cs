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
                case ButtonKey_Files:
                    {
                        if (ChooseDirectory(mainForm, out string directory))
                        {
                            await DirectoryImporter.RunAsync(mainForm, directory);
                        }
                    }
                    break;
                case ButtonKey_References:
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
            var commandBar = mainForm
                                .GetMainCommandbarManager()
                                .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu);
            commandBar
                .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                .InsertCommandbarButton(6, ButtonKey_Files, Resource.AddonCommandbarButton, image: Resource.addon);

            commandBar
                .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                .InsertCommandbarButton(3, ButtonKey_References, Resource.AddonCommandbarButton, image: Resource.addon);
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var menu = mainForm
                           .GetMainCommandbarManager()
                           .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu);

            var button = menu?
                            .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                            .GetCommandbarButton(ButtonKey_Files);

            if (button != null)
            {
                button.Text = Resource.AddonCommandbarButton;
            }

            button = menu?
                       .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                       .GetCommandbarButton(ButtonKey_References);

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