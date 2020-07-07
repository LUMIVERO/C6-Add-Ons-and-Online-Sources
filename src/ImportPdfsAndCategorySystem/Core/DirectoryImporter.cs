using SwissAcademic.Addons.ImportPdfsAndCategorySystemAddon.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.DataExchange;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ImportPdfsAndCategorySystemAddon
{
    internal static class DirectoryImporter
    {
        public async static Task RunAsync(MainForm mainForm, string directory)
        {
            try
            {
                var results = await GenericProgressDialog.RunTask(mainForm, FetchReferencesByFileAsync, mainForm, directory);

                foreach (var result in results)
                {
                    var addedReferences = mainForm.Project.References.AddRange(result.References);
                    AddCategories(mainForm.Project, addedReferences, result.Path.Substring(directory.Length, result.Path.Length - directory.Length - Path.GetFileName(result.Path).Length));
                }

                MessageBox.Show(mainForm, Resource.MacroResultMessage.FormatString(results.Count), mainForm.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException) { }
        }

        static async Task<List<FetchResult>> FetchReferencesByFileAsync(MainForm mainForm, string directory, IProgress<PercentageAndTextProgressInfo> progress, CancellationToken cancellationToken)
        {
            var results = new List<FetchResult>();

            var pathes = Path2.GetFilesSafe(new DirectoryInfo(directory), "*.pdf", SearchOption.AllDirectories).Select(fileInfo => fileInfo.FullName).ToList();

            for (var i = 0; i < pathes.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var path = pathes[i];

                var referencesFromFile = await new FileImportSupport().ImportFilesAsync(mainForm.Project, mainForm.Project.Engine.TransformerManager, new List<string>() { path }, AttachmentAction.Copy);

                if ((bool)referencesFromFile?.Any())
                {
                    results.Add(new FetchResult { Path = path, References = referencesFromFile });
                }

                progress.Report(new PercentageAndTextProgressInfo { Percentage = Convert.ToInt32(100 / pathes.Count * i) });
            }

            return results;
        }

        static void AddCategories(Project project, IEnumerable<Reference> references, string categoryHierarchy)
        {
            var categoryStrings = categoryHierarchy?.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if ((bool)categoryStrings?.Any())
            {
                var currentCategory = project.Categories.FirstOrDefault(item => item.Name.Equals(categoryStrings[0], StringComparison.Ordinal));

                if (currentCategory == null)
                {
                    currentCategory = project.Categories.Add(categoryStrings[0]);
                }

                foreach (var currentCategoryString in categoryStrings.Skip(1))
                {
                    var existingCategory = currentCategory.Categories.FirstOrDefault(item => item.Name.Equals(currentCategoryString, StringComparison.Ordinal));

                    currentCategory = existingCategory ?? currentCategory.Categories.Add(currentCategoryString);
                }

                references.ForEach(reference => reference.Categories.Add(currentCategory));
            }
        }
    }
}
