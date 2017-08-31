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

        MacroEditorForm _editor;
        CommandbarMenu _menu;
        Dictionary<string, MacroCommand> _macros;
        List<ToolBase> _tools;

        #endregion

        #region Constructors

        public Addon() : base()
        {
            _macros = new Dictionary<string, MacroCommand>();
            _tools = new List<ToolBase>();
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
                case (AddonKeys.ShowMacroEditor):
                    {
                        CurrentEditor(e.Form).Activate();
                    }
                    break;
                case (AddonKeys.DirectoryCommand):
                    {
                        if (_macros.ContainsKey(e.Key))
                        {
                            var macro = _macros[e.Key];

                            if (File.Exists(macro.Path))
                            {
                                var hide = macro.Action == MacroAction.Run;
                                _editor = CurrentEditor(e.Form, hide);

                                if (!_editor.IsHidden()) hide = false;

                                _editor.MacroCode = File.ReadAllText(macro.Path);
                                _editor.Activate();

                                if (macro.Action == MacroAction.Run) _editor.Run();

                                if (hide) _editor.Close();
                            }
                            else
                            {
                                MessageBox.Show(e.Form, MacroManagerResources.PathNotFoundMessage.FormatString(macro.Path), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                    }
                    break;
                case (AddonKeys.Refresh):
                    {
                        UpdateTools(e.Form);
                    }
                    break;
                case (AddonKeys.ConfigCommand):
                    {
                        using (var folderBrowseDialog = new FolderBrowserDialog { Description = MacroManagerResources.FolderBrowseDialogDescription })
                        {
                            if (folderBrowseDialog.ShowDialog(e.Form) == DialogResult.OK)
                            {
                                this.Settings[AddonKeys.MacrosDirectory] = folderBrowseDialog.SelectedPath;
                                UpdateTools(e.Form);
                            }
                        }
                    }
                    break;
                default:
                    e.Handled = false;
                    break;
            }

            base.OnBeforePerformingCommand(e);
        }

        protected override void OnHostingFormLoaded(Form form)
        {
            if (form is MainForm mainForm)
            {
                _menu = mainForm.GetMainCommandbarManager()
                               .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                               .InsertCommandbarMenu(17, AddonKeys.MacroMenu, MacroManagerResources.MacroCommand);
                if (_menu != null)
                {
                    _menu.AddCommandbarButton(AddonKeys.ConfigCommand, MacroManagerResources.ConfigurateCommand);

                    UpdateTools(form);
                }
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(Form form)
        {
            if (_menu != null)
            {
                _menu.Text = MacroManagerResources.MacroCommand;

                var button = _menu.GetCommandbarButton(AddonKeys.ConfigCommand);

                if (button != null) button.Text = MacroManagerResources.ConfigurateCommand;

                UpdateTools(form);
            }

            base.OnLocalizing(form);
        }

        MacroEditorForm CurrentEditor(Form form, bool hide = false)
        {
            if (_editor != null) return _editor;

            _editor = hide ? new MacroEditorForm { Owner = form, WindowState = FormWindowState.Minimized, Opacity = 0.00 } : new MacroEditorForm { Owner = form };
            _editor.FormClosed += MacroEditorForm_FormClosed;

#if DEBUG
            _editor.MacroCode = CodeResources.MacroEditor_CodeTemplate_MacroInternal;
#else
            _macroEditor.MacroCode = CodeResources.MacroEditor_CodeTemplate_MacroExternal;
#endif
            _editor.Show();

            if (hide) _editor.Hide();

            return _editor;
        }

        void UpdateTools(Form form)
        {
            foreach (var tool in _tools)
            {
                tool.ToolbarsManager.Tools.Remove(tool);
            }

            _tools.Clear();
            _macros.Clear();
            RunTravers(form);
        }

        void RunTravers(Form form)
        {
            if (IsValidDirectory(out string message))
            {
                if (_menu != null)
                {
                    var button = _menu.AddCommandbarButton(AddonKeys.ShowMacroEditor, MacroManagerResources.MacroEditorCommand);

                    _tools.Add(button.Tool);
                }

                var macrosDirectory = Settings[AddonKeys.MacrosDirectory];


                int folderCounter = 1;
                int fileCounter = 1;

                if (Directory.Exists(macrosDirectory))
                {
                    DirectoryConverter.Travers(_menu, ref folderCounter, ref fileCounter, macrosDirectory, _macros, _tools, true);
                }

                if (_menu != null)
                {
                    var button = _menu.AddCommandbarButton(AddonKeys.Refresh, MacroManagerResources.RefreshCommand, image: MacroManagerResources.Refresh);
                    button.Tool.InstanceProps.IsFirstInGroup = true;
                    _tools.Add(button.Tool);
                }
            }
            else
            {
                MessageBox.Show(form, message, "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool IsValidDirectory(out string message)
        {
            message = null;

            if (!this.Settings.ContainsKey(AddonKeys.MacrosDirectory))
            {
                message = MacroManagerResources.ConfigurateAddonMessage;
                return false;
            }

            if (!Directory.Exists(this.Settings[AddonKeys.MacrosDirectory]))
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
