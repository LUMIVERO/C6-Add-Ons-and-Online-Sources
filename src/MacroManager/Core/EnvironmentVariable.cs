using System;

namespace SwissAcademic.Addons.MacroManagerAddon
{
    internal class EnvironmentVariable
    {
        #region Constructors

        public EnvironmentVariable(string name, string path, EnvironmentVariableTarget type)
        {
            Name = $"%{name}%";
            Path = path;
            Type = type;
        }

        #endregion

        #region Properties

        public string Name { get; }

        public string Path { get; }

        public EnvironmentVariableTarget Type { get; }

        #endregion
    }
}
