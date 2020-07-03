using SwissAcademic.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.ReferenceGridFormWorkSpaceEditorAddon
{
    public partial class WorkSpaceNameEditor : FormBase
    {
        #region Fields

        readonly bool _editMode;
        readonly string _caption;
        readonly IEnumerable<string> _captions;

        #endregion

        #region Constructors

        public WorkSpaceNameEditor(Form owner, IEnumerable<string> captions) : base(owner)
        {
            InitializeComponent();
            Owner = owner;
            _captions = captions;
        }

        public WorkSpaceNameEditor(Form owner, IEnumerable<string> captions, string caption) : this(owner, captions)
        {
            txt_workspace_name.Text = caption;
            _caption = caption;
            _editMode = true;
        }

        #endregion

        #region Events

        protected override void OnLoad(EventArgs e)
        {
            Localize();
            base.OnLoad(e);
        }

        #endregion

        #region Methods

        public override void Localize()
        {
            base.Localize();
            Text = Properties.Resources.NameEditor_Form_Text;
            lbl_workspace_name.Text = Properties.Resources.NameEditor_Label_Name;
            btn_cancel.Text = Properties.Resources.NameEditor_Button_Cancel;
            btn_create.Text = _editMode ? Properties.Resources.NameEditor_Button_Rename : Properties.Resources.NameEditor_Button_Create;
        }

        protected override void OnApplicationIdle()
        {
            btn_create.Enabled =
                !string.IsNullOrEmpty(txt_workspace_name.Text.Trim()) &&
                ((_editMode && txt_workspace_name.Text.Trim().Equals(_caption, StringComparison.OrdinalIgnoreCase)) || !_captions.Any(c => c.Equals(txt_workspace_name.Text.Trim(), StringComparison.OrdinalIgnoreCase)));
            base.OnApplicationIdle();
        }

        #endregion

        #region Properties

        public string WorkSpaceName => txt_workspace_name.Text;

        #endregion

        #region EventHandlers

        void Txt_workspace_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && !string.IsNullOrEmpty(txt_workspace_name.Text))
            {
                DialogResult = DialogResult.OK;
            }
        }

        #endregion
    }
}
