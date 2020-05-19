using System.Collections.Generic;

namespace SwissAcademic.Addons.BookOrderByEmailAddon
{
    internal class MailTemplate
    {
        public string Body { get; set; }

        public string Subject { get; set; }

        public List<string> To { get; } = new List<string>();

        public List<string> Attachments { get; } = new List<string>();
    }
}
