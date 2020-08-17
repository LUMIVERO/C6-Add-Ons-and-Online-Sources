using Microsoft.Office.Interop.Outlook;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SwissAcademic.Addons.SendReferenceByEmailAddon
{
    public static class OutlookApplication
    {
        public static void SendEMail(MailTemplate mailTemplate)
        {
            var outlook = GetOutlookApplication();
            MailItem mail = outlook.CreateItem(OlItemType.olMailItem);
            mail.Subject = mailTemplate.Subject;
            mail.Body = mailTemplate.Body;
            mailTemplate.Attachments.ForEach(a => mail.Attachments.Add(a, OlAttachmentType.olByValue, Type.Missing, Type.Missing));
            mail.Display(true);
        }

        static Application GetOutlookApplication()
        {
            return Process.GetProcessesByName("OUTLOOK").Length > 0
                   ? Marshal.GetActiveObject("Outlook.Application") as Application
                   : new Application();
        }
    }
}
