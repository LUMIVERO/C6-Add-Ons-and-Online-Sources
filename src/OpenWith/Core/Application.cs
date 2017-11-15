using Newtonsoft.Json;
using SwissAcademic.Citavi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SwissAcademic.Addons.OpenWith
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Application : ICloneable
    {
        #region Constructors

        public Application()
        {
            Id = Guid.NewGuid().ToString();
            Filters = new List<string>();
            Argument = "%1";
        }

        #endregion

        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "argument")]
        public string Argument { get; set; }

        [JsonProperty(PropertyName = "filters")]
        public List<string> Filters { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Application { Name = this.Name, Argument = this.Argument, Path = this.Path, Id = this.Id, Filters = this.Filters };
        }

        public void Run(Dictionary<Location, string> locations)
        {
            if (locations.Count == 0) return;
            foreach (var pair in locations)
            {
                var path = pair.Value;
                var location = pair.Key;

                if (!File.Exists(path) && location.Address.LinkedResourceType != LinkedResourceType.RemoteUri) continue;

                var startInfo = new ProcessStartInfo(this.Path)
                {
                    WindowStyle = ProcessWindowStyle.Normal
                };

                if (!string.IsNullOrEmpty(this.Argument))
                {
                    startInfo.Arguments = this.Argument.FormatWithCheck(path);
                }

                Process.Start(startInfo);
            }
        }

        #endregion
    }
}
