using Newtonsoft.Json;
using SwissAcademic.Citavi.Settings;
using System;
using System.Collections.Generic;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditorAddon
{
    public class WorkSpace
    {
        // Constructors

        public WorkSpace()
        {
            Columns = new List<ColumnDescriptor>();
            Id = Guid.NewGuid().ToString();
        }

        // Properties

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

        // Methods

        public override string ToString() => Caption;
    }
}
