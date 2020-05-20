using SwissAcademic.Addons.BookOrderByEmailAddon.Properties;
using SwissAcademic.Citavi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SwissAcademic.Addons.BookOrderByEmailAddon
{
    internal static class Extensions
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
            var mail = new MailTemplate { Subject = Resources.Order };

            if (!string.IsNullOrEmpty(configuration.Receiver))
            {
                configuration
                    .Receiver
                    .Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => v.Trim())
                    .ForEach(adress => mail.To.Add(adress));
            }

            var isbn = string.IsNullOrEmpty(reference.Isbn)
                       ? Resources.OrderByEMailBodyTextISBNMissing
                       : reference.Isbn.ToString();

            mail.Body = string.Format(Resources.OrderByEMailBodyText, reference.ToString(TextFormat.Text), isbn, configuration.Body);
            return mail;
        }

        internal static void AddSafe<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return;
            }

            dictionary.Add(key, value);
        }

        internal static TValue GetSafe<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }


            return defaultValue;
        }
    }
}
