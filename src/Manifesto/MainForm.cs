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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Manifesto
{
    public partial class MainForm : Form
    {
        #region Felder



        #endregion

        #region Konstruktor

        public MainForm(string[] args)
        {
            InitializeComponent();

            if (args.Length != 0)
            {
                LoadJsonObject(args.FirstOrDefault(str => !string.IsNullOrEmpty(str) && File.Exists(str) && Path.GetExtension(str).Equals(".json", StringComparison.OrdinalIgnoreCase)));
            }
        }

        #endregion

        #region Eigenschaften

        CitaviResourceManifest Manifest { get; set; }
        string ManifestPath { get; set; }

        #endregion

        #region Methoden

        #region Accept

        void Accept()
        {
            Manifest.Name = nameTextBox.Text;
            Manifest.Description = descriptionTextBox.Text;
            Manifest.Version = new Version(Manifest.Version.Major, Manifest.Version.Minor, Manifest.Version.Build, Manifest.Version.Revision + 1);

            if (Manifest is AddOnManifest)
            {
                ((AddOnManifest)Manifest).Summary = summaryTextBox.Text;
                ((AddOnManifest)Manifest).Url = urlTextBox.Text;
            }
            foreach (TabPage tabPage in mainTabControl.TabPages)
            {
                if (tabPage.Tag == null) continue;
                AcceptLng(tabPage);
            }
            versionValueLabel.Text = Manifest.Version.ToString(4);
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

            Manifest.DescriptionLocalized.Remove(lng);

            if (!string.IsNullOrEmpty(set["Name"].Text))
            {
                Manifest.NameLocalized[lng] = set["Name"].Text;
            }

            if (!string.IsNullOrEmpty(set["Description"].Text))
            {
                Manifest.DescriptionLocalized[lng] = set["Description"].Text;
            }

            if (Manifest is AddOnManifest)
            {
                ((AddOnManifest)Manifest).SummaryLocalized.Remove(lng);
                if (!string.IsNullOrEmpty(set["Summary"].Text))
                {
                    ((AddOnManifest)Manifest).SummaryLocalized[lng] = set["Summary"].Text;
                }
            }
        }

        #endregion

        #region Init

        void Init()
        {
            urlTextBox.Enabled = Manifest is AddOnManifest;
            summaryTextBox.Enabled = Manifest is AddOnManifest;

            if (Manifest is AddOnManifest)
            {
                Text = "Manifesto: Citavi AddOn";
                summaryTextBox.Text = ((AddOnManifest)Manifest).Summary ?? string.Empty;
                urlTextBox.Text = ((AddOnManifest)Manifest).Url ?? string.Empty;
                descriptionTextBox.Text = ((AddOnManifest)Manifest).Description ?? string.Empty;
            }
            else
            {
                Text = "Manifesto: Zitationsstil-Element";
                descriptionTextBox.Text = ((CitationStyleElementManifest)Manifest).Description ?? string.Empty;
                summaryTextBox.Text = string.Empty;
                urlTextBox.Text = string.Empty;
            }
            nameTextBox.Text = Manifest.Name;
            versionValueLabel.Text = Manifest.Version.ToString(4);
            idValueLabel.Text = Manifest.Id.ToString();
            foreach (TabPage tabPage in mainTabControl.TabPages)
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

            if (Manifest.DescriptionLocalized.TryGetValue(lng, out _))
            {
                set["Description"].Text = Manifest.DescriptionLocalized[lng];
            }
            else
            {
                set["Description"].Text = string.Empty;
            }

            set["Summary"].Text = string.Empty;
            set["Summary"].Enabled = Manifest is AddOnManifest;
            if (Manifest is AddOnManifest)
            {
                if (((AddOnManifest)Manifest).SummaryLocalized.TryGetValue(lng, out _))
                {
                    set["Summary"].Text = ((AddOnManifest)Manifest).SummaryLocalized[lng];
                }
            }
        }

        void LoadJsonObject(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                newButton.Hide();
                saveButton.Show();
                var manifestJson = File.ReadAllText(path);
                if (manifestJson.Contains("EntryPoint"))
                {
                    Manifest = JsonConvert.DeserializeObject<AddOnManifest>(manifestJson);
                }
                else
                {
                    Manifest = JsonConvert.DeserializeObject<CitationStyleElementManifest>(manifestJson);
                }
                ManifestPath = path;
                Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Ereignishandler

        #region New
        void newButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fileDialog = new OpenFileDialog())
                {
                    fileDialog.Filter = "Zitationsstil-Element (*.xml)|*.xml|Citavi AddOn (*.dll)|*.dll";
                    if (fileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        if (fileDialog.FileName.EndsWith(".xml"))
                        {
                            Manifest = new CitationStyleElementManifest();
                            var doc = XDocument.Load(File.OpenRead(fileDialog.FileName));
                            Manifest.Id = new Guid(doc.Root.Attributes("id").First().Value);
                            if (doc.Root.Name == "BibliographyGroupingSet")
                            {
                                Manifest.Name = doc.Root.Elements("Name").First().Value;
                            }
                            ManifestPath = Path.Combine(Path.GetDirectoryName(fileDialog.FileName), Path.GetFileNameWithoutExtension(fileDialog.FileName) + ".json");
                        }
                        else
                        {
                            ManifestPath = Path.Combine(Path.GetDirectoryName(fileDialog.FileName), "manifest.json");
                            Manifest = new AddOnManifest();
                            ((AddOnManifest)Manifest).EntryPoint = Path.GetFileName(fileDialog.FileName);
                            var asm = Assembly.LoadFrom(fileDialog.FileName);
                            Manifest.Id = new Guid(((GuidAttribute)asm.GetCustomAttributes(typeof(GuidAttribute), true).First()).Value);
                            ((AddOnManifest)Manifest).SummaryLocalized = new Dictionary<string, string>();
                        }

                        Manifest.NameLocalized = new Dictionary<string, string>();
                        Manifest.DescriptionLocalized = new Dictionary<string, string>();
                        Manifest.Version = new Version(1, 0, 0, 0);
                        Manifest.CitaviMinVersion = new Version(5, 8, 0, 0);
                        Manifest.ManifestVersion = 1;
                        Init();

                        newButton.Hide();
                        loadButton.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Load

        void loadButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fileDialog = new OpenFileDialog())
                {
                    fileDialog.Filter = "Manifest (*.json)|*.json";
                    if (fileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadJsonObject(fileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
