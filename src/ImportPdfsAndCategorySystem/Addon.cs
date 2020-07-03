using SwissAcademic.Addons.ImportPdfsAndCategorySystemAddon.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;

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
                        await Macro.Run(mainForm);
                    }
                    break;
                case Key_Button_References:
                    {
                        await Macro.Run(mainForm);
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
    }
}