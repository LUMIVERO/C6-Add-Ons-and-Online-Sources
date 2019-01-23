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
        #region SwissAcademic.Citavi.Shell.Mainform

        public static IEnumerable<string> GetAvailableSelectedLocationsAsFilePath(this MainForm mainForm)
        {
            return (from location in mainForm.GetSelectedElectronicLocations()
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
                             select path).ToList();
        }

        #endregion

        #region System.Collections.Generic.IEnumerable<string>

        public static IEnumerable<IGrouping<string,string>> GroupByExtension(this IEnumerable<string> pathes)
        {
            return pathes.GroupBy(path => Path.GetExtension(path)).ToList();
        }

        #endregion

        #region System.Collections.Generic.IEnumerable<IGrouping<string, string>>

        public static IEnumerable<string> FirstOrDefault(this IEnumerable<IGrouping<string, string>> pathes)
        {
            return pathes.SelectMany(g => g).ToList();
        }

        #endregion

        #region System.string

        public static string FormatWithCheck(this string toFormat, string path)
        {
            if (string.IsNullOrEmpty(toFormat)) return toFormat;

            if (!toFormat.Contains("%1")) return toFormat;

            return toFormat.Replace("%1", "\"" + path + "\"");
        }

        #endregion
    }
}