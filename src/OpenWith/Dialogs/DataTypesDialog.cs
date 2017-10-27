using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.OpenWith
{
    public partial class DataTypesDialog : Form
    {
        #region Constructors

        public DataTypesDialog(List<string> datatypes)
        {
            InitializeComponent();
            Localize();
            InitializeDataTypes(datatypes);
        }

        #endregion

        #region Methods

        void Localize()
        {
            btnAdd.Enabled = false;
            btnAdd.Enabled = false;

            btnCancel.Text = Properties.OpenWithResources.Dialog_Cancel;
            btnOk.Text = Properties.OpenWithResources.Dialog_Ok;
            Text = Properties.OpenWithResources.DataTypesDialog_Text;
        }

        void InitializeDataTypes(List<string> dataTypes)
        {
            lbTarget.Items.AddRange(dataTypes.ToArray());
            lbTarget.SelectedItem = dataTypes.FirstOrDefault();

            var sourceDataTypes = Registry.ClassesRoot
                                         .GetSubKeyNames()
                                         .Where(key => key.StartsWith("."))
                                         .OrderBy(v => v)
                                         .Except(dataTypes)
                                         .ToArray();
            lbSource.Items.AddRange(sourceDataTypes);
            lbSource.SelectedItem = sourceDataTypes.FirstOrDefault();
        }

        #endregion

        #region Properties

        public List<string> SelectedDataTypes => lbTarget.Items.Cast<string>().ToList();

        #endregion

        #region Eventhandlers

        void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        void BtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        void BtnAdd_Click(object sender, EventArgs e)
        {
            if (lbSource.SelectedItem is string item)
            {
                lbSource.Items.Remove(item);
                lbTarget.Items.Add(item);
                lbTarget.Sorted = true;
            }
        }

        void BtnRemove_Click(object sender, EventArgs e)
        {
            if (lbTarget.SelectedItem is string item)
            {
                lbTarget.Items.Remove(item);
                lbSource.Items.Add(item);
                lbSource.Sorted = true;
            }
        }

        void LbSource_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lbSource.SelectedItem is string) btnAdd.Enabled = true;
            else btnAdd.Enabled = false;
        }

        void LbTarget_SelectedValueChanged(object sender, EventArgs e)
        {
            if (lbTarget.SelectedItem is string) btnRemove.Enabled = true;
            else btnRemove.Enabled = false;
        }

        #endregion
    }
}
