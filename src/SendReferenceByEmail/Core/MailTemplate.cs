using System.Collections.Generic;

namespace SwissAcademic.Addons.SendReferenceByEmailAddon
{
    public class MailTemplate
    {
        public string Body { get; set; }

        public string Subject { get; set; }

        public List<string> Attachments { get; } = new List<string>();
    }
}
