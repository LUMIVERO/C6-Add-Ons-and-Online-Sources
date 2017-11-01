using SwissAcademic.Addons.ImportPdfsAndCategorySystem.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.DataExchange;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportPdfsAndCategorySystem
{
    internal static class Macro
    {
        public static void Run(MainForm mainForm)
        {
            var counter = 0;

            string sourcePath;

            using (var folderDialog = new FolderBrowserDialog { Description = ImportPdfsAndCategorySystemResource.FolderBrowserDialogDescription })
            {
                if (folderDialog.ShowDialog() != DialogResult.OK) return;

                sourcePath = folderDialog.SelectedPath;
            }

            var action = AttachmentAction.Copy;
            var directoryInfo = new DirectoryInfo(sourcePath);
            var fileInfos = Path2.GetFilesSafe(directoryInfo, "*.pdf", SearchOption.AllDirectories).ToList();
            var newReferences = new List<Reference>();

            foreach (var filePath in fileInfos.Select(info => info.FullName).ToList())
            {
                var referencesFromFile = new FileImportSupport().ImportFiles(mainForm.Project, mainForm.Project.Engine.TransformerManager, new List<string>() { filePath }, action);

                if (referencesFromFile != null && referencesFromFile.Any())
                {
                    var referencesFromFileAdded = mainForm.Project.References.AddRange(referencesFromFile);

                    var fileName = Path.GetFileName(filePath);
                    AddCategories(referencesFromFileAdded, filePath.Substring(sourcePath.Length, filePath.Length - sourcePath.Length - fileName.Length));
                    counter++;
                }
            }

            MessageBox.Show(mainForm, ImportPdfsAndCategorySystemResource.MacroResultMessage.FormatString(counter), mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static void AddCategories(IEnumerable<Reference> references, string categoryHierarchy)
        {
            var categoryStrings = categoryHierarchy.Split(new Char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (categoryStrings == null || !categoryStrings.Any() || references == null || !references.Any()) return;

            var project = references.First().Project;
            var currentCategoryString = categoryStrings.FirstOrDefault();
            var currentCategory = project.Categories.FirstOrDefault(item => item.Name.Equals(currentCategoryString, StringComparison.Ordinal));
            if (currentCategory == null) currentCategory = project.Categories.Add(currentCategoryString);

            for (var i = 1; i < categoryStrings.Count; i++)
            {
                currentCategoryString = categoryStrings.ElementAt(i);
                var existingCategory = currentCategory.Categories.FirstOrDefault(item => item.Name.Equals(currentCategoryString, StringComparison.Ordinal));
                if (existingCategory == null)
                {
                    currentCategory = currentCategory.Categories.Add(currentCategoryString);
                }
                else
                {
                    currentCategory = existingCategory;
                }
            }

            foreach (var reference in references)
            {
                reference.Categories.Add(currentCategory);
            }
        }
    }
}
