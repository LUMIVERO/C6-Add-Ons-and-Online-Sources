using Infragistics.Win.UltraWinToolbars;
using SwissAcademic.Citavi.Shell;
using System.Collections.Generic;

namespace SwissAcademic.Addons.MacroManagerAddon
{
    internal class MacroContainer
    {
        // Constructors

        public MacroContainer(MainForm mainForm) => MainForm = mainForm;

        // Properties

        public MainForm MainForm { get; }

        public Dictionary<string, Macro> Macros { get; } = new Dictionary<string, Macro>();

        public Dictionary<ToolBase, string> Tools { get; } = new Dictionary<ToolBase, string>();

        // Methods

        public void Reset()
        {
            Tools.ForEach(tool => tool.Key.ToolbarsManager.Tools.Remove(tool.Key));
            Tools.Clear();
            Macros.Clear();
        }
    }
}