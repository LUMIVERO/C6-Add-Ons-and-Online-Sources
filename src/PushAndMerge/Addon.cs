using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwissAcademic;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Controls;
using SwissAcademic.Drawing;
using System.Windows.Forms;
using SwissAcademic.Addons.PushAndMerge.Properties;

namespace SwissAcademic.Addons.PushAndMerge
{
    public class Addon : CitaviAddOn
    {
        #region Constants
        const string CommandOpenPushAndMergeDialogKey = "SwissAcademic.Addons.PushAndMerge.OpenPushAndMergeDialogKey";
        #endregion

        #region Properties

        #region HostingForm
        public override AddOnHostingForm HostingForm
        {
            get { return AddOnHostingForm.MainForm; }
        }
        #endregion

        #endregion

        #region Methods

        protected override void OnApplicationIdle(System.Windows.Forms.Form form)
        {
            base.OnApplicationIdle(form);
        }

        protected override void OnBeforePerformingCommand(BeforePerformingCommandEventArgs e)
        {
            base.OnBeforePerformingCommand(e);

            var mainForm = e.Form as MainForm;

            switch(e.Key)
            {
                case CommandOpenPushAndMergeDialogKey:
                    {
                        e.Handled = true;

                        if (mainForm == null) return;

                        using (var dialog = new PushAndMergeDialog(e.Form, mainForm.Project))
                        {
                            dialog.Text = "Copy & Merge Knowledge";
                            dialog.ShowDialog();
                        }
                    }
                    break;
            }
        }

        protected override void OnChangingColorScheme(System.Windows.Forms.Form form, ColorScheme colorScheme)
        {
            base.OnChangingColorScheme(form, colorScheme);
        }

        protected override void OnHostingFormLoaded(System.Windows.Forms.Form form)
        {
            if (form is MainForm mainForm)
            {
                mainForm.GetMainCommandbarManager()
                        .GetReferenceEditorCommandbar(MainFormReferenceEditorCommandbarId.Menu)
                        .GetCommandbarMenu(MainFormReferenceEditorCommandbarMenuId.References)
                        .InsertCommandbarButton(4, CommandOpenPushAndMergeDialogKey, PushAndMergeResources.PushAndMergeCommandButtonText);
            }

            base.OnHostingFormLoaded(form);
        }

        protected override void OnLocalizing(System.Windows.Forms.Form form)
        {
            base.OnLocalizing(form);
        }

        #endregion
    }
}