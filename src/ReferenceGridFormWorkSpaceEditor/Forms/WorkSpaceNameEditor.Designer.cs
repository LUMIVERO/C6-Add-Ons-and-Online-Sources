namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor
{
    partial class WorkSpaceNameEditor
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
            this.lbl_workspace_name = new System.Windows.Forms.Label();
            this.txt_workspace_name = new System.Windows.Forms.TextBox();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_create = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_workspace_name
            // 
            this.lbl_workspace_name.AutoSize = true;
            this.lbl_workspace_name.Location = new System.Drawing.Point(23, 20);
            this.lbl_workspace_name.Name = "lbl_workspace_name";
            this.lbl_workspace_name.Size = new System.Drawing.Size(153, 15);
            this.lbl_workspace_name.TabIndex = 0;
            this.lbl_workspace_name.Text = "Name des Arbeitsbereiches:";
            // 
            // txt_workspace_name
            // 
            this.txt_workspace_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_workspace_name.Location = new System.Drawing.Point(26, 38);
            this.txt_workspace_name.Name = "txt_workspace_name";
            this.txt_workspace_name.Size = new System.Drawing.Size(390, 23);
            this.txt_workspace_name.TabIndex = 1;
            this.txt_workspace_name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Txt_workspace_name_KeyDown);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancel.Location = new System.Drawing.Point(323, 67);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(93, 23);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "Abbrechen";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.Btn_edit_Click);
            // 
            // btn_create
            // 
            this.btn_create.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_create.Location = new System.Drawing.Point(224, 67);
            this.btn_create.Name = "btn_create";
            this.btn_create.Size = new System.Drawing.Size(93, 23);
            this.btn_create.TabIndex = 3;
            this.btn_create.Text = "Anlegen";
            this.btn_create.UseVisualStyleBackColor = true;
            this.btn_create.Click += new System.EventHandler(this.Btn_create_Click);
            // 
            // WorkSpaceNameEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 98);
            this.Controls.Add(this.btn_create);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.txt_workspace_name);
            this.Controls.Add(this.lbl_workspace_name);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorkSpaceNameEditor";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WorkSpaceNameEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_workspace_name;
        private System.Windows.Forms.TextBox txt_workspace_name;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_create;
    }
}