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
        // Fields

        readonly List<MacroContainer> _containers = new List<MacroContainer>();

        // Methods

        public override void OnBeforePerformingCommand(MainForm mainForm, BeforePerformingCommandEventArgs e)
        {
            e.Handled = true;

            var container = _containers.FirstOrDefault(c => c.MainForm.Equals(mainForm));

            switch (e.Key)
            {
                case ButtonKey_ShowMacroEditor:
                    {
                        GetOrCreateMacroEditorForm(false, out bool _, out bool _).Activate();
                    }
                    break;
                case ButtonKey_Refresh:
                    {
                        UpdateTools(container);
                    }
                    break;
                case ButtonKey_Configurate:
                    {

                        using (var directoryDialog = new DirectoryForm(mainForm, Settings.TryGetStringValue(SettingsKey)))
                        {
                            if (directoryDialog.ShowDialog() == DialogResult.OK)
                            {
                                if (!directoryDialog.Directory.Equals(Settings.TryGetStringValue(SettingsKey), StringComparison.OrdinalIgnoreCase))
                                {
                                    Settings[SettingsKey] = directoryDialog.Directory;
                                    Program.Settings.InitialDirectories.SetInitialDirectoryContext(Citavi.Settings.InitialDirectoryContext.Macros, Path2.GetFullPathFromPathWithVariables(directoryDialog.Directory));
                                    UpdateTools(container);
                                }
                            }
                        }
                    }
                    break;
                case ButtonKey_OpenInExplorer:
                    {
                        if (IsValidDirectory(out string message))
                        {
                            var path = Path2.GetFullPathFromPathWithVariables(Settings[SettingsKey]);
                            Process.Start("explorer.exe", path);
                        }
                        else
                        {
                            MessageBox.Show(mainForm, message, mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    break;
                default:

                    if (e.Key.StartsWith(ButtonKey, StringComparison.OrdinalIgnoreCase))
                    {
                        if (container.Macros.ContainsKey(e.Key))
                        {
                            var macro = container.Macros[e.Key];

                            if (File.Exists(macro.Path))
                            {
                                var hide = macro.Action == MacroAction.Run;

                                var editor = GetOrCreateMacroEditorForm(hide, out bool hidden, out bool isNew);

                                if (!isNew && editor.IsDirty())
                                {
                                    if (MessageBox.Show(mainForm, Properties.Resources.UserWarningSaveMessage, mainForm.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                    {
                                        editor.Save();
                                    }
                                }

                                editor.MacroCode = File.ReadAllText(macro.Path);
                                editor.SetFilePath(macro.Path);
                                editor.Activate();

                                if (macro.Action == MacroAction.Run) editor.Run();

                                if (hidden) editor.Close();
                            }
                            else
                            {
                                MessageBox.Show(mainForm, Properties.Resources.PathNotFoundMessage.FormatString(macro.Path), mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            var menu = mainForm
                        .GetMainCommandbarManager()
                        .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                        .InsertCommandbarMenu(17, MenuKey_Macros, Properties.Resources.MacroCommand);

            if (menu != null)
            {
                menu.AddCommandbarButton(ButtonKey_Configurate, Properties.Resources.ConfigurateCommand);

                UpdateTools(container, true);

                var button = menu.InsertCommandbarButton(2, ButtonKey_ShowMacroEditor, Properties.Resources.MacroEditorCommand);
                button.Shortcut = (Shortcut)(Keys.Alt | Keys.F11);
                button.HasSeparator = true;


                button = menu.AddCommandbarButton(ButtonKey_Refresh, Properties.Resources.RefreshCommand, image: Properties.Resources.Refresh);
                button.Tool.InstanceProps.IsFirstInGroup = true;

            }
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var menu = mainForm.GetMacrosMenu();

            if (menu != null)
            {
                menu.Text = Properties.Resources.MacroCommand;

                var button = menu.GetCommandbarButton(ButtonKey_Configurate);

                if (button != null) button.Text = Properties.Resources.ConfigurateCommand;

                button = menu.GetCommandbarButton(ButtonKey_ShowMacroEditor);

                if (button != null) button.Text = Properties.Resources.MacroEditorCommand;

                button = menu.GetCommandbarButton(ButtonKey_Refresh);

                if (button != null) button.Text = Properties.Resources.RefreshCommand;

                var container = _containers.FirstOrDefault(c => c.MainForm.Equals(mainForm));

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

        MacroEditorForm GetOrCreateMacroEditorForm(bool hide, out bool hidden, out bool isNew)
        {
            var editor = Application.OpenForms.OfType<MacroEditorForm>().FirstOrDefault();

            if (editor != null)
            {
                hidden = !editor.Visible;
                isNew = false;
                return editor;
            }

            isNew = true;
            editor = new MacroEditorForm();

#if DEBUG
            editor.MacroCode = CodeResources.MacroEditor_CodeTemplate_MacroInternal;
#else
            editor.MacroCode = CodeResources.MacroEditor_CodeTemplate_MacroExternal;
#endif
            editor.SetAsDefault();
            if (hide)
            {
                editor.Opacity = 0;
                editor.Show();
            }
            else
                editor.Show();

            hidden = hide;

            if (hide) editor.Hide();

            return editor;
        }

        void UpdateTools(MacroContainer container, bool supressMessage = false)
        {
            container.MainForm.SuspendLayout();
            container.Reset();
            RunTravers(container, supressMessage);
            container.MainForm.ResumeLayout();
        }

        void RunTravers(MacroContainer container, bool supressMessage = false)
        {
            if (IsValidDirectory(out string message))
            {
                var menu = container.MainForm.GetMacrosMenu();

                if (menu != null)
                {
                    var button = menu.InsertCommandbarButton(1, ButtonKey_OpenInExplorer, Properties.Resources.OpenInExplorerCommand);
                    container.Tools.Add(button.Tool, "OpenInExplorerCommand");
                }

                var macrosDirectory = Path2.GetFullPathFromPathWithVariables(Settings[SettingsKey]);


                int folderCounter = 1;
                int fileCounter = 1;

                if (Directory.Exists(macrosDirectory))
                {
                    DirectoryConverter.Travers(menu, 3, ref folderCounter, ref fileCounter, macrosDirectory, container, true);
                }
            }
            else
            {
                if (!supressMessage) MessageBox.Show(container.MainForm, message, container.MainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool IsValidDirectory(out string message)
        {
            message = null;

            if (!Settings.ContainsKey(SettingsKey))
            {
                message = Properties.Resources.ConfigurateAddonMessage;
                return false;
            }

            if (!Directory.Exists(Path2.GetFullPathFromPathWithVariables(Settings[SettingsKey])))
            {
                message = Properties.Resources.DirectoryNotFoundMessage;
                return false;
            }

            return true;
        }

        // Eventhandler

        void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is MainForm mainForm)
            {
                var container = _containers.FirstOrDefault(c => c.MainForm.Equals(mainForm));
                if (container != null) _containers.Remove(container);

                mainForm.FormClosed -= Form_FormClosed;
            }
        }
    }
}