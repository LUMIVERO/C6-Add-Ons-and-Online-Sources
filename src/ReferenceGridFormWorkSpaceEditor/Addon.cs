using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditorAddon
{
    public partial class Addon : CitaviAddOn<ReferenceGridForm>
    {
        // Fields

        AddonSettings _settings;
        readonly Dictionary<ReferenceGridForm, CommandbarMenu> _menus = new Dictionary<ReferenceGridForm, CommandbarMenu>();

        // Methods

        public override void OnBeforePerformingCommand(ReferenceGridForm referenceGridForm, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;

            if (e.Key.Equals(ButtonKey_Edit.FormatString(referenceGridForm.Id.ToString()), StringComparison.Ordinal))
            {
                EditWorkSpaces(referenceGridForm);
            }
            else if (e.Key.Equals(ButtonKey_Create.FormatString(referenceGridForm.Id.ToString()), StringComparison.Ordinal))
            {
                CreateWorkSpace(referenceGridForm);
            }
            else
            {
                if (_settings.WorkSpaces.FirstOrDefault(ws => e.Key.Equals(ButtonKey.FormatString(referenceGridForm.Id.ToString(), ws.Id), StringComparison.Ordinal)) is WorkSpace workSpace)
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
                Settings.TryGetValue(SettingsKey, out string json);
                _settings = json.Load();
            }


            if (!_menus.ContainsKey(referenceGridForm))
            {
                var viewMenu = referenceGridForm
                           .GetCommandbar(ReferenceGridFormCommandbarId.Menu)
                           .GetCommandbarMenu(ReferenceGridFormCommandbarMenuId.View);

                var menu = viewMenu.InsertCommandbarMenu(viewMenu.Tool.Tools.Count - 1, MenuKey.FormatString(referenceGridForm.Id.ToString()), Properties.Resources.Menu_Caption);
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
            using (var form = new WorkSpaceNameEditorForm(referenceGridForm, captions))
            {
                if (form.ShowDialog(referenceGridForm) == DialogResult.Cancel) return;
                var workSpace = referenceGridForm.CreateWorkSpaceByName(form.WorkSpaceName);
                _settings.WorkSpaces.Add(workSpace);
                Settings[SettingsKey] = _settings.ToJson();
            }

            RefreshMenuItems();
        }

        void EditWorkSpaces(ReferenceGridForm referenceGridForm)
        {
            using (var workSpaceEditor = new WorkSpaceEditorForm(referenceGridForm, _settings))
            {
                workSpaceEditor.ShowDialog();
                _settings = workSpaceEditor.Settings;
            }

            Settings[SettingsKey] = _settings.ToJson();

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
                        menu.AddCommandbarButton(ButtonKey.FormatString(referenceGridForm.Id.ToString(), workSpace.Id), workSpace.Caption);
                    }


                    var button = menu.AddCommandbarButton(ButtonKey_Create.FormatString(referenceGridForm.Id.ToString()), Properties.Resources.Button_CreateWorkSpace);
                    button.HasSeparator = true;
                    menu.AddCommandbarButton(ButtonKey_Edit.FormatString(referenceGridForm.Id.ToString()), Properties.Resources.Button_EditWorkSpaces);
                }
            }
        }

        // Events

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
    }
}