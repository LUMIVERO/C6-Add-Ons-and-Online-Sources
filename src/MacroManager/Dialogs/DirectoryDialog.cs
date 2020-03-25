using SwissAcademic.Addons.MacroManager.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
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
            lblEnvironmentFullPath.Text = Path2.GetFullPathFromPathWithVariables(directory);
        }

        bool ExistDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                MessageBox.Show(this, Properties.Resources.FolderBrowseDialogDescription, Owner.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            var path = Path2.GetFullPathFromPathWithVariables(directory);

            if (System.IO.Directory.Exists(path)) return true;

            MessageBox.Show(this, Properties.Resources.DirectoryNotResolveMessage.FormatString(path), Owner.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        void Localize()
        {
            this.Text = Properties.Resources.DirectoryDialogTitle;
            btnCancel.Text = Properties.Resources.Btn_Cancel;
            btnOk.Text = Properties.Resources.Btn_Ok;
        }

        IEnumerable<EnvironmentVariable> GetPossibleUserEnvironmentVariables()
        {
            var variables = new List<EnvironmentVariable>();

            foreach (var entry in System.Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine).Cast<DictionaryEntry>())
            {
                var name = entry.Key.ToString();
                var value = entry.Value.ToString();

                if (!System.IO.Directory.Exists(value)) continue;

                variables.Add(new EnvironmentVariable(name, value, EnvironmentVariableTarget.Machine));
            }

            foreach (var entry in System.Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User).Cast<DictionaryEntry>())
            {
                var name = entry.Key.ToString();
                var value = entry.Value.ToString();

                if (!System.IO.Directory.Exists(value)) continue;

                variables.Add(new EnvironmentVariable(name, value, EnvironmentVariableTarget.User));
            }

            return variables;
        }



        string GetPathWithVariablesFromFullPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            var entry = GetPossibleUserEnvironmentVariables().FirstOrDefault(v => path.StartsWith(v.Path.Trim('\\'), StringComparison.OrdinalIgnoreCase));

            if (entry == null) return path;

            return path.Replace(entry.Path.Trim('\\'), entry.Name);
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
            using (var folderBrowseDialog = new FolderBrowserDialog { Description = Properties.Resources.FolderBrowseDialogDescription })
            {
                var root = System.IO.Path2.GetFullPathFromPathWithVariables(txtPath.Text);
                if (System.IO.Directory.Exists(root)) folderBrowseDialog.SelectedPath = root;

                if (folderBrowseDialog.ShowDialog(this) != DialogResult.OK) return;

                var entry = GetPathWithVariablesFromFullPath(folderBrowseDialog.SelectedPath);

                if (!string.IsNullOrEmpty(entry) && !folderBrowseDialog.SelectedPath.Equals(entry, StringComparison.OrdinalIgnoreCase) && MessageBox.Show(this, Properties.Resources.PathAsVariableMessage.FormatString(folderBrowseDialog.SelectedPath, entry), Owner.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    txtPath.Text = entry;
                    lblEnvironmentFullPath.Text = System.IO.Path2.GetFullPathFromPathWithVariables(entry);
                }
                else
                {
                    txtPath.Text = folderBrowseDialog.SelectedPath;
                    lblEnvironmentFullPath.Text = folderBrowseDialog.SelectedPath;
                }
            }
        }

        void BtnEnvironmentVariables_Click(object sender, EventArgs e)
        {
            ccEnvironment.Items.Clear();

            foreach (var entry in GetPossibleUserEnvironmentVariables())
            {
                switch (entry.Type)
                {
                    case EnvironmentVariableTarget.User:
                        ccEnvironment.Items.Add(entry.Name, Properties.Resources.user, ToolStripItem_ItemClick);
                        break;
                    case EnvironmentVariableTarget.Machine:
                        ccEnvironment.Items.Add(entry.Name, Properties.Resources.maschine, ToolStripItem_ItemClick);
                        break;
                }
            }

            if (ccEnvironment.Items.Count == 0)
            {
                MessageBox.Show(this, Properties.Resources.EnvironmentVariablesNotFound, Owner.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        void TxtPath_TextChanged(object sender, EventArgs e)
        {
            lblEnvironmentFullPath.Text = System.IO.Path2.GetFullPathFromPathWithVariables(txtPath.Text);
        }

        #endregion
    }
}
