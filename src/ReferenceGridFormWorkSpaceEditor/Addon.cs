using SwissAcademic;
using SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor
{
    public class Addon : CitaviAddOn<ReferenceGridForm>
    {
        #region Constants

        const string Key_Menu_Addon = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.MenuKey";
        const string Key_Button_Edit_Addon = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.EditButtonKey";
        const string Key_Button_Create_Addon = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.CreateButtonKey";

        #endregion

        #region Fields

        CommandbarMenu _menu;

        #endregion

        #region Methods

        public override void OnBeforePerformingCommand(ReferenceGridForm referenceGridForm, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case Key_Button_Edit_Addon:
                    break;
                case Key_Button_Create_Addon:
                    CreateWorkSpace(referenceGridForm);
                    break;
                default:
                    e.Handled = false;
                    break;
            }

            base.OnBeforePerformingCommand(referenceGridForm, e);
        }

        public override void OnHostingFormLoaded(ReferenceGridForm referenceGridForm)
        {
            //var viewMenu = referenceGridForm
            //               .GetCommandbar(ReferenceGridFormCommandbarId.Menu)
            //               .GetCommandbarMenu(ReferenceGridFormCommandbarMenuId.View);

            // TODO mit neuer Nightly ausbauen
            var viewMenu = Reflection2
                           .CreateInstanceOf<ReferenceGridFormCommandbar>(referenceGridForm.MainToolbarsManager.Toolbars["MainMenu"])
                           .GetCommandbarMenu(ReferenceGridFormCommandbarMenuId.View);



            _menu = viewMenu
                    .InsertCommandbarMenu(viewMenu.Tool.Tools.Count - 1, Key_Menu_Addon, ReferenceGridFormWorkSpaceEditorResources.Menu_Caption);
            _menu.HasSeparator = true;
            _menu.AddCommandbarButton(Key_Button_Create_Addon, ReferenceGridFormWorkSpaceEditorResources.Button_CreateWorkSpace);
            _menu.AddCommandbarButton(Key_Button_Edit_Addon, ReferenceGridFormWorkSpaceEditorResources.Button_EditWorkSpaces);

            base.OnHostingFormLoaded(referenceGridForm);
        }

        public override void OnLocalizing(ReferenceGridForm referenceGridForm)
        {
            if (_menu != null) _menu.Text = ReferenceGridFormWorkSpaceEditorResources.Menu_Caption;

            base.OnLocalizing(referenceGridForm);
        }

        void CreateWorkSpace(Form owner)
        {
            using (var form = new WorkSpaceNameEditor(owner))
            {
                if (form.ShowDialog() == DialogResult.Cancel) return;
            }
        }

        #endregion
    }
}