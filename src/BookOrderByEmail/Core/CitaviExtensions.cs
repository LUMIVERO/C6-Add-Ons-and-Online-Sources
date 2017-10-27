using SwissAcademic.Addons.BookOrderByEmail.Properties;
using SwissAcademic.Citavi;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SwissAcademic.Addons.BookOrderByEmail
{
    internal static class CitaviExtensions
    {
        public static void OrderByEMail(this Reference reference, Configuration configuration)
        {
            var mail = CreateMail(reference, configuration);
            Outlook.Send(mail);
        }

        public static void OrderByClipboard(this Reference reference, Configuration configuration)
        {
            var mail = CreateMail(reference, configuration);
            Clipboard.SetText(mail.Body);
        }

        static Mail CreateMail(Reference reference, Configuration configuration)
        {
            var mail = new Mail();

            if (!string.IsNullOrEmpty(configuration.Receiver))
            {
                var adresses = configuration.Receiver.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                adresses.ForEach(adress => mail.To.Add(adress.Trim()));
            }

            mail.Subject = BookOrderByEmailResources.Order;
            var isbn = reference.Isbn;
            if (string.IsNullOrEmpty(isbn))
            {
                isbn = BookOrderByEmailResources.OrderByEMailBodyTextISBNMissing;
            }

            mail.Body = string.Format(BookOrderByEmailResources.OrderByEMailBodyText, new string[] { reference.ToString(TextFormat.Text), isbn, configuration.Body });
            return mail;
        }
    }
}
