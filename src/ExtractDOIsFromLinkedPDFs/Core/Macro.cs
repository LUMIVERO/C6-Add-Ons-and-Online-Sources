using SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.DataExchange;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ExtractDOIsFromLinkedPDFs
{
    internal static class Macro
    {
        public static void Run(Form form, Project project)
        {
            int counter = 0;

            try
            {
                var citaviFilesPath = project.Engine.DesktopEngineConfiguration.GetFolderPath(CitaviFolder.Attachments, project);

                var fileLocations = project.AllLocations.Where(location =>
                    location.LocationType == LocationType.ElectronicAddress &&
                    (
                        location.AddressUri.AddressInfo == ElectronicAddressInfo.Attachment ||
                        location.AddressUri.AddressInfo == ElectronicAddressInfo.AbsoluteFileUri ||
                        location.AddressUri.AddressInfo == ElectronicAddressInfo.RelativeFileUri
                    )
                ).ToList();


                var supporter = new ReferenceIdentifierSupport();


                foreach (Location location in fileLocations)
                {
                    if (location.Reference == null) continue;
                    if (!string.IsNullOrEmpty(location.Reference.Doi)) continue;

                    var path = string.Empty;
                    switch (location.AddressUri.AddressInfo)
                    {
                        case ElectronicAddressInfo.Attachment:
                            path = Path.Combine(citaviFilesPath, location.Address);
                            break;

                        case ElectronicAddressInfo.RelativeFileUri:
                            path = location.AddressUri.AbsoluteUri.LocalPath;
                            break;

                        case ElectronicAddressInfo.AbsoluteFileUri:
                            path = location.Address;
                            break;

                        default:
                            continue;
                    }

                    if (string.IsNullOrEmpty(path)) continue;
                    if (!File.Exists(path)) continue;
                    if (!Path.GetExtension(path).Equals(".pdf", StringComparison.OrdinalIgnoreCase)) continue;

                    var matches = supporter.FindIdentifierInFile(path, ReferenceIdentifierType.Doi, false);
                    if (matches.Count == 0) continue;
                    var match = matches.ElementAt(0);

                    if (string.IsNullOrEmpty(location.Reference.Doi))
                    {
                        location.Reference.Doi = match.ToString();
                        counter++;
                    }
                }


            }

            finally
            {
                MessageBox.Show(form, ExtractDOIsFromLinkedPDFsResources.MacroFinallyMessage.FormatString(counter), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
