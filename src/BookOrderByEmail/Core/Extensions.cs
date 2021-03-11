using SwissAcademic.Addons.BookOrderByEmailAddon.Properties;
using SwissAcademic.Citavi;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SwissAcademic.Addons.BookOrderByEmailAddon
{
    internal static class Extensions
    {
        public static void OrderByEMail(this Reference reference, string receiver, string body)
        {
            var mailTemplate = reference.CreateMailTemplate(receiver, body);
            Outlook.Send(mailTemplate);
        }

        public static void OrderByClipboard(this Reference reference, string receiver, string body)
        {
            var mailTemplate = reference.CreateMailTemplate(receiver, body);
            Clipboard.SetText(mailTemplate.Body);
        }

        static MailTemplate CreateMailTemplate(this Reference reference, string receiver, string body)
        {
            var mail = new MailTemplate { Subject = Resources.Order + " " + reference.ShortTitle };

            if (!string.IsNullOrEmpty(receiver))
            {
                receiver
                    .Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                    .ForEach(adress => mail.To.Add(adress.Trim()));
            }

            var isbn = string.IsNullOrEmpty(reference.Isbn)
                       ? Resources.OrderByEMailBodyTextISBNMissing
                       : reference.Isbn.ToString();

            mail.Body = string.Format(Resources.OrderByEMailBodyText, reference.ToString(TextFormat.Text), isbn, body);
            return mail;
        }

        internal static void SetValueSafe<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return;
            }

            dictionary.Add(key, value);
        }

        internal static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue @default = default)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }


            return @default;
        }
    }
}
