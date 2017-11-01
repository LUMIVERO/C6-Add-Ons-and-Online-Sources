using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwissAcademic.Addons.MacroManager
{
    internal class EnvironmentVariable
    {
        public EnvironmentVariable(string name, string path, EnvironmentVariableTarget type)
        {
            Name = $"%{name}%";
            Path = path;
            Type = type;
        }

        public string Name { get; }

        public string Path { get; }
        public EnvironmentVariableTarget Type { get; }
    }
}
