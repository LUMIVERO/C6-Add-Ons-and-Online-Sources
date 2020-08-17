using Newtonsoft.Json;
using System.Collections.Generic;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditorAddon
{
    public class AddonSettings
    {
        // Constructors

        AddonSettings() => WorkSpaces = new List<WorkSpace>();

        // Properties

        [JsonIgnore]
        public static AddonSettings Default => new AddonSettings();

        [JsonProperty]
        public List<WorkSpace> WorkSpaces { get; set; }
    }
}
