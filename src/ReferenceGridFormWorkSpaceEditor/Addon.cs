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

        const string Key_Menu = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.MenuKey";
        const string Key_Button_Edit = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.EditButtonKey";
        const string Key_Button_Create = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.CreateButtonKey";
        const string Key_Settings = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Settings";
        const string Key_Button_WorkSpaces = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Buttons.{0}";

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
                case Key_Button_Edit:
                    EditWorkSpaces(referenceGridForm);
                    break;
                case Key_Button_Create:
                    CreateWorkSpace(referenceGridForm);
                    break;
                default:

                    if (_settings.WorkSpaces.FirstOrDefault(ws =>e.Key.Equals(Key_Button_WorkSpaces.FormatString(ws.Id), StringComparison.Ordinal)) is WorkSpace workSpace)
                    {
                        LoadWorkSpace(referenceGridForm, workSpace);
                    }
                    else
                    {
                        e.Handled = false;
                    }
                    break;
            }

            base.OnBeforePerformingCommand(referenceGridForm, e);
        }

        public override void OnHostingFormLoaded(ReferenceGridForm referenceGridForm)
        {
            if (_settings == null)
            {
                this.Settings.TryGetValue(Key_Settings, out string json);
                _settings = json.Load();
            }


            //var viewMenu = referenceGridForm
            //               .GetCommandbar(ReferenceGridFormCommandbarId.Menu)
            //               .GetCommandbarMenu(ReferenceGridFormCommandbarMenuId.View);

            // TODO mit neuer Nightly ausbauen
            var viewMenu = Reflection2
                           .CreateInstanceOf<ReferenceGridFormCommandbar>(referenceGridForm.MainToolbarsManager.Toolbars["MainMenu"])
                           .GetCommandbarMenu(ReferenceGridFormCommandbarMenuId.View);



            _menu = viewMenu.InsertCommandbarMenu(viewMenu.Tool.Tools.Count - 1, Key_Menu, ReferenceGridFormWorkSpaceEditorResources.Menu_Caption);
            _menu.HasSeparator = true;

            RefreshMenuItems();

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
                this.Settings[Key_Settings] = _settings.ToJson();
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


        void LoadWorkSpace(ReferenceGridForm referenceGridForm, WorkSpace workSpace)
        {
            Program.Settings.ReferenceGridForm.AllowUpdate = workSpace.AllowUpdate;
            Program.Settings.ReferenceGridForm.GroupByBoxVisible = workSpace.GroupByBoxVisible;
            Program.Settings.ReferenceGridForm.ColumnDescriptors.Clear();
            workSpace.Columns.ForEach(cl => Program.Settings.ReferenceGridForm.ColumnDescriptors.Add(cl));
            referenceGridForm.Invoke("mainGrid_InitializeLayout ", referenceGridForm.Field<ReferenceGridForm, UltraGrid>("mainGrid"), EventArgs.Empty);
        }

        void RefreshMenuItems()
        {
            if (_menu != null)
            {
                for (int index = _menu.Tool.Tools.Count - 1; index >= 0; index--)
                {
                    using (var tool = _menu.Tool.Tools[index])
                    {
                        _menu.Tool.Tools.RemoveAt(index);
                    }
                }

                foreach (var workSpace in _settings.WorkSpaces)
                {
                    _menu.AddCommandbarButton(Key_Button_WorkSpaces.FormatString(workSpace.Id), workSpace.Caption);
                }


                var button = _menu.AddCommandbarButton(Key_Button_Create, ReferenceGridFormWorkSpaceEditorResources.Button_CreateWorkSpace);
                button.HasSeparator = true;
                _menu.AddCommandbarButton(Key_Button_Edit, ReferenceGridFormWorkSpaceEditorResources.Button_EditWorkSpaces);
            }
        }

        #endregion
    }
}