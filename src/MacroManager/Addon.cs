using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Addons.MacroManager.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SwissAcademic.Addons.MacroManager
{
    public class Addon : CitaviAddOn
    {
        #region Constants

        const string Key_ShowMacroEditor = "SwissAcademic.Addons.MacroManager.ShowMacroEditor";
        const string Key_OpenInExplorer = "SwissAcademic.Addons.MacroManager.OpenInExplorer";
        const string Key_Refresh = "SwissAcademic.Addons.MacroManager.Refresh";
        internal const string Key_DirectoryMenu = "SwissAcademic.Addons.MacroManager.Menu.{0}";
        internal const string Key_DirectoryCommand = "SwissAcademic.Addons.MacroManager.Command";
        const string Key_MacroMenu = "SwissAcademic.Addons.MacroManager.MacroMenu";
        const string Key_ConfigCommand = "SwissAcademic.Addons.MacroManager.ConfigCommand";
        const string Key_MacrosDirectory = "SwissAcademic.Addons.MacroManager.MacrosDirectory";

        #endregion

        #region Fields

        MacroEditorForm _editor;
        CommandbarMenu _menu;
        Dictionary<string, MacroCommand> _macros;
        Dictionary<ToolBase, string> _tools;

        #endregion

        #region Constructors

        public Addon() : base()
        {
            _macros = new Dictionary<string, MacroCommand>();
            _tools = new Dictionary<ToolBase, string>();
        }

        #endregion

        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;

            switch (e.Key)
            {
                case (Key_ShowMacroEditor):
                    {
                        CurrentEditor(e.Form, false, out bool hidden, out bool isNew).Activate();
                    }
                    break;
                case (Key_Refresh):
                    {
                        UpdateTools(e.Form);
                    }
                    break;
                case (Key_ConfigCommand):
                    {

                        using (var directoryDialog = new DirectoryDialog(Settings.TryGetStringValue(Key_MacrosDirectory)) { Owner = e.Form })
                        {
                            if (directoryDialog.ShowDialog() == DialogResult.OK)
                            {
                                Settings[Key_MacrosDirectory] = directoryDialog.Directory;
                                Program.Settings.InitialDirectories.SetInitialDirectoryContext(SwissAcademic.Citavi.Settings.InitialDirectoryContext.Macros, Path2.GetFullPathFromPathWithVariables(directoryDialog.Directory));
                                UpdateTools(e.Form);
                            }
                        }
                    }
                    break;
                case (Key_OpenInExplorer):
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

                    if (e.Key.StartsWith(Key_DirectoryCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        if (_macros.ContainsKey(e.Key))
                        {
                            var macro = _macros[e.Key];

                            if (File.Exists(macro.Path))
                            {
                                var hide = macro.Action == MacroAction.Run;

                                _editor = CurrentEditor(e.Form, hide, out bool hidden, out bool isNew);

                                if (!isNew && _editor.IsDirty())
                                {
                                    if (MessageBox.Show(e.Form, MacroManagerResources.UserWarningSaveMessage, e.Form.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
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
                                MessageBox.Show(e.Form, MacroManagerResources.PathNotFoundMessage.FormatString(macro.Path), e.Form.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                UpdateTools(e.Form, true);
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

            base.OnBeforePerformingCommand(e);
        }

        protected override void OnHostingFormLoaded(Form form)
        {
            if (form is MainForm mainForm)
            {
                var oldMacroEditorTool = mainForm.GetMainCommandbarManager().GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu).GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.Tools).GetCommandbarButton("ShowMacroEditorForm");

                if (oldMacroEditorTool != null)
                {
                    oldMacroEditorTool.Tool.ToolbarsManager.Tools.Remove(oldMacroEditorTool.Tool);
                }

                _menu = mainForm.GetMainCommandbarManager()
                               .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                               .InsertCommandbarMenu(17, Key_MacroMenu, MacroManagerResources.MacroCommand);
                if (_menu != null)
                {
                    _menu.AddCommandbarButton(Key_ConfigCommand, MacroManagerResources.ConfigurateCommand);

                    UpdateTools(form, true);

                    if (_menu != null)
                    {
                        var button = _menu.AddCommandbarButton(Key_Refresh, MacroManagerResources.RefreshCommand, image: MacroManagerResources.Refresh);
                        button.Tool.InstanceProps.IsFirstInGroup = true;
                    }
                }
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            if (_menu != null)
            {
                _menu.Text = MacroManagerResources.MacroCommand;


                var button = _menu.GetCommandbarButton(Key_ConfigCommand);

                if (button != null) button.Text = MacroManagerResources.ConfigurateCommand;

                button = _menu.GetCommandbarButton(Key_Refresh);

                if (button != null) button.Text = MacroManagerResources.RefreshCommand;

                foreach (var toolPair in _tools)
                {
                    var tool = toolPair.Key;
                    var resourceId = toolPair.Value;

                    if (string.IsNullOrEmpty(resourceId)) continue;
                    tool.InstanceProps.Caption = MacroManagerResources.ResourceManager.GetString(resourceId, MacroManagerResources.Culture);
                }
            }

            base.OnLocalizing(form);
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
            _editor.Show();

            hidden = hide;

            if (hide) _editor.Hide();

            return _editor;
        }

        void UpdateTools(Form form, bool supressMessage = false)
        {
            foreach (var tool in _tools)
            {
                tool.Key.ToolbarsManager.Tools.Remove(tool.Key);
            }

            _tools.Clear();
            _macros.Clear();
            RunTravers(form, supressMessage);
        }

        void RunTravers(Form form, bool supressMessage = false)
        {
            if (IsValidDirectory(out string message))
            {
                if (_menu != null)
                {
                    var button = _menu.InsertCommandbarButton(1, Key_OpenInExplorer, MacroManagerResources.OpenInExplorerCommand);
                    _tools.Add(button.Tool, "OpenInExplorerCommand");

                    button = _menu.InsertCommandbarButton(2, Key_ShowMacroEditor, MacroManagerResources.MacroEditorCommand);
                    button.Shortcut = (Shortcut)(Keys.Alt | Keys.F11);
                    button.HasSeparator = true;
                    _tools.Add(button.Tool, "MacroEditorCommand");
                }

                var macrosDirectory = Path2.GetFullPathFromPathWithVariables(Settings[Key_MacrosDirectory]);


                int folderCounter = 1;
                int fileCounter = 1;

                if (Directory.Exists(macrosDirectory))
                {
                    DirectoryConverter.Travers(_menu, 3, ref folderCounter, ref fileCounter, macrosDirectory, _macros, _tools, true);
                }

            }
            else
            {
                if (!supressMessage) MessageBox.Show(form, message, form.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool IsValidDirectory(out string message)
        {
            message = null;

            if (!Settings.ContainsKey(Key_MacrosDirectory))
            {
                message = MacroManagerResources.ConfigurateAddonMessage;
                return false;
            }

            if (!Directory.Exists(Path2.GetFullPathFromPathWithVariables(Settings[Key_MacrosDirectory])))
            {
                message = MacroManagerResources.DirectoryNotFoundMessage;
                return false;
            }

            return true;
        }

        #endregion

        #region Eventhandler

        void MacroEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _editor.FormClosed -= MacroEditorForm_FormClosed;
            _editor = null;
        }

        #endregion
    }
}
