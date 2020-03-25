using SwissAcademic.Addons.ImportPdfsAndCategorySystem.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;

namespace SwissAcademic.Addons.ImportPdfsAndCategorySystem
{
    public class Addon : CitaviAddOn<MainForm>
    {
        #region Constants

        const string Key_Button_File = "SwissAcademic.Addons.ImportPdfsAndCategorySystem.CommandbarButtonFile";
        const string Key_Button_References = "SwissAcademic.Addons.ImportPdfsAndCategorySystem.CommandbarButtonReferences";

        #endregion

        #region Methods

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case (Key_Button_File):
                    {
                        Macro.Run(mainForm);
                    }
                    break;
                case (Key_Button_References):
                    {
                        Macro.Run(mainForm);
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
            var button = mainForm.GetMainCommandbarManager()
                                      .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                      .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                                      .GetCommandbarButton(Key_Button_File);

            if (button != null) button.Text = Resource.AddonCommandbarButton;

            button = mainForm.GetMainCommandbarManager()
                             .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                             .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                             .GetCommandbarButton(Key_Button_References);

            if (button != null) button.Text = Resource.AddonCommandbarButton;
        }

        #endregion
    }
}