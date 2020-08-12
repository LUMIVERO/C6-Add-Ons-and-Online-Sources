using SwissAcademic.Addons.ExportAttachmentsToCategoryFolderStructureAddon.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ExportAttachmentsToCategoryFolderStructureAddon
{
    public static class Macro
    {
        public static void Run(MainForm mainForm, string exportPath)
        {
            int changeCounter = 0;
            int errorCounter = 0;

            mainForm.Project.AllCategories.CreateCategoryPathes(exportPath).ForEach(path => CreateFolderStructure(path));

            foreach (var reference in mainForm.GetFilteredReferences())
            {
                var locations = reference.Locations.FilterBySupportedLocations();
                if (locations.Count == 0) continue;

                var categoryPathes = reference.Categories.CreateCategoryPathes(exportPath);
                if (categoryPathes.Count == 0)
                {
                    var tempPath = mainForm.Project.AllCategories.Count == 0 ? exportPath : exportPath + @"\" + Resources.NoCategoryFolder;
                    categoryPathes.Add(tempPath);
                }


                // create folders if necessary ...
                foreach (var categoryPath in categoryPathes)
                {
                    CreateFolderStructure(categoryPath);

                    foreach (var location in locations)
                    {
                        var sourcePath = location.Address.Resolve().GetLocalPathSafe();
                        var destinationPath = string.Empty;

                        if (location.Address.LinkedResourceType == LinkedResourceType.AttachmentRemote)
                        {
                            if (location.Address.LinkedResourceStatus != LinkedResourceStatus.Attached) continue;
                            if (location.Address.CachingStatus != CachingStatus.Available) continue;

                            destinationPath = categoryPath + @"\" + location.FullName;
                        }
                        else
                        {
                            destinationPath = categoryPath + @"\" + Path.GetFileName(sourcePath);
                        }

                        if (!File.Exists(sourcePath))
                        {
                            errorCounter++;
                            continue;
                        }

                        bool tryAgain = true;
                        while (tryAgain)
                        {
                            try
                            {
                                File.Copy(sourcePath, destinationPath, true);
                                changeCounter++;
                                tryAgain = false;
                            }
                            catch (Exception e)
                            {
                                tryAgain = false;
                                DialogResult directoryError = MessageBox.Show(Resources.Messages_CreatingFolderError + e.Message,
                                                            Resources.Messages_Error, MessageBoxButtons.AbortRetryIgnore,
                                                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                                if (directoryError == DialogResult.Abort) return;
                                else if (directoryError == DialogResult.Retry) tryAgain = true;
                                else
                                {
                                    errorCounter++;
                                    break;
                                };
                            }
                        }
                    }
                }
            }

            MessageBox.Show(Resources.Messages_Completed.FormatString(System.Environment.NewLine + changeCounter, System.Environment.NewLine + errorCounter), mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static List<Location> FilterBySupportedLocations(this ReferenceLocationCollection collection)
        {
            return
                collection
                .Where(location =>
                       location.LocationType == LocationType.ElectronicAddress &&
                       (
                            location.Address.LinkedResourceType == LinkedResourceType.AttachmentRemote ||
                            location.Address.LinkedResourceType == LinkedResourceType.AttachmentFile ||
                            location.Address.LinkedResourceType == LinkedResourceType.AbsoluteFileUri ||
                            location.Address.LinkedResourceType == LinkedResourceType.RelativeFileUri
                       )).ToList();
        }

        static IList<string> CreateCategoryPathes(this CitaviEntityCollection<Category> categories, string exportPath)
        {
            var pathes = new List<string>();
            foreach (var category in categories.ToList())
            {
                var categoryPaths = category.GetPath(true).Split(new string[] { " > " }, StringSplitOptions.None);
                for (int i = 0; i < categoryPaths.Length; i++)
                {
                    categoryPaths[i] = MakeValidFileName(categoryPaths[i]);
                }
                pathes.Add(exportPath + @"\" + string.Join(@"\", categoryPaths));
            }
            return pathes;
        }

        static void CreateFolderStructure(string fullPath)
        {
            if (!Directory.Exists(fullPath))
            {
                while (true)
                {
                    try
                    {
                        Directory.CreateDirectory(fullPath);
                        return;

                    }
                    catch (Exception e)
                    {
                        var directoryError = MessageBox.Show(Resources.Messages_CreatingFolderError + e.Message,
                                                             Resources.Messages_Error,
                                                             MessageBoxButtons.AbortRetryIgnore,
                                                             MessageBoxIcon.Error,
                                                             MessageBoxDefaultButton.Button1
                                                            );

                        if (directoryError == DialogResult.Retry) continue;
                        else return;
                    }
                }
            }
        }

        static string MakeValidFileName(this string name) => string.Join("_", name.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
    }
}
