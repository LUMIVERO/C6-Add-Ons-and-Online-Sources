using SwissAcademic.Addons.ImportPdfsAndCategorySystem.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportPdfsAndCategorySystem
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_CommandbarButtonFile = "SwissAcademic.Addons.ImportPdfsAndCategorySystem.CommandbarButtonFile";
        const string Key_CommandbarButtonReferences = "SwissAcademic.Addons.ImportPdfsAndCategorySystem.CommandbarButtonReferences";

        #endregion

        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Form is MainForm mainForm)
            {
                e.Handled = true;
                switch (e.Key)
                {
                    case (Key_CommandbarButtonFile):
                        {
                            Macro.Run(mainForm);
                        }
                        break;
                    case (Key_CommandbarButtonReferences):
                        {
                            Macro.Run(mainForm);
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
                        .InsertCommandbarButton(6, Key_CommandbarButtonFile, ImportPdfsAndCategorySystemResource.AddonCommandbarButton, image: ImportPdfsAndCategorySystemResource.addon);

                mainForm.GetMainCommandbarManager()
                        .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                        .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                        .InsertCommandbarButton(3, Key_CommandbarButtonReferences, ImportPdfsAndCategorySystemResource.AddonCommandbarButton, image: ImportPdfsAndCategorySystemResource.addon);
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
                                     .GetCommandbarButton(Key_CommandbarButtonFile);

                if (button != null) button.Text = ImportPdfsAndCategorySystemResource.AddonCommandbarButton;

                button = mainForm.GetMainCommandbarManager()
                                 .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                                 .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                                 .GetCommandbarButton(Key_CommandbarButtonReferences);

                if (button != null) button.Text = ImportPdfsAndCategorySystemResource.AddonCommandbarButton;
            }

            base.OnLocalizing(form);
        }

        #endregion
    }
}