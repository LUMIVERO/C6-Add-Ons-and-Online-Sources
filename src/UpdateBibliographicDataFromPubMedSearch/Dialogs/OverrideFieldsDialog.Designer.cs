namespace SwissAcademic.Addons.UpdateBibliographicDataFromPubMedSearchAddon
{
    partial class OverrideFieldsDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.chbOverrideKeywords = new System.Windows.Forms.CheckBox();
            this.chbOverrideAbstract = new System.Windows.Forms.CheckBox();
            this.chbOverrideTOC = new System.Windows.Forms.CheckBox();
            this.chbRemoveNotes = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(110, 106);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(29, 106);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // chbOverrideKeywords
            // 
            this.chbOverrideKeywords.AutoSize = true;
            this.chbOverrideKeywords.Location = new System.Drawing.Point(12, 12);
            this.chbOverrideKeywords.Name = "chbOverrideKeywords";
            this.chbOverrideKeywords.Size = new System.Drawing.Size(124, 19);
            this.chbOverrideKeywords.TabIndex = 2;
            this.chbOverrideKeywords.Text = "Override keywords";
            this.chbOverrideKeywords.UseVisualStyleBackColor = true;
            // 
            // chbOverrideAbstract
            // 
            this.chbOverrideAbstract.AutoSize = true;
            this.chbOverrideAbstract.Location = new System.Drawing.Point(12, 35);
            this.chbOverrideAbstract.Name = "chbOverrideAbstract";
            this.chbOverrideAbstract.Size = new System.Drawing.Size(116, 19);
            this.chbOverrideAbstract.TabIndex = 3;
            this.chbOverrideAbstract.Text = "Override abstract";
            this.chbOverrideAbstract.UseVisualStyleBackColor = true;
            // 
            // chbOverrideTOC
            // 
            this.chbOverrideTOC.AutoSize = true;
            this.chbOverrideTOC.Location = new System.Drawing.Point(12, 58);
            this.chbOverrideTOC.Name = "chbOverrideTOC";
            this.chbOverrideTOC.Size = new System.Drawing.Size(158, 19);
            this.chbOverrideTOC.TabIndex = 4;
            this.chbOverrideTOC.Text = "Override table of content";
            this.chbOverrideTOC.UseVisualStyleBackColor = true;
            // 
            // chbRemoveNotes
            // 
            this.chbRemoveNotes.AutoSize = true;
            this.chbRemoveNotes.Location = new System.Drawing.Point(12, 81);
            this.chbRemoveNotes.Name = "chbRemoveNotes";
            this.chbRemoveNotes.Size = new System.Drawing.Size(101, 19);
            this.chbRemoveNotes.TabIndex = 5;
            this.chbRemoveNotes.Text = "Remove notes";
            this.chbRemoveNotes.UseVisualStyleBackColor = true;
            // 
            // OverrideFieldsDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(197, 141);
            this.Controls.Add(this.chbRemoveNotes);
            this.Controls.Add(this.chbOverrideTOC);
            this.Controls.Add(this.chbOverrideAbstract);
            this.Controls.Add(this.chbOverrideKeywords);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OverrideFieldsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chbOverrideKeywords;
        private System.Windows.Forms.CheckBox chbOverrideAbstract;
        private System.Windows.Forms.CheckBox chbOverrideTOC;
        private System.Windows.Forms.CheckBox chbRemoveNotes;
    }
}