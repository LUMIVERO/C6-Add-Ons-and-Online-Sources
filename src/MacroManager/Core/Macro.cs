namespace SwissAcademic.Addons.MacroManagerAddon
{
    public class Macro
    {
        // Constructors

        public Macro(string path, MacroAction action)
        {
            Path = path;
            Action = action;
        }

        //n Properties

        public string Path { get; }

        public MacroAction Action { get; }
    }
}
