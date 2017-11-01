using System.Collections.Generic;

namespace SwissAcademic.Addons.SendReferenceByEmail
{
    public class MailTemplate
    {
        #region Constructors

        public MailTemplate() => Attachments = new List<string>();

        #endregion

        #region Properties

        public string Body { get; set; }

        public string Subject { get; set; }

        public List<string> Attachments { get; }

        #endregion
    }
}
