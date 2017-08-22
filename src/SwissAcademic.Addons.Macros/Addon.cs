// ######################################
// #                                    #
// #    Copyright                       #
// #    Daniel Lutz                     #
// #    Swiss Academic Software GmbH    #
// #    2014                            #
// #                                    #
// ######################################

using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Drawing;
using SwissAcademic.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using WinForm = System.Windows.Forms.Form;

namespace ManageMacrosAddon
{
    public class Addon : CitaviAddOn
    {
        #region Properties

        #region AddonInfo

        public AddonInfo AddonInfo { get; set; }

        #endregion

        #region HostingForm

        public override AddOnHostingForm HostingForm
        {
            get { return AddOnHostingForm.MainForm; }
        }

        #endregion

        #region MacroEditorForm

        public MacroEditorForm MacroEditorForm { get; set; }

        #endregion

        #region Menu

        public CommandbarMenu Menu { get; set; }

        #endregion

        #region MacroCommands

        public Dictionary<string, MacroCommand> MacroCommands { get; set; }

        #endregion

        #region Tools

        public List<ToolBase> Tools { get; set; }

        #endregion


        #endregion

        #region Methods

        #region GetMacroEditor

        MacroEditorForm GetMacroEditor(WinForm form, bool hide = false)
        {
            MacroEditorForm = new MacroEditorForm();
            MacroEditorForm.FormClosed += MacroEditorForm_FormClosed;

            if (hide)
            {
                MacroEditorForm.WindowState = System.Windows.Forms.FormWindowState.Minimized;
                MacroEditorForm.Opacity = 0.00;
            }

#if DEBUG
            MacroEditorForm.MacroCode = CodeResources.MacroEditor_CodeTemplate_MacroInternal;
#else
            MacroEditorForm.MacroCode = CodeResources.MacroEditor_CodeTemplate_MacroExternal;
#endif
            MacroEditorForm.Show();

            if (hide) MacroEditorForm.Hide();

            return MacroEditorForm;
        }

        #endregion

        #region OnApplicationIdle

        protected override void OnApplicationIdle(WinForm form)
        {
            base.OnApplicationIdle(form);
        }

        #endregion

        #region OnBeforePerformingCommand

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(AddonKeys.ShowMacroEditor, StringComparison.Ordinal))
            {

                if (MacroEditorForm == null)
                {
                    MacroEditorForm = GetMacroEditor(e.Form);
                }

                MacroEditorForm.Activate();

                e.Handled = true;
            }
            else if (e.Key.StartsWith(AddonKeys.DirectoryCommand))
            {
                if (MacroCommands.ContainsKey(e.Key))
                {
                    var command = MacroCommands[e.Key];

                    if (System.IO.File.Exists(command.MacroPath) && System.IO.Path.GetExtension(command.MacroPath).Equals(".cs", StringComparison.Ordinal))
                    {
                        var hide = command.MacroAction == MacroAction.Run;

                        if (MacroEditorForm == null)
                        {
                            MacroEditorForm = GetMacroEditor(e.Form, hide);
                        }
                        else
                        {
                            if (MacroEditorForm.WindowState != System.Windows.Forms.FormWindowState.Minimized && MacroEditorForm.Visible && MacroEditorForm.Opacity != 0.00) hide = false;
                        }


                        MacroEditorForm.MacroCode = System.IO.File.ReadAllText(command.MacroPath);

                        MacroEditorForm.Activate();

                        if (command.MacroAction == MacroAction.Run)
                        {
                            var method = MacroEditorForm.GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).FirstOrDefault(mth => mth.Name.Equals("PerformCommand"));

                            if (method != null)
                            {
                                method.Invoke(MacroEditorForm, new object[] { "Run", null, null, null });
                            }
                        }

                        if (hide)
                        {
                            MacroEditorForm.Close();
                        }
                    }
                }
                e.Handled = true;
            }
            else if (e.Key.Equals(AddonKeys.Refresh, StringComparison.Ordinal))
            {
                foreach (var tool in Tools)
                {
                    tool.ToolbarsManager.Tools.Remove(tool);
                }

                Tools.Clear();
                MacroCommands.Clear();

                RunDirectoryConverter();

                e.Handled = true;
            }
        }

        #endregion

        #region OnChangingColorScheme

        protected override void OnChangingColorScheme(WinForm form, ColorScheme colorScheme)
        {
            base.OnChangingColorScheme(form, colorScheme);
        }

        #endregion

        #region OnHostingFormLoaded

        protected override void OnHostingFormLoaded(WinForm form)
        {

            if (form is MainForm mainForm)
            {
                if (AddonInfo == null) AddonInfo = new AddonInfo();

                Menu = mainForm.GetMainCommandbarManager().GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu).InsertCommandbarMenu(17, "SwissAcademic.Addons.Macros.Menu", "Makros");
                if (Menu != null)
                {
                    MacroCommands = new Dictionary<string, MacroCommand>();
                    Tools = new List<ToolBase>();

                    RunDirectoryConverter();


                }
            }

            base.OnHostingFormLoaded(form);
        }

        #endregion

        #region OnLocalizing

        protected override void OnLocalizing(WinForm form)
        {
            base.OnLocalizing(form);
        }

        #endregion

        #region RunDirectoryConverter

        void RunDirectoryConverter()
        {
            if (Menu != null)
            {
                var button = Menu.AddCommandbarButton(AddonKeys.ShowMacroEditor, "Makro-Editor…");

                Tools.Add(button.Tool);
            }

            if (AddonInfoHelper.ValidConfiguration(AddonInfo))
            {
                var macrosDirectory = AddonInfoHelper.GetConfigurationContentAsFilePath(AddonInfo);

                int folderCounter = 1;
                int fileCounter = 1;

                if (System.IO.Directory.Exists(macrosDirectory))
                {
                    DirectoryConverter.Travers(Menu, ref folderCounter, ref fileCounter, macrosDirectory, MacroCommands, Tools, true);
                }
            }

            if (Menu != null)
            {
                var button = Menu.AddCommandbarButton(AddonKeys.Refresh, "Aktualisieren", image: Properties.Resources.Refresh);
                button.Tool.InstanceProps.IsFirstInGroup = true;
                Tools.Add(button.Tool);
            }
        }

        #endregion

        #endregion

        #region Eventhandler

        void MacroEditorForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            MacroEditorForm.FormClosed -= MacroEditorForm_FormClosed;
            MacroEditorForm = null;
        }

        #endregion
    }
}
