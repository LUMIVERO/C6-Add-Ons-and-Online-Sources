namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor
{
    partial class WorkSpaceEditor
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
            this.lb_workspaces = new System.Windows.Forms.ListBox();
            this.btn_remove = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_down = new System.Windows.Forms.Button();
            this.btn_up = new System.Windows.Forms.Button();
            this.btn_edit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_workspaces
            // 
            this.lb_workspaces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_workspaces.FormattingEnabled = true;
            this.lb_workspaces.ItemHeight = 15;
            this.lb_workspaces.Location = new System.Drawing.Point(20, 20);
            this.lb_workspaces.Name = "lb_workspaces";
            this.lb_workspaces.Size = new System.Drawing.Size(244, 321);
            this.lb_workspaces.TabIndex = 0;
            // 
            // btn_remove
            // 
            this.btn_remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_remove.Image = global::SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Properties.Resources.remove;
            this.btn_remove.Location = new System.Drawing.Point(277, 49);
            this.btn_remove.Name = "btn_remove";
            this.btn_remove.Size = new System.Drawing.Size(25, 23);
            this.btn_remove.TabIndex = 2;
            this.btn_remove.UseVisualStyleBackColor = true;
            this.btn_remove.Click += new System.EventHandler(this.Btn_remove_Click);
            // 
            // btn_add
            // 
            this.btn_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_add.Image = global::SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Properties.Resources.add;
            this.btn_add.Location = new System.Drawing.Point(277, 20);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(25, 23);
            this.btn_add.TabIndex = 1;
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.Btn_add_Click);
            // 
            // btn_down
            // 
            this.btn_down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_down.Image = global::SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Properties.Resources.down;
            this.btn_down.Location = new System.Drawing.Point(277, 136);
            this.btn_down.Name = "btn_down";
            this.btn_down.Size = new System.Drawing.Size(25, 24);
            this.btn_down.TabIndex = 5;
            this.btn_down.UseVisualStyleBackColor = true;
            this.btn_down.Click += new System.EventHandler(this.Btn_down_Click);
            // 
            // btn_up
            // 
            this.btn_up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_up.Image = global::SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Properties.Resources.up;
            this.btn_up.Location = new System.Drawing.Point(277, 107);
            this.btn_up.Name = "btn_up";
            this.btn_up.Size = new System.Drawing.Size(25, 23);
            this.btn_up.TabIndex = 4;
            this.btn_up.UseVisualStyleBackColor = true;
            this.btn_up.Click += new System.EventHandler(this.Btn_up_Click);
            // 
            // btn_edit
            // 
            this.btn_edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_edit.Image = global::SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Properties.Resources.edit;
            this.btn_edit.Location = new System.Drawing.Point(277, 78);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(25, 23);
            this.btn_edit.TabIndex = 3;
            this.btn_edit.UseVisualStyleBackColor = true;
            this.btn_edit.Click += new System.EventHandler(this.Btn_edit_Click);
            // 
            // WorkSpaceEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 361);
            this.Controls.Add(this.btn_edit);
            this.Controls.Add(this.btn_remove);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.btn_down);
            this.Controls.Add(this.btn_up);
            this.Controls.Add(this.lb_workspaces);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WorkSpaceEditor";
            this.Padding = new System.Windows.Forms.Padding(20, 20, 50, 20);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WorkSpaceEditor";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox lb_workspaces;
        private System.Windows.Forms.Button btn_up;
        private System.Windows.Forms.Button btn_down;
        private System.Windows.Forms.Button btn_remove;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_edit;
    }
}