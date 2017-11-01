using SwissAcademic.Citavi;
using System.Collections.Generic;

namespace SwissAcademic.Addons.CheckUrlAndSetDate
{
    internal class MacroResult
    {
        #region Constructors

        public MacroResult()
        {
            LoopedCount = 0;
            InvalidCount = 0;
            ChangedCount = 0;
            InvalidReferences = new List<Reference>();
        }

        #endregion

        #region Properties

        public int LoopedCount { get; set; }

        public int InvalidCount { get; set; }

        public int ChangedCount { get; set; }

        public List<Reference> InvalidReferences { get; }

        #endregion
    }
}
