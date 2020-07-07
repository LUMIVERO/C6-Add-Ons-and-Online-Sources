namespace SwissAcademic.Addons.MacroManagerAddon
{
    public class MacroCommand
    {
        // Constructors

        public MacroCommand(string path, MacroAction action)
        {
            Path = path;
            Action = action;
        }

        //n Properties

        public string Path { get; }

        public MacroAction Action { get; }
    }
}
