using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor
{
    public partial class WorkSpaceEditor : FormBase
    {
        #region Constructors

        public WorkSpaceEditor(Form owner, AddonSettings settings) : base(owner)
        {
            InitializeComponent();
            InitializeListBox(settings);
        }

        #endregion

        #region EventHandlers

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnApplicationIdle()
        {
            base.OnApplicationIdle();
            btn_remove.Enabled = lb_workspaces.SelectedIndex != -1;
            btn_up.Enabled = lb_workspaces.SelectedIndex != -1 && lb_workspaces.SelectedIndex != 0;
            btn_down.Enabled = lb_workspaces.SelectedIndex != -1 && lb_workspaces.SelectedIndex != lb_workspaces.Items.Count - 1;
            btn_edit.Enabled = lb_workspaces.SelectedIndex != -1;
        }


        #endregion

        #region Properties

        public AddonSettings Settings
        {
            get
            {
                var settings = AddonSettings.Default;
                foreach (var item in lb_workspaces.Items)
                {
                    if (item is WorkSpace workSpace)
                    {
                        settings.WorkSpaces.Add(workSpace);
                    }
                }
                return settings;
            }
        }

        #endregion

        #region Methods

        void InitializeListBox(AddonSettings settings)
        {

        }

        public override void Localize()
        {
            base.Localize();

            Text = Properties.ReferenceGridFormWorkSpaceEditorResources.WorkSpaceEditor_Form_Text;
        }

        #endregion

        #region Events

        void Btn_close_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        void Btn_add_Click(object sender, EventArgs e)
        {
            using (var form = new WorkSpaceNameEditor(this))
            {
                if (form.ShowDialog() == DialogResult.Cancel) return;
                var workSpace = ((ReferenceGridForm)form.Owner).CreateWorkSpaceByName(form.WorkSpaceName);
                lb_workspaces.Items.Add(workSpace);
                lb_workspaces.Update();
            }
        }

        void Btn_remove_Click(object sender, EventArgs e)
        {

        }

        void Btn_up_Click(object sender, EventArgs e)
        {

        }

        void Btn_down_Click(object sender, EventArgs e)
        {

        }

        void Btn_edit_Click(object sender, EventArgs e)
        {
            if (lb_workspaces.SelectedItem is WorkSpace workspace)
            {
                using (var form = new WorkSpaceNameEditor(this, workspace.Caption))
                {
                    if (form.ShowDialog() == DialogResult.Cancel) return;

                    workspace.Caption = form.WorkSpaceName;
                }

                lb_workspaces.Update();
            }
        }

        #endregion


    }
}
