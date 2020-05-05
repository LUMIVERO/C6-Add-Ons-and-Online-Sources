using Newtonsoft.Json;
using SwissAcademic.Citavi.Settings;
using System;
using System.Collections.Generic;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditorAddon
{
    public class WorkSpace
    {
        #region Constructors

        public WorkSpace()
        {
            Columns = new List<ColumnDescriptor>();
            Id = Guid.NewGuid().ToString();
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

        [JsonIgnore]
        public string Id { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Caption;
        }

        #endregion
    }
}
