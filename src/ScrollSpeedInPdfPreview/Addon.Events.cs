using pdftron.PDF;
using SwissAcademic.Addons.ScrollSpeedInPdfPreview.Properties;
using SwissAcademic.Citavi.Shell;
using System;

namespace SwissAcademic.Addons.ScrollSpeedInPdfPreview
{
    partial class Addon
    {
        public override void OnApplicationIdle(MainForm mainForm)
        {
            var button = mainForm
                               .GetPreviewCommandbar(MainFormPreviewCommandbarId.Toolbar)
                               .GetCommandbarMenu(MainFormPreviewCommandbarMenuId.Tools)
                               .GetCommandbarButton(COMMAND_KEY);
            if (button != null)
            {
                button.Visible = mainForm.ActiveRightPaneTab == RightPaneTab.Preview && mainForm.PreviewControl.ActivePreviewType == Citavi.Shell.Controls.Preview.PreviewType.Pdf;
                button.Tool.SharedProps.Enabled = button.Visible;
            }
        }

        public override void OnBeforePerformingCommand(MainForm mainForm, Controls.BeforePerformingCommandEventArgs e)
        {
            if (e.Key.Equals(COMMAND_KEY, StringComparison.OrdinalIgnoreCase))
            {
                e.Handled = true;

                using (var dialog = new ScrollSpeedDialog(mainForm, _scrollSpeed, _onlyInFullScreenMode))
                {
                    if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
                    _scrollSpeed = dialog.ScrollSpeed;
                    _onlyInFullScreenMode = dialog.OnlyInFullScreenMode;
                    SaveScrollSpeedInSettings();
                }
            }
        }

        public override void OnHostingFormLoaded(MainForm mainForm)
        {
            LoadScrollSpeedFromSettings();

            mainForm.FormClosed += MainForm_FormClosed;

            var toolsMenu = mainForm
                            .GetPreviewCommandbar(MainFormPreviewCommandbarId.Toolbar)
                            .GetCommandbarMenu(MainFormPreviewCommandbarMenuId.Tools);

            var button = toolsMenu.InsertCommandbarButton(toolsMenu.Tool.Tools.IndexOf(toolsMenu.Tool.Tools["SkipPreviewEntryPage"]) - 1, COMMAND_KEY, Resources.Command_Text);
            button.Shortcut = (System.Windows.Forms.Shortcut)(System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G);
            button.HasSeparator = true;

            if (mainForm.GetViewControl() is PDFViewWPF viewControl)
            {
                viewControl.PreviewMouseWheel += ViewControl_PreviewMouseWheel;
            }
        }

        public override void OnLocalizing(MainForm mainForm)
        {
            var button = mainForm
                            .GetPreviewCommandbar(MainFormPreviewCommandbarId.Toolbar)
                            .GetCommandbarMenu(MainFormPreviewCommandbarMenuId.Tools)
                            .GetCommandbarButton(COMMAND_KEY);
            if (button != null)
            {
                button.Text = Resources.Command_Text;
            }
        }
    }
}
