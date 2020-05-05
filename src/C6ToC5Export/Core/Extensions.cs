using SwissAcademic.Citavi;
using System.Linq;

namespace SwissAcademic.Addons.C6ToC5ExportAddon
{
    internal static class Extensions
    {
        public static bool HasFileLocations(this ProjectAllLocationsCollection locations)
        {
            return (from location in locations
                    where
                       location.LocationType == LocationType.ElectronicAddress &&
                       ((location.Address.LinkedResourceType == LinkedResourceType.AttachmentRemote) ||
                       (
                           location.Address.LinkedResourceType == LinkedResourceType.AttachmentFile ||
                           location.Address.LinkedResourceType == LinkedResourceType.AbsoluteFileUri ||
                           location.Address.LinkedResourceType == LinkedResourceType.RelativeFileUri
                       ))
                    select location).Any();
        }
    }
}
