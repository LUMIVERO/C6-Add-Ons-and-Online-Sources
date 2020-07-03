using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.MacroManagerAddon
{
    public partial class Addon : CitaviAddOn<MainForm>
    {
        #region Fields

        MacroEditorForm _editor;
        readonly List<MacroContainer> _containers;

        #endregion

        #region Constructors

        public Addon() => _containers = new List<MacroContainer>();

        #endregion

        #region Methods

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;

            var container = _containers.FirstOrDefault(c => c.Form.Equals(mainForm));

            switch (e.Key)
            {
                case Key_Button_ShowMacroEditor:
                    {
                        CurrentEditor(mainForm, false, out bool hidden, out bool isNew).Activate();
                    }
                    break;
                case Key_Button_Refresh:
                    {
                        UpdateTools(container);
                    }
                    break;
                case Key_Button_Config:
                    {

                        using (var directoryDialog = new DirectoryDialog(mainForm, Settings.TryGetStringValue(Key_MacrosDirectory)))
                        {
                            if (directoryDialog.ShowDialog() == DialogResult.OK)
                            {
                                Settings[Key_MacrosDirectory] = directoryDialog.Directory;
                                Program.Settings.InitialDirectories.SetInitialDirectoryContext(Citavi.Settings.InitialDirectoryContext.Macros, Path2.GetFullPathFromPathWithVariables(directoryDialog.Directory));
                                UpdateTools(container);
                            }
                        }
                    }
                    break;
                case Key_Button_OpenInExplorer:
                    {
                        if (IsValidDirectory(out string message))
                        {
                            var path = Path2.GetFullPathFromPathWithVariables(Settings[Key_MacrosDirectory]);
                            Process.Start("explorer.exe", path);
                        }
                        else
                        {
                            MessageBox.Show(e.Form, message, e.Form.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    break;
                default:

                    if (e.Key.StartsWith(Key_Button_Directory, StringComparison.OrdinalIgnoreCase))
                    {
                        if (container.Macros.ContainsKey(e.Key))
                        {
                            var macro = container.Macros[e.Key];

                            if (File.Exists(macro.Path))
                            {
                                var hide = macro.Action == MacroAction.Run;

                                _editor = CurrentEditor(e.Form, hide, out bool hidden, out bool isNew);

                                if (!isNew && _editor.IsDirty())
                                {
                                    if (MessageBox.Show(e.Form, Properties.Resources.UserWarningSaveMessage, e.Form.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                    {
                                        _editor.Save();
                                    }
                                }

                                _editor.MacroCode = File.ReadAllText(macro.Path);
                                _editor.SetFilePath(macro.Path);
                                _editor.Activate();

                                if (macro.Action == MacroAction.Run) _editor.Run();

                                if (hidden) _editor.Close();
                            }
                            else
                            {
                                MessageBox.Show(e.Form, Properties.Resources.PathNotFoundMessage.FormatString(macro.Path), e.Form.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                UpdateTools(container, true);
                            }

                        }
                        else
                        {
                            e.Handled = false;
                        }
                    }
                    else
                    {
                        e.Handled = false;
                    }
                    break;
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            var container = new MacroContainer(mainForm);
            mainForm.FormClosed += Form_FormClosed;
            _containers.Add(container);

            var toolsMenu = mainForm.GetMainCommandbarManager()?.GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)?.GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.Tools);
            var oldMacroEditorTool = toolsMenu.GetCommandbarButton("ShowMacroEditorForm");

            if (oldMacroEditorTool != null)
            {
                toolsMenu?.Tool?.Tools?.Remove(oldMacroEditorTool.Tool);
                oldMacroEditorTool.Tool.ToolbarsManager.Tools.Remove(oldMacroEditorTool.Tool);
            }

            var menu = mainForm.GetMainCommandbarManager()
                           .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                           .InsertCommandbarMenu(17, Key_Menu_MacroManager, Properties.Resources.MacroCommand);

            if (menu != null)
            {
                menu.AddCommandbarButton(Key_Button_Config, Properties.Resources.ConfigurateCommand);

                UpdateTools(container, true);

                if (menu != null)
                {

                    var button = menu.InsertCommandbarButton(2, Key_Button_ShowMacroEditor, Properties.Resources.MacroEditorCommand);
                    button.Shortcut = (Shortcut)(Keys.Alt | Keys.F11);
                    button.HasSeparator = true;


                    button = menu.AddCommandbarButton(Key_Button_Refresh, Properties.Resources.RefreshCommand, image: Properties.Resources.Refresh);
                    button.Tool.InstanceProps.IsFirstInGroup = true;
                }
            }
        }

        public override void OnLocalizing(MainForm form)
        {
            var menu = GetMacroMenu(form);

            if (menu != null)
            {
                menu.Text = Properties.Resources.MacroCommand;

                var button = menu.GetCommandbarButton(Key_Button_Config);

                if (button != null) button.Text = Properties.Resources.ConfigurateCommand;

                button = menu.GetCommandbarButton(Key_Button_ShowMacroEditor);

                if (button != null) button.Text = Properties.Resources.MacroEditorCommand;

                button = menu.GetCommandbarButton(Key_Button_Refresh);

                if (button != null) button.Text = Properties.Resources.RefreshCommand;

                var container = _containers.FirstOrDefault(c => c.Form.Equals(form));

                if (container != null)
                {
                    foreach (var toolPair in container.Tools)
                    {
                        var tool = toolPair.Key;
                        var resourceId = toolPair.Value;

                        if (string.IsNullOrEmpty(resourceId)) continue;
                        tool.InstanceProps.Caption = Properties.Resources.ResourceManager.GetString(resourceId, Properties.Resources.Culture);
                    }
                }
            }
        }

        MacroEditorForm CurrentEditor(Form form, bool hide, out bool hidden, out bool isNew)
        {
            if (_editor != null)
            {
                hidden = !_editor.Visible;
                isNew = false;
                return _editor;
            }

            isNew = true;
            _editor = new MacroEditorForm();
            _editor.FormClosed += MacroEditorForm_FormClosed;


#if DEBUG
            _editor.MacroCode = CodeResources.MacroEditor_CodeTemplate_MacroInternal;
#else
            _editor.MacroCode = CodeResources.MacroEditor_CodeTemplate_MacroExternal;
#endif
            _editor.SetAsDefault();
            if (hide)
            {
                _editor.Opacity = 0;
                _editor.Show();
            }
            else
                _editor.Show();

            hidden = hide;

            if (hide) _editor.Hide();

            return _editor;
        }

        void UpdateTools(MacroContainer container, bool supressMessage = false)
        {
            foreach (var tool in container.Tools)
            {
                tool.Key.ToolbarsManager.Tools.Remove(tool.Key);
            }

            container.Tools.Clear();
            container.Macros.Clear();
            RunTravers(container, supressMessage);

        }

        void RunTravers(MacroContainer container, bool supressMessage = false)
        {
            if (IsValidDirectory(out string message))
            {
                var menu = GetMacroMenu(container.Form);

                if (menu != null)
                {
                    var button = menu.InsertCommandbarButton(1, Key_Button_OpenInExplorer, Properties.Resources.OpenInExplorerCommand);
                    container.Tools.Add(button.Tool, "OpenInExplorerCommand");
                }

                var macrosDirectory = Path2.GetFullPathFromPathWithVariables(Settings[Key_MacrosDirectory]);


                int folderCounter = 1;
                int fileCounter = 1;

                if (Directory.Exists(macrosDirectory))
                {
                    DirectoryConverter.Travers(menu, 3, ref folderCounter, ref fileCounter, macrosDirectory, container, true);
                }


            }
            else
            {
                if (!supressMessage) MessageBox.Show(container.Form, message, container.Form.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool IsValidDirectory(out string message)
        {
            message = null;

            if (!Settings.ContainsKey(Key_MacrosDirectory))
            {
                message = Properties.Resources.ConfigurateAddonMessage;
                return false;
            }

            if (!Directory.Exists(Path2.GetFullPathFromPathWithVariables(Settings[Key_MacrosDirectory])))
            {
                message = Properties.Resources.DirectoryNotFoundMessage;
                return false;
            }

            return true;
        }

        CommandbarMenu GetMacroMenu(Form form)
        {
            if (form is MainForm mainForm)
            {
                return mainForm.GetMainCommandbarManager()
                            .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                            .GetCommandbarMenu(Key_Menu_MacroManager);
            }

            return null;
        }

        #endregion

        #region Eventhandler

        void MacroEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _editor.FormClosed -= MacroEditorForm_FormClosed;
            _editor = null;
        }

        void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is Form form)
            {
                var container = _containers.FirstOrDefault(c => c.Form.Equals(form));
                if (container != null) _containers.Remove(container);

                form.FormClosed -= Form_FormClosed;
            }
        }

        #endregion
    }
}