using Newtonsoft.Json;
using SwissAcademic.Citavi.Settings;
using System.Collections.Generic;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor
{
    public class WorkSpace
    {
        #region Constructors

        public WorkSpace()
        {
            Columns = new List<ColumnDescriptor>();
        }

        #endregion

        #region Properties

        [JsonProperty]
        public string Caption { get; set; }

        [JsonProperty]
        public bool AllowUpdate { get; set; }

        [JsonProperty]
        public List<ColumnDescriptor> Columns { get; set; }

        [JsonProperty]
        public bool GroupByBoxVisible { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Caption;
        }

        #endregion
    }
}
