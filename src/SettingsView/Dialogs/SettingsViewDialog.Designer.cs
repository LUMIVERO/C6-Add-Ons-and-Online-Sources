namespace SwissAcademic.Addons.SettingsView
{
    partial class SettingsViewDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.lvAddonsSettings = new System.Windows.Forms.ListView();
            this.chKeys = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chValues = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(576, 393);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(495, 393);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // lvAddonsSettings
            // 
            this.lvAddonsSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAddonsSettings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chKeys,
            this.chValues});
            this.lvAddonsSettings.GridLines = true;
            this.lvAddonsSettings.Location = new System.Drawing.Point(12, 12);
            this.lvAddonsSettings.MultiSelect = false;
            this.lvAddonsSettings.Name = "lvAddonsSettings";
            this.lvAddonsSettings.Size = new System.Drawing.Size(639, 375);
            this.lvAddonsSettings.TabIndex = 2;
            this.lvAddonsSettings.UseCompatibleStateImageBehavior = false;
            this.lvAddonsSettings.View = System.Windows.Forms.View.Details;
            this.lvAddonsSettings.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LvAddonsSettings_MouseDown);
            // 
            // chKeys
            // 
            this.chKeys.Width = -1;
            // 
            // chValues
            // 
            this.chValues.Width = -1;
            // 
            // SettingsViewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 428);
            this.Controls.Add(this.lvAddonsSettings);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnClose);
            this.Name = "SettingsViewDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SettingsViewDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ListView lvAddonsSettings;
        private System.Windows.Forms.ColumnHeader chKeys;
        private System.Windows.Forms.ColumnHeader chValues;
    }
}