namespace SwissAcademic.Addons.MacroManager
{
    public class MacroCommand
    {
        #region Constructors

        public MacroCommand(string macroPath, MacroAction macroAction)
        {
            MacroPath = macroPath;
            MacroAction = macroAction;
        }

        #endregion

        #region Properties

        public string MacroPath { get; }

        public MacroAction MacroAction { get; }

        #endregion
    }
}
