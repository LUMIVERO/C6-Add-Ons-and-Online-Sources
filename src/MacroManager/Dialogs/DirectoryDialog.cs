using SwissAcademic.Addons.MacroManager.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.MacroManager
{
    public partial class DirectoryDialog : Form
    {
        #region Constructors

        public DirectoryDialog(string directory)
        {
            InitializeComponent();
            InitializeDirectory(directory);
            Localize();
        }

        #endregion

        #region Properties

        public string Directory => txtPath.Text;

        #endregion

        #region Methods

        void InitializeDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory)) return;

            txtPath.Text = directory;
            lblEnvironmentFullPath.Text = IsEnvironmentVariable(directory)
                                                ? System.IO.Path2.GetFullPathFromPathWithVariables(directory)
                                                : string.Empty;
        }

        bool ExistDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                MessageBox.Show(this, MacroManagerResources.FolderBrowseDialogDescription, "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            var path = System.IO.Path2.GetFullPathFromPathWithVariables(directory);

            if (System.IO.Directory.Exists(path)) return true;

            MessageBox.Show(this, MacroManagerResources.DirectoryNotResolveMessage.FormatString(path), "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        bool IsEnvironmentVariable(string directory)
        {
            return directory.StartsWith("%") && directory.EndsWith("%") && System.IO.Directory.Exists(System.IO.Path2.GetFullPathFromPathWithVariables(directory));
        }

        void Localize()
        {
            this.Text = MacroManagerResources.DirectoryDialogTitle;
            btnCancel.Text = MacroManagerResources.Btn_Cancel;
            btnOk.Text = MacroManagerResources.Btn_Ok;
        }

        IEnumerable<DictionaryEntry> GetPossibleUserEnvironmentVariables()
        {
            return System.Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User)
                                     .Cast<DictionaryEntry>()
                                     .Where(d => System.IO.Directory.Exists(d.Value.ToString()))
                                     .OrderBy(d => d.Key.ToString())
                                     .ToList();
        }

        string SearchUserEnvironmentVariableForPath(string path)
        {
            return (from e in (from entry in GetPossibleUserEnvironmentVariables()
                               select new { key = entry.Key.ToString(), value = entry.Value.ToString() })
                    where !string.IsNullOrEmpty(e.key) && !string.IsNullOrEmpty(e.value) && e.value.TrimEnd('\\').Equals(path.TrimEnd('\\'), StringComparison.OrdinalIgnoreCase)
                    select e.key).FirstOrDefault();
        }

        #endregion

        #region Eventhandlers

        void BtnOk_Click(object sender, EventArgs e)
        {
            if (!ExistDirectory(txtPath.Text)) return;

            DialogResult = DialogResult.OK;
        }

        void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        void BtnFolderBrowserDialog_Click(object sender, EventArgs e)
        {
            using (var folderBrowseDialog = new FolderBrowserDialog { Description = MacroManagerResources.FolderBrowseDialogDescription })
            {
                if (folderBrowseDialog.ShowDialog(this) != DialogResult.OK) return;

                var entry = SearchUserEnvironmentVariableForPath(folderBrowseDialog.SelectedPath);

                if (!string.IsNullOrEmpty(entry) && MessageBox.Show(this, MacroManagerResources.PathAsVariableMessage.FormatString(folderBrowseDialog.SelectedPath, entry), "Citavi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    txtPath.Text = $"%{entry}%";
                    lblEnvironmentFullPath.Text = folderBrowseDialog.SelectedPath;
                }
                else
                {
                    txtPath.Text = folderBrowseDialog.SelectedPath;
                    lblEnvironmentFullPath.Text = string.Empty;
                }
            }
        }

        void BtnEnvironmentVariables_Click(object sender, EventArgs e)
        {
            ccEnvironment.Items.Clear();

            foreach (var key in GetPossibleUserEnvironmentVariables().Select(d => $"%{d.Key.ToString()}%"))
            {
                ccEnvironment.Items.Add(key, null, ToolStripItem_ItemClick);
            }

            if (ccEnvironment.Items.Count == 0)
            {
                MessageBox.Show(this, MacroManagerResources.EnvironmentVariablesNotFound, "Citavi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ccEnvironment.Show(btnEnvironmentVariables, new Point(btnEnvironmentVariables.Height));
        }

        void ToolStripItem_ItemClick(object sender, EventArgs e)
        {
            if (sender is ToolStripItem item)
            {
                txtPath.Text = item.Text;
                lblEnvironmentFullPath.Text = System.IO.Path2.GetFullPathFromPathWithVariables(item.Text);
            }
        }

        #endregion
    }
}
