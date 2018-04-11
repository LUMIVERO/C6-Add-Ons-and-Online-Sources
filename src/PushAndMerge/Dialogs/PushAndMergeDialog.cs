using SwissAcademic.Addons.PushAndMerge.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Licensing;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace SwissAcademic.Addons.PushAndMerge
{
    public partial class PushAndMergeDialog : FormBase
    {
        #region Enums
        enum TabKeys
        {
            TitleSelection,
            SelectTitleData,
            MergeTitleData,
            SelectTargetProject
        }
        #endregion

        #region Fields
        Project _sourceProject;
        PushAndMergeOptions _pushAndMergeOptions = new PushAndMergeOptions();
        #endregion

        #region Properties

        #region CurrentTabKey
        TabKeys _currentTabKey;
        TabKeys CurrentTabKey
        {
            get => _currentTabKey;
            set
            {
                if(value != _currentTabKey)
                {
                    SetCurrentTab(value);
                }
            }
        } 
        #endregion

        #endregion

        #region Constructor
        public PushAndMergeDialog(Form dialogOwner, Project sourceProject) : base(dialogOwner)
        {
            InitializeComponent();

            _sourceProject = sourceProject;

            ignoreOtherTitlesRadioButton.Checked = true;
            mergeSameIdsCheckbox.Checked = true;
            mergeStaticIdCheckbox.Checked = true;

            SetCurrentTab(TabKeys.TitleSelection);

        }
        #endregion

        #region Methods

        public override void Localize()
        {
            cancelButton.Text = PushAndMergeResources.CancelButton;
            backButton.Text = PushAndMergeResources.BackButton;

            base.Localize();
        }

        #region SetCurrentTab

        void SetCurrentTab(TabKeys key)
        {
            _currentTabKey = key;

            wizardTabControl.SelectedTab = wizardTabControl.Tabs[key.ToString()];

            switch (key)
            {
                case TabKeys.TitleSelection:
                    LocalizeTitleSelection();
                    InitTitleSelection(DialogOwner, _sourceProject);
                    break;
                case TabKeys.SelectTitleData:
                    LocalizeSelectTitleData(_sourceProject);
                    InitSelectTitleData();
                    break;
                case TabKeys.MergeTitleData:
                    LocalizeMergeTitleData();
                    InitMergeTitleData();
                    break;
                case TabKeys.SelectTargetProject:
                    LocalizeSelectProject();
                    InitSelectProject(_sourceProject);
                    break;
            }
        }

        #endregion

        #endregion

        #region Eventhandlers

        #region NextButtonClick
        async void NextButtonClick(object sender, EventArgs e)
        {
            switch(CurrentTabKey)
            {
                case TabKeys.TitleSelection:
                    SetTitleSelectionData();
                    if(ignoreOtherTitlesRadioButton.Checked)
                    {
                        CurrentTabKey = TabKeys.MergeTitleData;
                    }
                    else
                    {
                        CurrentTabKey = TabKeys.SelectTitleData;
                    }
                    break;
                case TabKeys.SelectTitleData:
                    SetSelectTitleData();
                    CurrentTabKey = TabKeys.MergeTitleData;
                    break;
                case TabKeys.MergeTitleData:
                    SetMergeTitleData();
                    CurrentTabKey = TabKeys.SelectTargetProject;
                    break;
                case TabKeys.SelectTargetProject:
                    await FinishAsync();
                    break;
            }
        }
        #endregion

        #region BackButtonClick
        void BackButtonClick(object sender, EventArgs e)
        {
            switch (CurrentTabKey)
            {
                case TabKeys.SelectTitleData:
                    SetSelectTitleData();
                    CurrentTabKey = TabKeys.TitleSelection;
                    break;
                case TabKeys.MergeTitleData:
                    SetMergeTitleData();
                    if(_pushAndMergeOptions.CopyAllNonMatchedReferences)
                    {
                        CurrentTabKey = TabKeys.SelectTitleData;
                    }
                    else
                    {
                        CurrentTabKey = TabKeys.TitleSelection;
                    }
                    break;
                case TabKeys.SelectTargetProject:
                    CurrentTabKey = TabKeys.MergeTitleData;
                    break;
            }
        }
        #endregion

        #endregion

        #region Events

        #region UpdateGui
        protected override void UpdateGui()
        {
            if(CurrentTabKey == TabKeys.SelectTargetProject)
            {
                nextButton.Enabled = projectTextEditor.SelectedItem != null;
            }
            else
            {
                nextButton.Enabled = true;
            }

            base.UpdateGui();
        }
        #endregion

        #endregion

        private void cancelButton_Click(object sender, EventArgs e)
        {

        }
    }
}
