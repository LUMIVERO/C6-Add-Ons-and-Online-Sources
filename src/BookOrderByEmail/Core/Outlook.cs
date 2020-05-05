using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SwissAcademic.Addons.BookOrderByEmail
{
    internal static class Outlook
    {
        #region Methods

        public static void Send(MailTemplate mailTemplate)
        {
            var outlook = GetOutlookApplication();
            Microsoft.Office.Interop.Outlook.MailItem mail = outlook.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            mail.Subject = mailTemplate.Subject;
            mail.Body = mailTemplate.Body;
            mailTemplate.To.ForEach(adress => mail.Recipients.Add(adress));
            mailTemplate.Attachments.ForEach(a => mail.Attachments.Add(a, Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing));
            mail.Display(true);
        }

        static Microsoft.Office.Interop.Outlook.Application GetOutlookApplication()
        {
            return Process.GetProcessesByName("OUTLOOK").Length > 0
                   ? Marshal.GetActiveObject("Outlook.Application") as Microsoft.Office.Interop.Outlook.Application
                   : new Microsoft.Office.Interop.Outlook.Application();
        }

        #endregion

        #region Properties

        public static bool IsInstalled => Type.GetTypeFromProgID("Outlook.Application") != null;

        #endregion
    }
}
