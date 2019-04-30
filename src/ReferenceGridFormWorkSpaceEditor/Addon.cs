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
        const string Key_Settings = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Settings";

        #endregion

        #region Fields

        CommandbarMenu _menu;
        AddonSettings _settings;

        #endregion

        #region Methods

        public override void OnBeforePerformingCommand(ReferenceGridForm referenceGridForm, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                case Key_Button_Edit_Addon:
                    EditWorkSpaces(referenceGridForm);
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
            _settings = this.Settings[Key_Settings]?.Load();

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

        void CreateWorkSpace(ReferenceGridForm owner)
        {
            using (var form = new WorkSpaceNameEditor(owner))
            {
                if (form.ShowDialog() == DialogResult.Cancel) return;
                var workSpace = owner.CreateWorkSpaceByName(form.WorkSpaceName);
                _settings.WorkSpaces.Add(workSpace);
            }

            RefreshMenuItems();
        }

        void EditWorkSpaces(ReferenceGridForm owner)
        {
            using (var workSpaceEditor = new WorkSpaceEditor(owner, _settings))
            {
                workSpaceEditor.ShowDialog();
                _settings = workSpaceEditor.Settings;
            }

            this.Settings[Key_Settings] = _settings.ToJson();

            RefreshMenuItems();
        }

        void RefreshMenuItems()
        {

        }

        #endregion
    }
}