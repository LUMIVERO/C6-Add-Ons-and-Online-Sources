using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditorAddon
{
    public class Addon : CitaviAddOn<ReferenceGridForm>
    {
        #region Constants

        const string Key_Menu = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.{0}.Menu";
        const string Key_Button_Edit = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.{0}.EditButtonKey";
        const string Key_Button_Create = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.{0}.CreateButtonKey";
        const string Key_Settings = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Settings";
        const string Key_Button_WorkSpaces = "SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Buttons.{0}.{1}";

        #endregion

        #region Fields

        AddonSettings _settings;
        Dictionary<ReferenceGridForm, CommandbarMenu> _menus;

        #endregion

        #region Constructors

        public Addon()
        {
            _menus = new Dictionary<ReferenceGridForm, CommandbarMenu>();
        }

        #endregion

        #region Methods

        public override void OnBeforePerformingCommand(ReferenceGridForm referenceGridForm, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;

            if (e.Key.Equals(Key_Button_Edit.FormatString(referenceGridForm.Id.ToString()), StringComparison.Ordinal))
            {
                EditWorkSpaces(referenceGridForm);
            }
            else if (e.Key.Equals(Key_Button_Create.FormatString(referenceGridForm.Id.ToString()), StringComparison.Ordinal))
            {
                CreateWorkSpace(referenceGridForm);
            }
            else
            {
                if (_settings.WorkSpaces.FirstOrDefault(ws => e.Key.Equals(Key_Button_WorkSpaces.FormatString(referenceGridForm.Id.ToString(), ws.Id), StringComparison.Ordinal)) is WorkSpace workSpace)
                {
                    referenceGridForm.LoadWorkSpace(workSpace);
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        public override void OnHostingFormLoaded(ReferenceGridForm referenceGridForm)
        {
            if (_settings == null)
            {
                this.Settings.TryGetValue(Key_Settings, out string json);
                _settings = json.Load();
            }


            if (!_menus.ContainsKey(referenceGridForm))
            {
                var viewMenu = referenceGridForm
                           .GetCommandbar(ReferenceGridFormCommandbarId.Menu)
                           .GetCommandbarMenu(ReferenceGridFormCommandbarMenuId.View);

                var menu = viewMenu.InsertCommandbarMenu(viewMenu.Tool.Tools.Count - 1, Key_Menu.FormatString(referenceGridForm.Id.ToString()), Properties.Resources.Menu_Caption);
                menu.HasSeparator = true;
                _menus.Add(referenceGridForm, menu);
                referenceGridForm.FormClosed += ReferenceGridForm_FormClosed;
            }


            RefreshMenuItems();
        }

        public override void OnLocalizing(ReferenceGridForm referenceGridForm)
        {
            if (_menus.ContainsKey(referenceGridForm))
            {
                var menu = _menus[referenceGridForm];
                menu.Text = Properties.Resources.Menu_Caption;
                ((ButtonTool)menu.Tool.Tools[menu.Tool.Tools.Count - 2]).SharedProps.Caption = Properties.Resources.Button_CreateWorkSpace;
                ((ButtonTool)menu.Tool.Tools[menu.Tool.Tools.Count - 1]).SharedProps.Caption = Properties.Resources.Button_EditWorkSpaces;
            }
        }

        void CreateWorkSpace(ReferenceGridForm referenceGridForm)
        {
            var captions = _settings.WorkSpaces.Select(ws => ws.Caption).ToList();
            using (var form = new WorkSpaceNameEditor(referenceGridForm, captions))
            {
                if (form.ShowDialog(referenceGridForm) == DialogResult.Cancel) return;
                var workSpace = referenceGridForm.CreateWorkSpaceByName(form.WorkSpaceName);
                _settings.WorkSpaces.Add(workSpace);
                this.Settings[Key_Settings] = _settings.ToJson();
            }

            RefreshMenuItems();
        }

        void EditWorkSpaces(ReferenceGridForm referenceGridForm)
        {
            using (var workSpaceEditor = new WorkSpaceEditor(referenceGridForm, _settings))
            {
                workSpaceEditor.ShowDialog();
                _settings = workSpaceEditor.Settings;
            }

            this.Settings[Key_Settings] = _settings.ToJson();

            RefreshMenuItems();
        }



        void RefreshMenuItems()
        {
            foreach (var menuPairs in _menus)
            {
                var referenceGridForm = menuPairs.Key;
                var menu = menuPairs.Value;
                if (menu != null)
                {
                    for (int index = menu.Tool.Tools.Count - 1; index >= 0; index--)
                    {
                        using (var tool = menu.Tool.Tools[index])
                        {
                            menu.Tool.Tools.Remove(tool);
                            menu.Tool.ToolbarsManager.Tools.Remove(tool);
                        }
                    }

                    foreach (var workSpace in _settings.WorkSpaces)
                    {
                        menu.AddCommandbarButton(Key_Button_WorkSpaces.FormatString(referenceGridForm.Id.ToString(), workSpace.Id), workSpace.Caption);
                    }


                    var button = menu.AddCommandbarButton(Key_Button_Create.FormatString(referenceGridForm.Id.ToString()), Properties.Resources.Button_CreateWorkSpace);
                    button.HasSeparator = true;
                    menu.AddCommandbarButton(Key_Button_Edit.FormatString(referenceGridForm.Id.ToString()), Properties.Resources.Button_EditWorkSpaces);
                }
            }
        }

        #endregion

        #region Events

        void ReferenceGridForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is ReferenceGridForm referenceGridForm)
            {
                if (_menus.ContainsKey(referenceGridForm))
                {
                    _menus.Remove(referenceGridForm);
                }
                referenceGridForm.FormClosed -= ReferenceGridForm_FormClosed;
            }
        }

        #endregion
    }
}