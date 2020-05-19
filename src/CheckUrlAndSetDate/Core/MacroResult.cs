using SwissAcademic.Citavi;
using System.Collections.Generic;

namespace SwissAcademic.Addons.CheckUrlAndSetDateAddon
{
    internal class MacroResult
    {
        public int LoopedCount { get; set; }

        public int InvalidCount { get; set; }

        public int ChangedCount { get; set; }

        public List<Reference> InvalidReferences { get; } = new List<Reference>();
    }
}
