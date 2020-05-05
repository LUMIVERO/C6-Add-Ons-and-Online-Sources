using Infragistics.Win.UltraWinToolbars;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SwissAcademic.Addons.MacroManagerAddon
{
    internal class MacroContainer
    {
        #region Constructors

        public MacroContainer(Form form)
        {
            Form = form;

            Macros = new Dictionary<string, MacroCommand>();
            Tools = new Dictionary<ToolBase, string>();
        }

        #endregion

        #region Properties

        public Form Form { get; private set; }

        public Dictionary<string, MacroCommand> Macros { get; private set; }

        public Dictionary<ToolBase, string> Tools { get; private set; }

        #endregion
    }
}