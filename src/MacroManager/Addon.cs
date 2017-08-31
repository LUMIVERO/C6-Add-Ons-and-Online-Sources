using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Addons.MacroManager.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.MacroManager
{
    public class Addon : CitaviAddOn
    {
        #region Fields

        AddonInfo _addonInfo;
        MacroEditorForm _macroEditor;
        CommandbarMenu _commandbarMenu;
        Dictionary<string, MacroCommand> _macroCommands;
        List<ToolBase> _tools;

        #endregion

        #region Properties

        public override AddOnHostingForm HostingForm => AddOnHostingForm.MainForm;

        #endregion

        #region Methods

        MacroEditorForm GetMacroEditor(Form form, bool hide = false)
        {
            _macroEditor = hide ? new MacroEditorForm { Owner = form, WindowState = FormWindowState.Minimized, Opacity = 0.00 } : new MacroEditorForm { Owner = form };
            _macroEditor.FormClosed += MacroEditorForm_FormClosed;

#if DEBUG
            _macroEditor.MacroCode = CodeResources.MacroEditor_CodeTemplate_MacroInternal;
#else
            _macroEditor.MacroCode = CodeResources.MacroEditor_CodeTemplate_MacroExternal;
#endif
            _macroEditor.Show();

            if (hide) _macroEditor.Hide();

            return _macroEditor;
        }

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(AddonKeys.ShowMacroEditor, StringComparison.OrdinalIgnoreCase))
            {
                _macroEditor = _macroEditor ?? GetMacroEditor(e.Form);

                _macroEditor.Activate();

                e.Handled = true;
            }
            else if (e.Key.StartsWith(AddonKeys.DirectoryCommand))
            {
                if (_macroCommands.ContainsKey(e.Key))
                {
                    var command = _macroCommands[e.Key];

                    if (File.Exists(command.MacroPath) && Path.GetExtension(command.MacroPath).Equals(".cs", StringComparison.Ordinal))
                    {
                        var hide = command.MacroAction == MacroAction.Run;

                        if (_macroEditor == null)
                        {
                            _macroEditor = GetMacroEditor(e.Form, hide);
                        }
                        else
                        {
                            if (_macroEditor.WindowState != FormWindowState.Minimized && _macroEditor.Visible && _macroEditor.Opacity != 0.00) hide = false;
                        }


                        _macroEditor.MacroCode = File.ReadAllText(command.MacroPath);

                        _macroEditor.Activate();

                        if (command.MacroAction == MacroAction.Run)
                        {
                            var method = _macroEditor.GetType()
                                                        .GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                                                        .FirstOrDefault(mth => mth.Name.Equals("PerformCommand", StringComparison.OrdinalIgnoreCase));

                            if (method != null)
                            {
                                method.Invoke(_macroEditor, new object[] { "Run", null, null, null });
                            }
                        }

                        if (hide)
                        {
                            _macroEditor.Close();
                        }
                    }
                }
                e.Handled = true;
            }
            else if (e.Key.Equals(AddonKeys.Refresh, StringComparison.OrdinalIgnoreCase))
            {
                Refresh();

                e.Handled = true;
            }
        }

        protected override void OnHostingFormLoaded(Form form)
        {
            if (form is MainForm mainForm)
            {
                if (_addonInfo == null) _addonInfo = new AddonInfo();

                _commandbarMenu = mainForm.GetMainCommandbarManager()
                               .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                               .InsertCommandbarMenu(17, AddonKeys.MacroMenu, MacroManagerResources.MacroCommand);
                if (_commandbarMenu != null)
                {
                    _macroCommands = new Dictionary<string, MacroCommand>();
                    _tools = new List<ToolBase>();

                    RunDirectoryConverter();
                }
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form hostingForm)
        {
            if (_commandbarMenu != null)
            {
                _commandbarMenu.Text = MacroManagerResources.MacroCommand;
                Refresh();
            }

            base.OnLocalizing(hostingForm);
        }

        void RunDirectoryConverter()
        {
            if (_commandbarMenu != null)
            {
                var button = _commandbarMenu.AddCommandbarButton(AddonKeys.ShowMacroEditor, MacroManagerResources.MacroEditorCommand);

                _tools.Add(button.Tool);
            }

            if (AddonInfoHelper.ValidConfiguration(_addonInfo))
            {
                var macrosDirectory = AddonInfoHelper.GetConfigurationContentAsFilePath(_addonInfo);

                int folderCounter = 1;
                int fileCounter = 1;

                if (Directory.Exists(macrosDirectory))
                {
                    DirectoryConverter.Travers(_commandbarMenu, ref folderCounter, ref fileCounter, macrosDirectory, _macroCommands, _tools, true);
                }
            }

            if (_commandbarMenu != null)
            {
                var button = _commandbarMenu.AddCommandbarButton(AddonKeys.Refresh, MacroManagerResources.RefreshCommand, image: MacroManagerResources.Refresh);
                button.Tool.InstanceProps.IsFirstInGroup = true;
                _tools.Add(button.Tool);
            }
        }


        void Refresh()
        {
            foreach (var tool in _tools)
            {
                tool.ToolbarsManager.Tools.Remove(tool);
            }

            _tools.Clear();
            _macroCommands.Clear();

            RunDirectoryConverter();
        }

       


        #endregion

        #region Eventhandler

        void MacroEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _macroEditor.FormClosed -= MacroEditorForm_FormClosed;
            _macroEditor = null;
        }

        #endregion
    }
}
