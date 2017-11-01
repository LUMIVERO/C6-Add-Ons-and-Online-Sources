using Newtonsoft.Json;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SwissAcademic.Addons.OpenWith
{
    internal static class Extensions
    {
        public static Dictionary<Location, string> GetAvailableSelectedLocations(this MainForm mainForm)
        {
            var locations = (from location in mainForm.GetSelectedElectronicLocations()
                             where
                                 location.LocationType == LocationType.ElectronicAddress &&
                                 ((location.Address.LinkedResourceType == LinkedResourceType.AttachmentRemote &&
                                 location.Address.CachingStatus == CachingStatus.Available) ||
                                 (
                                     location.Address.LinkedResourceType == LinkedResourceType.AttachmentFile ||
                                     location.Address.LinkedResourceType == LinkedResourceType.AbsoluteFileUri ||
                                     location.Address.LinkedResourceType == LinkedResourceType.RelativeFileUri ||
                                     location.Address.LinkedResourceType == LinkedResourceType.RemoteUri
                                 ))
                             let path = location.Address.LinkedResourceType != LinkedResourceType.RemoteUri ? location.Address.Resolve().GetLocalPathSafe() : location.Address.UriString
                             where
                                location.Address.LinkedResourceType == LinkedResourceType.RemoteUri || File.Exists(path)
                             select new KeyValuePair<Location, string>(location, path)).ToList();

            var dictionary = new Dictionary<Location, string>();
            dictionary.AddRange(locations);
            return dictionary;
        }

        public static Configuration Load(this Dictionary<string, string> settings)
        {
            if (!settings.ContainsKey(Addon.Key_Settings.FormatString(Device.Name))) return Configuration.Empty;

            var json = settings[Addon.Key_Settings.FormatString(Device.Name)];

            return JsonConvert.DeserializeObject<Configuration>(json);
        }

        public static void Save(this Dictionary<string, string> settings, Configuration configuration)
        {
            var json = JsonConvert.SerializeObject(configuration);
            settings[Addon.Key_Settings.FormatString(Device.Name)] = json;
        }

        public static IEnumerable<T> Clone<T>(this IEnumerable<T> list) where T : ICloneable
        {
            return (from value in list
                    select (T)Convert.ChangeType(value, typeof(T)));
        }

        public static string FormatWithCheck(this string toFormat, string path)
        {
            if (string.IsNullOrEmpty(toFormat)) return toFormat;

            if (!toFormat.Contains("%1")) return toFormat;

            return toFormat.Replace("%1", "\"" + path + "\"");
        }
    }
}
