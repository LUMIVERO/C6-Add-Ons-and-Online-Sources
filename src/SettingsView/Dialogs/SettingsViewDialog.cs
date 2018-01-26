using SwissAcademic.Addons.SettingsView.Properties;
using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissAcademic.Addons.SettingsView
{
    public partial class SettingsViewDialog : Form
    {
        #region Constructors

        public SettingsViewDialog(Form owner)
        {
            InitializeComponent();
            Owner = owner;
            Icon = (Icon)owner.Icon.Clone();
            Localize();
            InitializeListView();
        }

        #endregion

        #region Methods

        void Localize()
        {
            btnClose.Text = SettingsViewResources.SettingsViewDialog_Close;
            btnExport.Text = SettingsViewResources.SettingsViewDialog_Export;
            Text = SettingsViewResources.SettingsViewDialog_Title;
            chKeys.Text = SettingsViewResources.SettingsViewDialog_LV_Keys;
            chValues.Text = SettingsViewResources.SettingsViewDialog_LV_Values;
        }

        void InitializeListView()
        {
            lvAddonsSettings.Items.Clear();
            foreach (CitaviAddOn addon in Program.AddOnEngine.AddOns)
            {
                var index = lvAddonsSettings.Groups.Add(new ListViewGroup(addon.Name, HorizontalAlignment.Left));
                foreach (var setting in addon.Settings)
                {
                    var item = new ListViewItem(new[] { setting.Key, setting.Value });
                    lvAddonsSettings.Items.Add(item);
                    item.Group = lvAddonsSettings.Groups[index];
                }
            }

            btnExport.Enabled = lvAddonsSettings.Items.Count != 0;
        }

        void WriteSettingsInFile(string filePath)
        {
            var stringBuilder = new StringBuilder();

            foreach (CitaviAddOn addon in Program.AddOnEngine.AddOns)
            {
                stringBuilder.AppendLine($"{addon.Name} (ID: {addon.Id} | VERSION: {addon.Version.ToString()})");
                stringBuilder.AppendLine();
                foreach (var setting in addon.Settings)
                {
                    stringBuilder.AppendLine($"\t{setting.Key}: {setting.Value}");
                }

                stringBuilder.AppendLine($"\n");
            }

            File.WriteAllText(filePath, stringBuilder.ToString(), Encoding.UTF8);
        }

        #endregion

        #region Eventhandlers

        void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        void BtnExport_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog { Filter = SettingsViewResources.SettingsViewDialog_Export_Filter })
            {
                if (saveFileDialog.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    WriteSettingsInFile(saveFileDialog.FileName);
                    if (MessageBox.Show(this, SettingsViewResources.SettingsViewDialog_Export_CompleteMessage, SettingsViewResources.SettingsViewDialog_Export_CompleteTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information,MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Process.Start("explorer.exe", Path.GetDirectoryName(saveFileDialog.FileName));
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, SettingsViewResources.SettingsViewDialog_Export_Exception.FormatSmart(exception.Message), SettingsViewResources.SettingsViewDialog_Export_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion
    }
}
