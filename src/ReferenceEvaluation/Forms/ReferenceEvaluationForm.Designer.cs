namespace SwissAcademic.Addons.ReferenceEvaluationAddon
{
    partial class ReferenceEvaluationForm
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
            this.lblChoice = new System.Windows.Forms.Label();
            this.cboFunctions = new System.Windows.Forms.ComboBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnClipboard = new System.Windows.Forms.Button();
            this.lblOptions = new System.Windows.Forms.Label();
            this.chbShowHeaders = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(882, 549);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lblChoice
            // 
            this.lblChoice.AutoSize = true;
            this.lblChoice.Location = new System.Drawing.Point(9, 9);
            this.lblChoice.Name = "lblChoice";
            this.lblChoice.Size = new System.Drawing.Size(54, 15);
            this.lblChoice.TabIndex = 2;
            this.lblChoice.Text = "Function";
            // 
            // cboFunctions
            // 
            this.cboFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFunctions.BackColor = System.Drawing.Color.White;
            this.cboFunctions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFunctions.FormattingEnabled = true;
            this.cboFunctions.Location = new System.Drawing.Point(12, 25);
            this.cboFunctions.Name = "cboFunctions";
            this.cboFunctions.Size = new System.Drawing.Size(945, 23);
            this.cboFunctions.TabIndex = 3;
            this.cboFunctions.SelectedIndexChanged += new System.EventHandler(this.CbFunctions_SelectedIndexChanged);
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResult.Location = new System.Drawing.Point(12, 109);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(945, 400);
            this.txtResult.TabIndex = 4;
            this.txtResult.WordWrap = false;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(12, 93);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(39, 15);
            this.lblResult.TabIndex = 5;
            this.lblResult.Text = "Result";
            // 
            // btnClipboard
            // 
            this.btnClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClipboard.Location = new System.Drawing.Point(15, 515);
            this.btnClipboard.Name = "btnClipboard";
            this.btnClipboard.Size = new System.Drawing.Size(942, 23);
            this.btnClipboard.TabIndex = 6;
            this.btnClipboard.Text = "Clipboard";
            this.btnClipboard.UseVisualStyleBackColor = true;
            this.btnClipboard.Click += new System.EventHandler(this.BtnClipboard_Click);
            // 
            // lblOptions
            // 
            this.lblOptions.AutoSize = true;
            this.lblOptions.Location = new System.Drawing.Point(9, 54);
            this.lblOptions.Name = "lblOptions";
            this.lblOptions.Size = new System.Drawing.Size(39, 15);
            this.lblOptions.TabIndex = 7;
            this.lblOptions.Text = "Result";
            // 
            // chbShowHeaders
            // 
            this.chbShowHeaders.AutoSize = true;
            this.chbShowHeaders.Checked = true;
            this.chbShowHeaders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowHeaders.Location = new System.Drawing.Point(12, 70);
            this.chbShowHeaders.Name = "chbShowHeaders";
            this.chbShowHeaders.Size = new System.Drawing.Size(83, 19);
            this.chbShowHeaders.TabIndex = 8;
            this.chbShowHeaders.Text = "checkBox1";
            this.chbShowHeaders.UseVisualStyleBackColor = true;
            this.chbShowHeaders.CheckedChanged += new System.EventHandler(this.ChbShowHeaders_CheckedChanged);
            // 
            // ReferenceEvaluationDialog
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(969, 584);
            this.Controls.Add(this.chbShowHeaders);
            this.Controls.Add(this.lblOptions);
            this.Controls.Add(this.btnClipboard);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.cboFunctions);
            this.Controls.Add(this.lblChoice);
            this.Controls.Add(this.btnClose);
            this.Name = "ReferenceEvaluationDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ReferenceEvaluationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblChoice;
        private System.Windows.Forms.ComboBox cboFunctions;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnClipboard;
        private System.Windows.Forms.Label lblOptions;
        private System.Windows.Forms.CheckBox chbShowHeaders;
    }
}