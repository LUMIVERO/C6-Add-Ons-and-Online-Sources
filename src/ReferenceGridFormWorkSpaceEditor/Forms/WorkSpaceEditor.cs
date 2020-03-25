using SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor.Properties;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditor
{
    public partial class WorkSpaceEditor : FormBase
    {
        #region Fields

        ReferenceGridForm _referenceGridForm;
        AddonSettings _settings;

        #endregion

        #region Constructors

        public WorkSpaceEditor(ReferenceGridForm owner, AddonSettings settings) : base(owner)
        {
            _referenceGridForm = owner;
            _settings = settings;
            InitializeComponent();
        }

        #endregion

        #region EventHandlers

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeListBox(_settings);

            btn_add.Image = Control2.ScaleBitmap(Resources.add);
            btn_down.Image = Control2.ScaleBitmap(Resources.down);
            btn_edit.Image = Control2.ScaleBitmap(Resources.edit);
            btn_remove.Image = Control2.ScaleBitmap(Resources.remove);
            btn_up.Image = Control2.ScaleBitmap(Resources.up);
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
            foreach (var workSpace in settings.WorkSpaces)
            {
                lb_workspaces.Items.Add(workSpace);
            }
        }

        public override void Localize()
        {
            base.Localize();

            Text = Properties.Resources.WorkSpaceEditor_Form_Text;
        }

        #endregion

        #region Events

        void Btn_add_Click(object sender, EventArgs e)
        {
            using (var form = new WorkSpaceNameEditor(this, lb_workspaces.Items.Cast<WorkSpace>().Select(ws => ws.Caption).ToList()))
            {
                if (form.ShowDialog(this) == DialogResult.Cancel) return;
                var workSpace = _referenceGridForm.CreateWorkSpaceByName(form.WorkSpaceName);
                lb_workspaces.Items.Add(workSpace);
                lb_workspaces.Refresh();
            }
        }

        void Btn_remove_Click(object sender, EventArgs e)
        {
            if (lb_workspaces.SelectedItem is WorkSpace workSpace)
            {
                if (MessageBox.Show(Properties.Resources.WorkSpaceEditor_Messages_RemoveWorkSpace.FormatString(workSpace.Caption), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    lb_workspaces.Items.RemoveAt(lb_workspaces.SelectedIndex);
                }
            }
        }

        void Btn_up_Click(object sender, EventArgs e)
        {
            if (lb_workspaces.SelectedItem is WorkSpace workSpace)
            {
                var index = lb_workspaces.SelectedIndex;
                lb_workspaces.Items.RemoveAt(index);
                lb_workspaces.Items.Insert(index - 1, workSpace);
                lb_workspaces.SelectedItem = workSpace;
            }
        }

        void Btn_down_Click(object sender, EventArgs e)
        {
            if (lb_workspaces.SelectedItem is WorkSpace workSpace)
            {
                var index = lb_workspaces.SelectedIndex;
                lb_workspaces.Items.RemoveAt(index);
                lb_workspaces.Items.Insert(index + 1, workSpace);
                lb_workspaces.SelectedItem = workSpace;
            }
        }

        void Btn_edit_Click(object sender, EventArgs e)
        {
            if (lb_workspaces.SelectedItem is WorkSpace workspace)
            {
                using (var form = new WorkSpaceNameEditor(this, lb_workspaces.Items.Cast<WorkSpace>().Select(ws => ws.Caption).ToList(), workspace.Caption))
                {
                    if (form.ShowDialog(this) == DialogResult.Cancel) return;

                    workspace.Caption = form.WorkSpaceName;
                }

                lb_workspaces.Invoke("RefreshItems");
            }
        }

        #endregion
    }
}
