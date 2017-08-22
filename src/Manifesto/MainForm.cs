using Newtonsoft.Json;
using SwissAcademic.Citavi.BackOffice;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Manifesto
{
    public partial class MainForm : Form
    {
        #region Felder



        #endregion

        #region Konstruktor

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Eigenschaften

        AddOnManifest Manifest { get; set; }
        string ManifestPath { get; set; }

        #endregion

        #region Methoden

        #region Accept

        void Accept()
        {
            Manifest.Name = nameTextBox.Text;
            Manifest.Summary = summaryTextBox.Text;
            Manifest.Description = descriptionTextBox.Text;
            Manifest.Url = urlTextBox.Text;
            Manifest.Version = new Version(Manifest.Version.Major, Manifest.Version.Minor, Manifest.Version.Build + 1, Manifest.Version.Revision);

            foreach (TabPage tabPage in mainTabControl.TabPages)
            {
                if (tabPage.Tag == null) continue;
                AcceptLng(tabPage);
            }
        }

        void AcceptLng(TabPage tab)
        {
            var lng = tab.Tag.ToString();
            var set = new Dictionary<string, TextBox>();
            foreach (var c in tab.Controls.OfType<TextBox>())
            {
                set.Add(c.Tag.ToString(), c);
            }

            Manifest.NameLocalized.Remove(lng);
            Manifest.SummaryLocalized.Remove(lng);
            Manifest.DescriptionLocalized.Remove(lng);

            if (!string.IsNullOrEmpty(set["Name"].Text))
            {
                Manifest.NameLocalized[lng] = set["Name"].Text;
            }
            if(!string.IsNullOrEmpty(set["Summary"].Text))
            {
                Manifest.SummaryLocalized[lng] = set["Summary"].Text;
            }
            if (!string.IsNullOrEmpty(set["Description"].Text))
            {
                Manifest.DescriptionLocalized[lng] = set["Description"].Text;
            }
        }

        #endregion

        #region Init

        void Init()
        {
            nameTextBox.Text = Manifest.Name;
            summaryTextBox.Text = Manifest.Summary;
            descriptionTextBox.Text = Manifest.Description;
            urlTextBox.Text = Manifest.Url;
            versionValueLabel.Text = Manifest.Version.ToString(4);

            foreach(TabPage tabPage in mainTabControl.TabPages)
            {
                if (tabPage.Tag == null) continue;
                InitLng(tabPage);
            }
        }

        void InitLng(TabPage tab)
        {
            var lng = tab.Tag.ToString();
            var set = new Dictionary<string, TextBox>();
            foreach (var c in tab.Controls.OfType<TextBox>())
            {
                set.Add(c.Tag.ToString(), c);
            }

            if (Manifest.NameLocalized.TryGetValue(lng, out _))
            {
                set["Name"].Text = Manifest.NameLocalized[lng];
            }
            else
            {
                set["Name"].Text = string.Empty;
            }
            if (Manifest.SummaryLocalized.TryGetValue(lng, out _))
            {
                set["Summary"].Text = Manifest.SummaryLocalized[lng];
            }
            else
            {
                set["Summary"].Text = string.Empty;
            }
            if (Manifest.DescriptionLocalized.TryGetValue(lng, out _))
            {
                set["Description"].Text = Manifest.DescriptionLocalized[lng];
            }
            else
            {
                set["Description"].Text = string.Empty;
            }
        }

        #endregion

        #endregion

        #region Ereignishandler

        #region Load

        void loadButton_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.FileName = "manifest.json";
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        var manifestJson = File.ReadAllText(fileDialog.FileName);
                        Manifest = JsonConvert.DeserializeObject<AddOnManifest>(manifestJson);
                        ManifestPath = fileDialog.FileName;
                        Init();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        #endregion

        #region Save

        void saveButton_Click(object sender, EventArgs e)
        {
            if (Manifest == null) return;
            try
            {
                Accept();

                var converter = new Newtonsoft.Json.Converters.VersionConverter();

                var json = JsonConvert.SerializeObject(Manifest);
                File.WriteAllText(ManifestPath, json, Encoding.UTF8);

                versionValueLabel.Text = Manifest.Version.ToString(4);
                MessageBox.Show("Ok");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #endregion
    }
}
