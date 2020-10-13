using pdftron.PDF;
using SwissAcademic.Citavi.Shell;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SwissAcademic.Addons.ScrollSpeedInPdfPreview
{
    partial class Addon
    {
        void MainForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            if (sender is MainForm mainForm)
            {
                mainForm.FormClosed -= MainForm_FormClosed;

                if (mainForm.GetViewControl() is PDFViewWPF viewControl)
                {
                    viewControl.PreviewMouseWheel -= ViewControl_PreviewMouseWheel;
                }
            }
        }

        void ViewControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is PDFViewWPF viewControl)
            {
                var mainForm = System.Windows.Forms.Application.OpenForms.OfType<MainForm>().FirstOrDefault(mf => mf.GetViewControl() == viewControl);

                if (!e.Handled && VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(e.Source as Canvas)) is ScrollContentPresenter scrollContentPresenter && (!_onlyInFullScreenMode || mainForm.IsPreviewFullScreenForm))
                {
                    scrollContentPresenter.SetVerticalOffset(scrollContentPresenter.VerticalOffset - e.Delta * _scrollSpeed);
                    e.Handled = true;
                }
            }
        }
    }
}
