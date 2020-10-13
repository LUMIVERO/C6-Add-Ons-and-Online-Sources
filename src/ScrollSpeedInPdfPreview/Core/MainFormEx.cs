using pdftron.PDF;
using SwissAcademic.Citavi.Shell;
using System.Reflection;

namespace SwissAcademic.Addons.ScrollSpeedInPdfPreview
{
    internal static class MainFormEx
    {
        public static PDFViewWPF GetViewControl(this MainForm mainForm)
        {
            var pdfViewControl = mainForm.PreviewControl.GetType().GetProperty("PdfViewControl", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(mainForm.PreviewControl);
            return pdfViewControl?.GetType().GetProperty("Viewer", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(pdfViewControl) as PDFViewWPF;
        }
    }
}
