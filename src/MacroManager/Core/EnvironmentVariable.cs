using System;

namespace SwissAcademic.Addons.MacroManagerAddon
{
    internal class EnvironmentVariable
    {
        // Constructors

        public EnvironmentVariable(string name, string path, EnvironmentVariableTarget type)
        {
            Name = $"%{name}%";
            Path = path;
            Type = type;
        }

        // Properties

        public string Name { get; }

        public string Path { get; }

        public EnvironmentVariableTarget Type { get; }
    }
}
