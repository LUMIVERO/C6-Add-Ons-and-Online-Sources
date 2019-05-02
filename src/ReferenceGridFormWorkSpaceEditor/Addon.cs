using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
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
                    LoadWorkSpace(referenceGridForm, workSpace);
                }
                else
                {
                    e.Handled = false;
                }
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

            if (!_menus.ContainsKey(referenceGridForm))
            {
                var menu = viewMenu.InsertCommandbarMenu(viewMenu.Tool.Tools.Count - 1, Key_Menu.FormatString(referenceGridForm.Id.ToString()), ReferenceGridFormWorkSpaceEditorResources.Menu_Caption);
                menu.HasSeparator = true;
                _menus.Add(referenceGridForm, menu);
                referenceGridForm.FormClosed += ReferenceGridForm_FormClosed;
            }


            RefreshMenuItems();

            base.OnHostingFormLoaded(referenceGridForm);
        }

        public override void OnLocalizing(ReferenceGridForm referenceGridForm)
        {
            if (_menus.ContainsKey(referenceGridForm))
            {
                var menu = _menus[referenceGridForm];
                menu.Text = ReferenceGridFormWorkSpaceEditorResources.Menu_Caption;
                ((ButtonTool)menu.Tool.Tools[menu.Tool.Tools.Count - 2]).SharedProps.Caption = ReferenceGridFormWorkSpaceEditorResources.Button_CreateWorkSpace;
                ((ButtonTool)menu.Tool.Tools[menu.Tool.Tools.Count - 1]).SharedProps.Caption = ReferenceGridFormWorkSpaceEditorResources.Button_EditWorkSpaces;
            }

            base.OnLocalizing(referenceGridForm);
        }

        void CreateWorkSpace(ReferenceGridForm referenceGridForm)
        {
            using (var form = new WorkSpaceNameEditor(referenceGridForm))
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


        void LoadWorkSpace(ReferenceGridForm referenceGridForm, WorkSpace workSpace)
        {
            Program.Settings.ReferenceGridForm.AllowUpdate = workSpace.AllowUpdate;
            Program.Settings.ReferenceGridForm.GroupByBoxVisible = workSpace.GroupByBoxVisible;
            Program.Settings.ReferenceGridForm.ColumnDescriptors.Clear();
            workSpace.Columns.ForEach(cl => Program.Settings.ReferenceGridForm.ColumnDescriptors.Add(cl));
            referenceGridForm.Invoke("mainGrid_InitializeLayout", new object(), new InitializeLayoutEventArgs(new UltraGridLayout()));
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


                    var button = menu.AddCommandbarButton(Key_Button_Create.FormatString(referenceGridForm.Id.ToString()), ReferenceGridFormWorkSpaceEditorResources.Button_CreateWorkSpace);
                    button.HasSeparator = true;
                    menu.AddCommandbarButton(Key_Button_Edit.FormatString(referenceGridForm.Id.ToString()), ReferenceGridFormWorkSpaceEditorResources.Button_EditWorkSpaces);
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