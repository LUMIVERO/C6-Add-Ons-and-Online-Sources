using SwissAcademic.Addons.ImportPdfsAndCategorySystem.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Drawing;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportPdfsAndCategorySystem
{
    public class Addon : CitaviAddOn
    {
        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            var mainForm = e.Form as MainForm;

            if (mainForm != null && e.Key.Equals(AddonKeys.CommandbarButtonFile, StringComparison.OrdinalIgnoreCase))
            {
                ImportPdfsAndCategorySystemMacro.Run(mainForm);
                e.Handled = true;
            }
            else if (mainForm != null && e.Key.Equals(AddonKeys.CommandbarButtonReferences, StringComparison.OrdinalIgnoreCase))
            {
                ImportPdfsAndCategorySystemMacro.Run(mainForm);
                e.Handled = true;
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
                    .InsertCommandbarButton(6, AddonKeys.CommandbarButtonFile, ImportPdfsAndCategorySystemResource.AddonCommandbarButton, image: ImportPdfsAndCategorySystemResource.addon);

                mainForm.GetMainCommandbarManager()
                  .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                  .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                  .InsertCommandbarButton(3, AddonKeys.CommandbarButtonReferences, ImportPdfsAndCategorySystemResource.AddonCommandbarButton, image: ImportPdfsAndCategorySystemResource.addon);
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
                    .GetCommandbarButton(AddonKeys.CommandbarButtonFile);

                if (button != null) button.Text = ImportPdfsAndCategorySystemResource.AddonCommandbarButton;

                button = mainForm.GetMainCommandbarManager()
                    .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                    .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.File)
                    .GetCommandbarButton(AddonKeys.CommandbarButtonReferences);

                if (button != null) button.Text = ImportPdfsAndCategorySystemResource.AddonCommandbarButton;
            }

            base.OnLocalizing(form);
        }

        #endregion
    }
}