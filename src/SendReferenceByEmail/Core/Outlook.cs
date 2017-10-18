using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using InteropOutlook = Microsoft.Office.Interop.Outlook;

namespace SwissAcademic.Addons.SendReferenceByEmail
{
    public static class Outlook
    {
        #region Methods

        public static void CreateEmail()
        {
            var oApp = GetApplicationObject();
            var oMail = oApp.CreateItem(InteropOutlook.OlItemType.olMailItem) as InteropOutlook.MailItem;
            oMail.Subject = "Subject";
            oMail.Body = "Body";
            oMail.To = "daniel.lutz@outllok.com";
            oMail.Display(true);
            //mail.Attachments.Add(@"c:\travel\itinerary.doc",InteropOutlook.OlAttachmentType.olByValue,Type.Missing, Type.Missing);
        }

        static InteropOutlook.Application GetApplicationObject()
        {

            InteropOutlook.Application application = null;

            // Check whether there is an Outlook process running.
            if (Process.GetProcessesByName("OUTLOOK").Count() > 0)
            {

                // If so, use the GetActiveObject method to obtain the process and cast it to an Application object.
                application = Marshal.GetActiveObject("Outlook.Application") as InteropOutlook.Application;
            }
            else
            {

                // If not, create a new instance of Outlook and log on to the default profile.
                application = new InteropOutlook.Application();
            }

            // Return the Outlook Application object.
            return application;
        }

        #endregion
    }
}
