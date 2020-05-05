using SwissAcademic.Addons.BookOrderByEmailAddon.Properties;
using SwissAcademic.Citavi;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.BookOrderByEmailAddon
{
    internal static class CitaviExtensions
    {
        public static void OrderByEMail(this Reference reference, Configuration configuration)
        {
            var mailTemplate = reference.CreateMailTemplate(configuration);
            Outlook.Send(mailTemplate);
        }

        public static void OrderByClipboard(this Reference reference, Configuration configuration)
        {
            var mailTemplate = reference.CreateMailTemplate(configuration);
            Clipboard.SetText(mailTemplate.Body);
        }

        static MailTemplate CreateMailTemplate(this Reference reference, Configuration configuration)
        {
            var mail = new MailTemplate
            {
                Subject = Resources.Order

            };

            if (!string.IsNullOrEmpty(configuration.Receiver))
            {
                var adresses = configuration.Receiver
                                            .Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(v => v.Trim())
                                            .ToList();

                mail.To.AddRange(adresses);
            }

            var isbn = string.IsNullOrEmpty(reference.Isbn)
                       ? Resources.OrderByEMailBodyTextISBNMissing
                       : reference.Isbn.ToString();

            mail.Body = string.Format(Resources.OrderByEMailBodyText, reference.ToString(TextFormat.Text), isbn, configuration.Body);
            return mail;
        }
    }
}
