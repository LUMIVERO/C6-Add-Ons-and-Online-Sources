using SwissAcademic.Citavi;
using System.Collections.Generic;

namespace SwissAcademic.Addons.CheckUrlAndSetDate
{
    internal class CheckUrlAndSetDateStringsMacroResult
    {
        public CheckUrlAndSetDateStringsMacroResult()
        {
            LoopedCount = 0;
            InvalidCount = 0;
            ChangedCount = 0;
            InvalidReferences = new List<Reference>();
        }

        public int LoopedCount { get; set; }

        public int InvalidCount { get; set; }

        public int ChangedCount { get; set; }

        public List<Reference> InvalidReferences { get;  }
    }
}
