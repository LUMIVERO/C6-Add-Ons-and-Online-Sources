using SwissAcademic.Citavi;
using System.Collections.Generic;

namespace SwissAcademic.Addons.ImportPdfsAndCategorySystemAddon
{
    internal class FetchResult
    {
        public string Path { get; set; }

        public IEnumerable<Reference> References { get; set; }
    }
}
