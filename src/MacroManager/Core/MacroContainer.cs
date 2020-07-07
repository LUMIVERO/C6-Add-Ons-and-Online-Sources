using Infragistics.Win.UltraWinToolbars;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SwissAcademic.Addons.MacroManagerAddon
{
    internal class MacroContainer
    {
        // Constructors

        public MacroContainer(Form form) => Form = form;

        // Properties

        public Form Form { get; }

        public Dictionary<string, MacroCommand> Macros { get; } = new Dictionary<string, MacroCommand>();

        public Dictionary<ToolBase, string> Tools { get; } = new Dictionary<ToolBase, string>();
    }
}