using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwissAcademic.Addons.OpenWith
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Configuration : ICloneable
    {
        #region Constructors

        public Configuration()
        {
            Applications = new List<Application>();
        }

        #endregion

        #region Properties

        [JsonProperty(PropertyName = "name")]
        public string MaschineName { get; set; }

        [JsonProperty(PropertyName = "applications")]
        public List<Application> Applications { get; set; }

        [JsonIgnore]
        public static Configuration Empty => new Configuration { MaschineName = Device.Name };

        #endregion

        #region Methods

        public object Clone()
        {
            return new Configuration { MaschineName = this.MaschineName, Applications = this.Applications.Clone().ToList() };
        }

        #endregion
    }
}
