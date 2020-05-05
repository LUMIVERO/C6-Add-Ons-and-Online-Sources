namespace SwissAcademic.Addons.MacroManagerAddon
{
    public class MacroCommand
    {
        #region Constructors

        public MacroCommand(string path, MacroAction action)
        {
            Path = path;
            Action = action;
        }

        #endregion

        #region Properties

        public string Path { get; }

        public MacroAction Action { get; }

        #endregion
    }
}
